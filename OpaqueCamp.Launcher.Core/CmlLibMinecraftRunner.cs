using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Downloader;
using OpaqueCamp.Launcher.Core.Memory;

namespace OpaqueCamp.Launcher.Core;

public sealed class CmlLibMinecraftRunner
{
    private readonly IModPackInfoProvider _modPackInfoProvider;
    private readonly IJvmMemorySettings _jvmMemorySettings;
    private readonly IServerConfigProvider _serverConfigProvider;
    private readonly IDownloadSpeedupService _downloadSpeedupService;
    private readonly IMinecraftFilesDirProvider _minecraftFilesDirProvider;

    public CmlLibMinecraftRunner(IModPackInfoProvider modPackInfoProvider, IJvmMemorySettings jvmMemorySettings,
        IServerConfigProvider serverConfigProvider, IDownloadSpeedupService downloadSpeedupService,
        IMinecraftFilesDirProvider minecraftFilesDirProvider)
    {
        _modPackInfoProvider = modPackInfoProvider;
        _jvmMemorySettings = jvmMemorySettings;
        _serverConfigProvider = serverConfigProvider;
        _downloadSpeedupService = downloadSpeedupService;
        _minecraftFilesDirProvider = minecraftFilesDirProvider;
    }

    public async Task<MinecraftCrashLogs?> RunMinecraftAsync(
        DownloadFileChangedHandler? onCurrentlyDownloadedFileChange,
        Action<int> onDownloadPercentageChange)
    {
        _downloadSpeedupService.MakeDownloadsFaster();
        var path = new MinecraftPath(_minecraftFilesDirProvider.DirPathForMinecraftFiles);
        var launcher = new CMLauncher(path);
        launcher.FileChanged += onCurrentlyDownloadedFileChange;
        launcher.ProgressChanged += (_, args) => onDownloadPercentageChange(args.ProgressPercentage);
        var process = await launcher.CreateProcessAsync(_modPackInfoProvider.UsedMinecraftVersion.ToString(),
            new MLaunchOption
            {
                MinimumRamMb = _jvmMemorySettings.InitialMemoryAllocation.Megabytes,
                MaximumRamMb = _jvmMemorySettings.MaximumMemoryAllocation.Megabytes,
                Session = MSession.GetOfflineSession("OpaqueLauncher"),
                ServerIp = _serverConfigProvider.ServerAddress,
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