using System.Diagnostics;
using OpaqueCamp.Launcher.Core.Memory;

namespace OpaqueCamp.Launcher.Core;

// TODO: Refactor and write tests
public sealed class MinecraftRunner
{
    private readonly JavaFinder _javaFinder;
    private readonly ILauncherInfoProvider _launcherInfoProvider;
    private readonly IPathProvider _pathProvider;
    private readonly ClasspathProvider _classpathProvider;
    private readonly IJvmMemorySettings _jvmMemorySettings;

    public MinecraftRunner(JavaFinder javaFinder, ILauncherInfoProvider launcherInfoProvider,
        IPathProvider pathProvider, ClasspathProvider classpathProvider, IJvmMemorySettings jvmMemorySettings)
    {
        _javaFinder = javaFinder;
        _launcherInfoProvider = launcherInfoProvider;
        _pathProvider = pathProvider;
        _classpathProvider = classpathProvider;
        _jvmMemorySettings = jvmMemorySettings;
    }

    /// <summary>
    /// Runs the Minecraft client asynchronously.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> which completes right after Minecraft exits.
    /// The task contains <c>null</c> if Minecraft exited successfully,
    /// or a <see cref="MinecraftCrashLogs"/> instance if it crashed.
    /// </returns>
    /// <exception cref="MinecraftStartFailureException">
    /// Thrown when the Minecraft client could not be started due to various Java-related problems,
    /// such as missing Java installation or classpath generation problems.
    /// The inner exception will contain the error that caused the startup to fail.
    /// </exception>
    public async Task<MinecraftCrashLogs?> RunMinecraftAsync()
    {
        var creds = new PlayerCredentials("lectureNice", "22d5ed98cb934e279b94eaa26f2ba401",
            "eyJhbGciOiJIUzI1NiJ9.eyJ4dWlkIjoiMjUzNTQyNDU2NDIyNDA5OCIsImFnZyI6IkFkdWx0Iiwic3ViIjoiZjFkNTgxZmYtN2NlZS00ZjZiLThlN2MtMTFmNjVjZmFhMWYzIiwibmJmIjoxNjY3NzM4MjM0LCJhdXRoIjoiWEJPWCIsInJvbGVzIjpbXSwiaXNzIjoiYXV0aGVudGljYXRpb24iLCJleHAiOjE2Njc4MjQ2MzQsImlhdCI6MTY2NzczODIzNCwicGxhdGZvcm0iOiJVTktOT1dOIiwieXVpZCI6Ijg0MzAxZjU1ODZhYmQyZGFjMDIxYmNkZWRiMDc3NjI0In0.oEU-cDcc0ps0AMZHEesPfeEqs4aDlJ2CBm6B4c16DRI");
        var startInfo = new ProcessStartInfo
        {
            FileName = GetJavawExePath(),
            Arguments = string.Join(' ', Args(creds)),
            ErrorDialog = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        var process = Process.Start(startInfo);
        if (process is null)
        {
            throw new MinecraftStartFailureException("Не удалось запустить Minecraft по неизвестной причине.");
        }

        await process.WaitForExitAsync();
        if (process.ExitCode == 0)
        {
            return null;
        }

        return new MinecraftCrashLogs(await process.StandardOutput.ReadToEndAsync(),
            await process.StandardError.ReadToEndAsync());
    }

    private IEnumerable<string> Args(PlayerCredentials credentials)
    {
        var list = new List<string>();
        list.AddRange(jvmArgs);
        list.Add($"-Xms{_jvmMemorySettings.InitialMemoryAllocation.Megabytes}M");
        list.Add($"-Xmx{_jvmMemorySettings.MaximumMemoryAllocation.Megabytes}M");
        list.AddRange(launcherArgs);
        list.AddMany("-cp", GetClasspath());
        list.AddRange(fabricArgs);
        list.AddMany("--username", credentials.UserName);
        list.AddMany("--version",
            $"{_launcherInfoProvider.LauncherName} {_launcherInfoProvider.LauncherVersion}".Quoted());
        list.AddMany("--gameDir", _pathProvider.GameDirectoryPath.Quoted());
        list.AddMany("--assetsDir", _pathProvider.AssetsDirectoryPath.Quoted());
        list.AddMany("--assetIndex", _launcherInfoProvider.LauncherVersion.MajorAndMinorString);
        list.AddMany("--uuid", credentials.Uuid);
        list.AddMany("--accessToken", credentials.AccessToken);
        list.AddMany("--userType", "mojang");
        list.AddMany("--versionType", "release");

        return list;
    }

    private string GetJavawExePath()
    {
        try
        {
            return _javaFinder.GetJavawExePath();
        }
        catch (JavaNotFoundException e)
        {
            throw new MinecraftStartFailureException(innerException: e);
        }
    }

    private string GetClasspath()
    {
        try
        {
            return _classpathProvider.GetClasspath().Quoted();
        }
        catch (ClasspathGenerationException e)
        {
            throw new MinecraftStartFailureException(innerException: e);
        }
    }

    private static readonly List<string> jvmArgs = new()
    {
        "-XX:+UnlockExperimentalVMOptions",
        "-XX:+UseG1GC",
        "-XX:G1NewSizePercent=20",
        "-XX:G1ReservePercent=20",
        "-XX:MaxGCPauseMillis=50",
        "-XX:G1HeapRegionSize=32M",
        "-XX:+DisableExplicitGC",
        "-XX:+AlwaysPreTouch",
        "-XX:+ParallelRefProcEnabled",
        "-Dfile.encoding=UTF-8",
        "-Xss1M",
    };

    private static readonly List<string> launcherArgs = new()
    {
        "-Dminecraft.launcher.brand=java-minecraft-launcher",
        "-Dminecraft.launcher.version=1.6.93",
    };

    private static readonly List<string> fabricArgs = new()
    {
        "-DFabricMcEmu=net.minecraft.client.main.Main",
        "net.fabricmc.loader.impl.launch.knot.KnotClient",
    };

    // Adds OptiFine Tweak for future
    private static readonly List<string> optfineArgs = new()
    {
        "--tweakClass",
        "optifine.OptiFineTweaker"
    };
}