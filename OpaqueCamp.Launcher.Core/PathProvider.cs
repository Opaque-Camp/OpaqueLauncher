namespace OpaqueCamp.Launcher.Core;

public sealed class PathProvider : IPathProvider
{
    private readonly IApplicationPathProvider _appPathProvider;
    private readonly ILauncherInfoProvider _launcherInfoProvider;

    public PathProvider(IApplicationPathProvider appPathProvider, ILauncherInfoProvider launcherInfoProvider)
    {
        _appPathProvider = appPathProvider;
        _launcherInfoProvider = launcherInfoProvider;
    }

    public string GameDirectoryPath => Path.Join(_appPathProvider.ApplicationPath, "game");
    public string LibraryDirectoryPath => Path.Join(GameDirectoryPath, "libraries");
    public string AssetsDirectoryPath => Path.Join(GameDirectoryPath, "assets");
    public string ClasspathJsonPath => Path.Join(GameDirectoryPath, "versions", _launcherInfoProvider.LauncherNameAndVersion, $"{_launcherInfoProvider.LauncherNameAndVersion}.json");
}
