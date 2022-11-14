using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class FileSystem : IFileSystem
{
    public void WriteAllText(string path, string contents)
    {
        File.WriteAllText(path, contents);
    }
}
