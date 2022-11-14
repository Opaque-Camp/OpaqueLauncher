using System.Net;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Downloader;
using OpaqueCamp.Launcher.Core.Memory;

namespace OpaqueCamp.Launcher.Core;

public sealed class CmlLibMinecraftRunner
{
    private readonly IModPackInfoProvider _modPackInfoProvider;
    private readonly IJvmMemorySettings _jvmMemorySettings;

    public CmlLibMinecraftRunner(IModPackInfoProvider modPackInfoProvider, IJvmMemorySettings jvmMemorySettings)
    {
        _modPackInfoProvider = modPackInfoProvider;
        _jvmMemorySettings = jvmMemorySettings;
    }

    public async Task<MinecraftCrashLogs?> RunMinecraftAsync(
        DownloadFileChangedHandler? onCurrentlyDownloadedFileChange,
        Action<int> onDownloadPercentageChange)
    {
        ServicePointManager.DefaultConnectionLimit = 256;
        var path = new MinecraftPath("opaque-vanilla");
        var launcher = new CMLauncher(path);
        launcher.FileChanged += onCurrentlyDownloadedFileChange;
        launcher.ProgressChanged += (_, args) => onDownloadPercentageChange(args.ProgressPercentage);
        var process = await launcher.CreateProcessAsync(_modPackInfoProvider.UsedMinecraftVersion.ToString(),
            new MLaunchOption
            {
                MinimumRamMb = _jvmMemorySettings.InitialMemoryAllocation.Megabytes,
                MaximumRamMb = _jvmMemorySettings.MaximumMemoryAllocation.Megabytes,
                Session = MSession.GetOfflineSession("OpaqueLauncher"),
                ServerIp = "oc.aboba.host",
                GameLauncherName = _modPackInfoProvider.ModPackName,
                GameLauncherVersion = _modPackInfoProvider.UsedMinecraftVersion.ToString()
            });
        process.Start();
        await process.WaitForExitAsync();
        return process.ExitCode == 0
            ? null
            : new MinecraftCrashLogs(await process.StandardOutput.ReadToEndAsync(),
                await process.StandardError.ReadToEndAsync());
    }
}