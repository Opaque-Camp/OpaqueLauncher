using System.ComponentModel;
using CmlLib.Core;
using CmlLib.Core.Downloader;
using CmlLib.Core.VersionLoader;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class CmlLibMinecraftLauncher : IMinecraftLauncher
{
    private readonly CMLauncher _launcher;

    public CmlLibMinecraftLauncher(CMLauncher launcher, IVersionLoader versionLoader)
    {
        _launcher = launcher;
        _launcher.VersionLoader = versionLoader;
    }

    public event DownloadFileChangedHandler? FileChanged
    {
        add => _launcher.FileChanged += value;
        remove => _launcher.FileChanged -= value;
    }

    public event ProgressChangedEventHandler? ProgressChanged
    {
        add => _launcher.ProgressChanged += value;
        remove => _launcher.ProgressChanged -= value;
    }

    public async Task<IMinecraftProcess> CreateProcessAsync(IMinecraftVersionMetadata versionMetadata, MLaunchOption options)
    {
        var process = await _launcher.CreateProcessAsync(versionMetadata.Name, options);
        return new MinecraftProcess(process);
    }
}