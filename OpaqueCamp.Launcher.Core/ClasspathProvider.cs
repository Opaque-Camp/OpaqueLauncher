using System.Text.Json;

namespace OpaqueCamp.Launcher.Core;

public sealed class ClasspathProvider
{
    private readonly IPathProvider _pathProvider;
    private readonly IFileSystem _fileSystem;

    public ClasspathProvider(IPathProvider pathProvider, IFileSystem fileSystem)
    {
        _pathProvider = pathProvider;
        _fileSystem = fileSystem;
    }

    /// <summary>
    /// Returns a complete semicolon-separated classpath for launching Minecraft.
    /// </summary>
    /// <exception cref="ClasspathGenerationException">
    /// When <c>game\versions\Opaque Vanilla 1.XX.X\Opaque Vanilla 1.XX.X.json</c> does not exist or contains corrupted JSON.
    /// </exception>
    public string GetClasspath()
    {
        var libraries = ParseClasspathJson();
        var libPaths = libraries.Libraries
            .Select(l => l.Name)
            .Select(LibraryNameToRelativePath)
            .Select(p => Path.Join(_pathProvider.LibraryDirectoryPath, p));
        var classpath = libPaths.Append(_pathProvider.GameJarPath);
        return string.Join(Path.PathSeparator, classpath);
    }

    private ClasspathJson ParseClasspathJson()
    {
        string json;
        try
        {
            json = _fileSystem.ReadAllText(_pathProvider.ClasspathJsonPath);
        }
        catch (IOException e)
        {
            throw new ClasspathGenerationException(
                "Не удалось открыть файл со списком компонентов сборки. Попробуйте скачать лаунчер заново.",
                $"Failed to open classpath JSON file - {e.Message}", e);
        }

        ClasspathJson libraries;
        try
        {
            libraries = JsonSerializer.Deserialize<ClasspathJson>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            if (libraries is null)
            {
                throw new ClasspathGenerationException(
                    "Файл со списком компонентов сборки поврежден. Попробуйте скачать лаунчер заново.",
                    "Failed to parse classpath JSON file - whole file object is null");
            }

            if (libraries.Libraries is null)
            {
                throw new ClasspathGenerationException(
                    "Файл со списком компонентов сборки поврежден. Попробуйте скачать лаунчер заново.",
                    "Failed to parse classpath JSON file - libraries is null");
            }
        }
        catch (JsonException e)
        {
            throw new ClasspathGenerationException(
                "Файл со списком компонентов сборки поврежден. Попробуйте скачать лаунчер заново.",
                $"Failed to parse classpath JSON file - {e.Message}", e);
        }

        return libraries;
    }

    private record ClasspathJson(LibraryDescription[] Libraries);

    private record LibraryDescription(string Name);

    private string LibraryNameToRelativePath(string libName)
    {
        var groupNameAndVersion = libName.Split(':');
        var group = groupNameAndVersion[0];
        var name = groupNameAndVersion[1];
        var version = groupNameAndVersion[2];

        var fileName = $"{name}-{version}.jar";
        var directory = Path.Join(Path.Join(group.Split('.')), name, version);
        return Path.Join(directory, fileName);
    }
}