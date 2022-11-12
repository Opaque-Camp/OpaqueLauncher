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

    private string VersionDirectoryPath =>
        Path.Join(GameDirectoryPath, "versions", _launcherInfoProvider.LauncherNameAndVersion);

    public string ClasspathJsonPath =>
        Path.Join(VersionDirectoryPath, $"{_launcherInfoProvider.LauncherNameAndVersion}.json");

    public string GameJarPath => Path.Join(VersionDirectoryPath, $"{_launcherInfoProvider.LauncherNameAndVersion}.jar");
}