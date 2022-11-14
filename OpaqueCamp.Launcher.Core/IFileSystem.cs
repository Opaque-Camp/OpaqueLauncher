namespace OpaqueCamp.Launcher.Core;

public interface IFileSystem
{
    void WriteAllText(string path, string contents);
}