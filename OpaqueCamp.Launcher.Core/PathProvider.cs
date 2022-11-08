namespace OpaqueCamp.Launcher.Core;

public sealed class PathProvider
{
    private readonly IApplicationPathProvider _appPathProvider;

    public PathProvider(IApplicationPathProvider appPathProvider)
    {
        _appPathProvider = appPathProvider;
    }

    public string GameDirectoryPath => Path.Join(_appPathProvider.ApplicationPath, "game");
    public string NativeLibraryDirectoryPath => Path.Join(GameDirectoryPath, "libraries");
    public string AssetsDirectoryPath => Path.Join(GameDirectoryPath, "assets");
}
