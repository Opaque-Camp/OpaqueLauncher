namespace OpaqueCamp.Launcher.Core;

public interface IFileSystem
{
    string ReadAllText(string path);
    void WriteAllText(string path, string contents);
    bool FileExists(string path);
}