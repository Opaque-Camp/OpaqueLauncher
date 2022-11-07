using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpaqueLauncher.Core;

public sealed class MinecraftStarter
{
    private readonly JavaFinder _javaFinder;
    private readonly LauncherInfoProvider _launcherVersionProvider;
    private readonly DirectoryPathProvider _directoryPathProvider;
    private readonly ClasspathProvider _classpathProvider;

    public MinecraftStarter(JavaFinder javaFinder, LauncherInfoProvider launcherVersionProvider, DirectoryPathProvider directoryPathProvider, ClasspathProvider classpathProvider)
    {
        _javaFinder = javaFinder;
        _launcherVersionProvider = launcherVersionProvider;
        _directoryPathProvider = directoryPathProvider;
        _classpathProvider = classpathProvider;
    }

    void StartMinecraft()
    {
        var creds = new PlayerCredentials("lectureNice", "22d5ed98cb934e279b94eaa26f2ba401", "eyJhbGciOiJIUzI1NiJ9.eyJ4dWlkIjoiMjUzNTQyNDU2NDIyNDA5OCIsImFnZyI6IkFkdWx0Iiwic3ViIjoiZjFkNTgxZmYtN2NlZS00ZjZiLThlN2MtMTFmNjVjZmFhMWYzIiwibmJmIjoxNjY3NzM4MjM0LCJhdXRoIjoiWEJPWCIsInJvbGVzIjpbXSwiaXNzIjoiYXV0aGVudGljYXRpb24iLCJleHAiOjE2Njc4MjQ2MzQsImlhdCI6MTY2NzczODIzNCwicGxhdGZvcm0iOiJVTktOT1dOIiwieXVpZCI6Ijg0MzAxZjU1ODZhYmQyZGFjMDIxYmNkZWRiMDc3NjI0In0.oEU-cDcc0ps0AMZHEesPfeEqs4aDlJ2CBm6B4c16DRI");
        Process.Start("", string.Join(' ', Args(creds)));
    }

    private IEnumerable<string> Args(PlayerCredentials credentials)
    {
        var list = new List<string> { _javaFinder.FindAbsoluteJavaExePath() };
        list.AddRange(jvmArgs);
        list.AddRange(launcherArgs);
        list.AddRange(new List<string> { "-cp", _classpathProvider.GetClasspath() });
        list.AddRange(fabricArgs);
        list.AddRange(new List<string> { "--username", credentials.UserName });
        list.AddRange(new List<string> { "--version", $"{_launcherVersionProvider.LauncherName} {_launcherVersionProvider.LauncherVersion}" });
        list.AddRange(new List<string> { "--gameDir", _directoryPathProvider.GameDirectoryPath });
        list.AddRange(new List<string> { "--assetsDir", _directoryPathProvider.AssetsDirectoryPath });
        list.AddRange(new List<string> { "--assetIndex", _launcherVersionProvider.LauncherVersion.MajorAndMinorString });
        list.AddRange(new List<string> { "--uuid", credentials.Uuid });
        list.AddRange(new List<string> { "--accessToken", credentials.AccessToken });
        list.AddRange(new List<string> { "--userType", "mojang" });
        list.AddRange(new List<string> { "--versionType", "release" });

        return list;
    }

    private static readonly List<string> jvmArgs = new()
    {
        "-XX:+UnlockExperimentalVMOptions",
        "-XX:+UseG1GC",
        "-XX:G1NewSizePercent = 20",
        "-XX:G1ReservePercent = 20",
        "-XX:MaxGCPauseMillis = 50",
        "-XX:G1HeapRegionSize = 32M",
        "-XX:+DisableExplicitGC",
        "-XX:+AlwaysPreTouch",
        "-XX:+ParallelRefProcEnabled",
        "-Xms2048M",
        "-Xmx4096M",
        "-Dfile.encoding = UTF-8",
        "-Xss1M",
    };

    private static readonly List<string> launcherArgs = new()
    {
        "-Dminecraft.launcher.brand = java-minecraft-launcher",
        "-Dminecraft.launcher.version = 1.6.93",
    };

    private static readonly List<string> fabricArgs = new()
    {
        "-DFabricMcEmu=net.minecraft.client.main.Main",
        "net.fabricmc.loader.impl.launch.knot.KnotClient",
    };
}
