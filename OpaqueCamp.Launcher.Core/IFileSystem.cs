namespace OpaqueCamp.Launcher.Core;

public interface IFileSystem
{
    bool FileExists(string path);
}