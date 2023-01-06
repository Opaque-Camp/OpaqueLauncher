using System.ComponentModel;
using CmlLib.Core;
using CmlLib.Core.Downloader;

namespace OpaqueCamp.Launcher.Core;

public interface IMinecraftLauncher
{
    event DownloadFileChangedHandler? FileChanged;
    event ProgressChangedEventHandler? ProgressChanged;

    Task<IMinecraftProcess> CreateProcessAsync(IMinecraftVersionMetadata versionMetadata, MLaunchOption options);
}