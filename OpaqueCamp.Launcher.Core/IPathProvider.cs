namespace OpaqueCamp.Launcher.Core
{
    public interface IPathProvider
    {
        string AssetsDirectoryPath { get; }
        string ClasspathJsonPath { get; }
        string GameDirectoryPath { get; }
        string LibraryDirectoryPath { get; }
        string GameJarPath { get; }
    }
}