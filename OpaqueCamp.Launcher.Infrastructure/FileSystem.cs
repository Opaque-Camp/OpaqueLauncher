using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class FileSystem : IFileSystem
{
    public string ReadAllText(string path) => File.ReadAllText(path);

    public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);

    public bool FileExists(string path) => File.Exists(path);

    public bool IsDirectoryEmptyOrMissing(string path) =>
        !Directory.Exists(path) || !Directory.EnumerateFileSystemEntries(path).Any();

    public string[] GetFilesInDirectory(string path) => Directory.GetFiles(path);
    public DateTime GetFileCreationTime(string path) => File.GetCreationTime(path);
}