using System.ComponentModel;
using CmlLib.Core.Downloader;

namespace OpaqueCamp.Launcher.Core;

public interface IMinecraftInstallerAndRunner
{
    Task<string?> InstallAndRunAsync(DownloadFileChangedHandler? onCurrentlyDownloadedFileChange = null,
        ProgressChangedEventHandler? onDownloadPercentageChange = null,
        Action? onDownloadComplete = null);
}