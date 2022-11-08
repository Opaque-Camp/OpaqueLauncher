using System.Text.Json;

namespace OpaqueCamp.Launcher.Core;

public sealed class ClasspathProvider
{
    private readonly PathProvider _pathProvider;

    public ClasspathProvider(PathProvider pathProvider)
    {
        _pathProvider = pathProvider;
    }

    /// <summary>
    /// Returns a complete semicolon-separated classpath for launching Minecraft.
    /// </summary>
    /// <exception cref="ClasspathGenerationException">
    /// When <c>game\versions\Opaque Vanilla 1.XX.X\Opaque Vanilla 1.XX.X.json</c> does not exist or contains corrupted JSON.
    /// </exception>
    public string GetClasspath()
    {
        ClasspathJson libraries = ParseClasspathJson();

        return string.Join(';', libraries.Libraries.Select(l => l.Name).Select(LibraryNameToRelativePath));
    }

    private ClasspathJson ParseClasspathJson()
    {
        string json;
        string classpathJsonPath = _pathProvider.ClasspathJsonPath;
        try
        {
            json = File.ReadAllText(classpathJsonPath);
        }
        catch (IOException e)
        {
            throw new ClasspathGenerationException($"Failed to open {classpathJsonPath} - {e.Message}", e);
        }

        ClasspathJson libraries;
        try
        {
            libraries = JsonSerializer.Deserialize<ClasspathJson>(json)!;
            if (libraries is null)
            {
                throw new ClasspathGenerationException($"Failed to parse {classpathJsonPath} - libraries is null");
            }
        }
        catch (JsonException e)
        {
            throw new ClasspathGenerationException($"Failed to parse {classpathJsonPath} - {e.Message}", e);
        }

        return libraries;
    }

    private record ClasspathJson(LibraryDescription[] Libraries);

    private record LibraryDescription(string Name);

    private string LibraryNameToRelativePath(string libName)
    {
        var pathComponents = libName.Split(new char[] { '.', ':' });
        var fileName = string.Join('-', pathComponents.TakeLast(2)) + ".jar";
        return Path.Join(Path.Join(pathComponents), fileName);
    }
}
