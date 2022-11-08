﻿using System.Text.Json;

namespace OpaqueCamp.Launcher.Core;

public sealed class ClasspathProvider
{
    private readonly IClasspathJsonProvider _classpathJsonProvider;
    private readonly IPathProvider _pathProvider;

    public ClasspathProvider(IClasspathJsonProvider classpathJsonProvider, IPathProvider pathProvider)
    {
        _classpathJsonProvider = classpathJsonProvider;
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
        var libraries = ParseClasspathJson();
        var libPaths = libraries.Libraries
            .Select(l => l.Name)
            .Select(LibraryNameToRelativePath)
            .Select(p => Path.Join(_pathProvider.LibraryDirectoryPath, p));
        return string.Join(';', libPaths);
    }

    private ClasspathJson ParseClasspathJson()
    {
        string json;
        try
        {
            json = _classpathJsonProvider.GetClasspathJson();
        } catch(IOException e)
        {
            throw new ClasspathGenerationException($"Failed to open classpath JSON file - {e.Message}", e);
        }

        ClasspathJson libraries;
        try
        {
            libraries = JsonSerializer.Deserialize<ClasspathJson>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            if (libraries is null)
            {
                throw new ClasspathGenerationException($"Failed to parse classpath JSON file - whole file object is null");
            }
            if (libraries.Libraries is null)
            {
                throw new ClasspathGenerationException($"Failed to parse classpath JSON file - libraries is null");
            }
        }
        catch (JsonException e)
        {
            throw new ClasspathGenerationException($"Failed to parse classpath JSON file - {e.Message}", e);
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
