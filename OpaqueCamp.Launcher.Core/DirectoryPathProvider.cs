namespace OpaqueCamp.Launcher.Core;

public sealed class DirectoryPathProvider
{
    private readonly IApplicationPathProvider _appPathProvider;

    public DirectoryPathProvider(IApplicationPathProvider appPathProvider)
    {
        _appPathProvider = appPathProvider;
    }

    public string GameDirectoryPath => Path.Join(_appPathProvider.ApplicationPath, "game");
    public string NativeLibraryDirectoryPath => Path.Join(GameDirectoryPath, "libraries");
    public string AssetsDirectoryPath => Path.Join(GameDirectoryPath, "assets");
}
