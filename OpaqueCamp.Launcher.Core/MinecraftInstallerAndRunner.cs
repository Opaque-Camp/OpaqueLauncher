using System.ComponentModel;
using CmlLib.Core;
using CmlLib.Core.Downloader;

namespace OpaqueCamp.Launcher.Core;

public sealed class MinecraftInstallerAndRunner : IMinecraftInstallerAndRunner
{
    private readonly IMinecraftLauncherFactory _launcherFactory;
    private readonly IMinecraftLaunchOptionsProvider _launchOptionsProvider;
    private readonly IMinecraftVersionMetadataProvider _versionMetadataProvider;
    private readonly IMinecraftFilesDirProvider _minecraftFilesDirProvider;
    private readonly IModsInstaller _modsInstaller;
    private readonly IMinecraftCrashReportReader _crashReportReader;

    public MinecraftInstallerAndRunner(IMinecraftLauncherFactory launcherFactory,
        IMinecraftLaunchOptionsProvider launchOptionsProvider,
        IMinecraftVersionMetadataProvider versionMetadataProvider,
        IMinecraftFilesDirProvider minecraftFilesDirProvider,
        IModsInstaller modsInstaller, IMinecraftCrashReportReader crashReportReader)
    {
        _launcherFactory = launcherFactory;
        _launchOptionsProvider = launchOptionsProvider;
        _versionMetadataProvider = versionMetadataProvider;
        _minecraftFilesDirProvider = minecraftFilesDirProvider;
        _modsInstaller = modsInstaller;
        _crashReportReader = crashReportReader;
    }

    public async Task<string?> InstallAndRunAsync(DownloadFileChangedHandler? onCurrentlyDownloadedFileChange = null,
        ProgressChangedEventHandler? onDownloadPercentageChange = null,
        Action? onDownloadComplete = null)
    {
        var launcher = _launcherFactory.CreateLauncher();
        launcher.FileChanged += onCurrentlyDownloadedFileChange;
        launcher.ProgressChanged += onDownloadPercentageChange;

        var version = await _versionMetadataProvider.GetVersionMetadataAsync();
        await version.SaveAsync(new MinecraftPath(_minecraftFilesDirProvider.DirPathForMinecraftFiles));
        var process = await launcher.CreateProcessAsync(version, _launchOptionsProvider.GetLaunchOptions());
        await _modsInstaller.InstallModsAsync();
        onDownloadComplete?.Invoke();

        process.Start();
        await process.WaitForExitAsync();

        return process.ExitCode == 0
            ? null
            : _crashReportReader.ReadLastCrashReport();
    }
}