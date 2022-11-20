using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Downloader;
using CmlLib.Core.Installer.FabricMC;
using CmlLib.Core.VersionMetadata;
using OpaqueCamp.Launcher.Core.Memory;

namespace OpaqueCamp.Launcher.Core;

public sealed class CmlLibMinecraftRunner
{
    private readonly ICurrentAccountProvider _currentAccountProvider;
    private readonly IDownloadSpeedupService _downloadSpeedupService;
    private readonly IJvmMemorySettings _jvmMemorySettings;
    private readonly IMinecraftFilesDirProvider _minecraftFilesDirProvider;
    private readonly IModPackInfoProvider _modPackInfoProvider;
    private readonly IModsInstaller _modsInstaller;
    private readonly IServerConfigProvider _serverConfigProvider;

    public CmlLibMinecraftRunner(IModPackInfoProvider modPackInfoProvider, IJvmMemorySettings jvmMemorySettings,
        IServerConfigProvider serverConfigProvider, IDownloadSpeedupService downloadSpeedupService,
        IMinecraftFilesDirProvider minecraftFilesDirProvider, ICurrentAccountProvider currentAccountProvider,
        IModsInstaller modsInstaller)
    {
        _modPackInfoProvider = modPackInfoProvider;
        _jvmMemorySettings = jvmMemorySettings;
        _serverConfigProvider = serverConfigProvider;
        _downloadSpeedupService = downloadSpeedupService;
        _minecraftFilesDirProvider = minecraftFilesDirProvider;
        _currentAccountProvider = currentAccountProvider;
        _modsInstaller = modsInstaller;
    }

    private MinecraftPath MinecraftPath => new(_minecraftFilesDirProvider.DirPathForMinecraftFiles);

    public async Task<MinecraftCrashLogs?> RunMinecraftAsync(
        DownloadFileChangedHandler? onCurrentlyDownloadedFileChange,
        Action<int> onDownloadPercentageChange)
    {
        _downloadSpeedupService.MakeDownloadsFaster();
        var launcher = new CMLauncher(MinecraftPath);
        launcher.FileChanged += onCurrentlyDownloadedFileChange;
        launcher.ProgressChanged += (_, args) => onDownloadPercentageChange(args.ProgressPercentage);

        var fabricVersion = await GetFabricVersionForOurMinecraftVersion();
        await fabricVersion.SaveAsync(MinecraftPath);

        await _modsInstaller.InstallModsAsync();

        var process = await launcher.CreateProcessAsync(fabricVersion.Name,
            new MLaunchOption
            {
                MinimumRamMb = _jvmMemorySettings.InitialMemoryAllocation.Megabytes,
                MaximumRamMb = _jvmMemorySettings.MaximumMemoryAllocation.Megabytes,
                Session = MSession.GetOfflineSession(GetAccountUsername()),
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

    private async Task<MVersionMetadata> GetFabricVersionForOurMinecraftVersion()
    {
        var fabricVersionLoader = new FabricVersionLoader();
        var fabricVersions = await fabricVersionLoader.GetVersionMetadatasAsync();
        var fabricForOurMinecraftVersion = fabricVersions
            .First(fabricVersion =>
                fabricVersion.Name.Split('-').Last() == _modPackInfoProvider.UsedMinecraftVersion.ToString());
        return fabricForOurMinecraftVersion;
    }

    private string GetAccountUsername()
    {
        return (_currentAccountProvider.CurrentAccount ?? throw new CurrentAccountIsNullException()).Username;
    }
}