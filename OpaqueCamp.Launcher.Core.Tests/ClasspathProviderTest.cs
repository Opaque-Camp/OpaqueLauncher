namespace OpaqueCamp.Launcher.Core.Tests;

public class ClasspathProviderTest
{
    private readonly Mock<IPathProvider> _pathProvider = new();
    private readonly Mock<IFileSystem> _fs = new();
    private readonly string ConfigPath = Path.Join("C:", "OpaqueVanilla", "config.json");
    private readonly ClasspathProvider _provider;

    public ClasspathProviderTest()
    {
        _pathProvider
            .Setup(p => p.LibraryDirectoryPath)
            .Returns(Path.Join("C:", "OpaqueVanilla", "game", "libraries"));
        _pathProvider.Setup(p => p.ClasspathJsonPath).Returns(ConfigPath);
        _provider = new ClasspathProvider(_pathProvider.Object, _fs.Object);
    }

    [Fact]
    public void GetClasspath_GivenJsonWithFewLibraries_ReturnClasspathString()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(ConfigPath)).Returns("""
            {
                "libraries": [
                    {
                        "name": "net.fabricmc:tiny-mappings-parser:0.3.0+build.17",
                        "url": "http://example.com"
                    },
                    {
                        "name": "org.ow2.asm:asm:9.3",
                        "url": "http://example.com"
                    }
                ]
            }
            """);

        // When
        var classpath = _provider.GetClasspath();

        // Then
        classpath.Should().Be(
            Path.Join("C:", "OpaqueVanilla", "game", "libraries", "net", "fabricmc", "tiny-mappings-parser",
                "0.3.0+build.17", "tiny-mappings-parser-0.3.0+build.17.jar") +
            Path.PathSeparator +
            Path.Join("C:", "OpaqueVanilla", "game", "libraries", "org", "ow2", "asm", "asm", "9.3", "asm-9.3.jar")
        );
    }

    [Fact]
    public void GetClasspath_OnClasspathJsonProviderIOException_ThrowClasspathGenerationException()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(ConfigPath)).Throws<IOException>();

        // When
        var action = () => _provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }

    [Fact]
    public void GetClasspath_OnInvalidClasspathJson_ThrowClasspathGenerationException()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(ConfigPath)).Returns("wtf");

        // When
        var action = () => _provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }

    [Fact]
    public void GetClasspath_OnWholeBodyBeingNull_ThrowClasspathGenerationException()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(ConfigPath)).Returns("null");

        // When
        var action = () => _provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }

    [Fact]
    public void GetClasspath_OnLibrariesBeingNull_ThrowClasspathGenerationException()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(ConfigPath)).Returns("""{"libraries": null}""");

        // When
        var action = () => _provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }
}
