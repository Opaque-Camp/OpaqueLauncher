using CmlLib.Core;
using CmlLib.Core.VersionLoader;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class CmlLibMinecraftLauncherFactory : IMinecraftLauncherFactory
{
    private readonly IMinecraftFilesDirProvider _minecraftFilesDirProvider;
    private readonly IVersionLoader _versionLoader;

    public CmlLibMinecraftLauncherFactory(IMinecraftFilesDirProvider minecraftFilesDirProvider,
        IVersionLoader versionLoader)
    {
        _minecraftFilesDirProvider = minecraftFilesDirProvider;
        _versionLoader = versionLoader;
    }

    public IMinecraftLauncher CreateLauncher() =>
        new CmlLibMinecraftLauncher(new CMLauncher(_minecraftFilesDirProvider.DirPathForMinecraftFiles),
            _versionLoader);
}