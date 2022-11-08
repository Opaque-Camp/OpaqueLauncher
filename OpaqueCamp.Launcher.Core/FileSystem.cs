namespace OpaqueCamp.Launcher.Core;

class FileSystem : IFileSystem
{
    public bool FileExists(string path) => File.Exists(path);
}
