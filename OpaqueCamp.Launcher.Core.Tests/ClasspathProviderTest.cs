namespace OpaqueCamp.Launcher.Core.Tests;

public class ClasspathProviderTest
{
    private readonly Mock<IPathProvider> _pathProvider = new();
    private readonly Mock<IFileSystem> _fs = new();
    private readonly string _configPath = Path.Join("C:", "OpaqueVanilla", "config.json");
    private readonly string _gameJarPath = Path.Join("C:", "OpaqueVanilla", "game", "game.jar");
    private readonly ClasspathProvider _provider;

    public ClasspathProviderTest()
    {
        _pathProvider
            .Setup(p => p.LibraryDirectoryPath)
            .Returns(Path.Join("C:", "OpaqueVanilla", "game", "libraries"));
        _pathProvider.Setup(p => p.GameJarPath).Returns(_gameJarPath);
        _pathProvider.Setup(p => p.ClasspathJsonPath).Returns(_configPath);
        _provider = new ClasspathProvider(_pathProvider.Object, _fs.Object);
    }

    [Fact]
    public void GetClasspath_GivenJsonWithFewLibraries_ReturnClasspathString()
    {
        // Given
        _fs
            .Setup(f => f.ReadAllText(_configPath))
            .Returns("""
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
            string.Join(
                Path.PathSeparator,
                Path.Join("C:", "OpaqueVanilla", "game", "libraries", "net", "fabricmc",
                    "tiny-mappings-parser",
                    "0.3.0+build.17", "tiny-mappings-parser-0.3.0+build.17.jar"),
                Path.Join("C:", "OpaqueVanilla", "game", "libraries", "org", "ow2", "asm", "asm", "9.3", "asm-9.3.jar"),
                _gameJarPath
            )
        );
    }

    [Fact]
    public void GetClasspath_OnClasspathJsonProviderIOException_ThrowClasspathGenerationException()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(_configPath)).Throws<IOException>();

        // When
        var action = () => _provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }

    [Fact]
    public void GetClasspath_OnInvalidClasspathJson_ThrowClasspathGenerationException()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(_configPath)).Returns("wtf");

        // When
        var action = () => _provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }

    [Fact]
    public void GetClasspath_OnWholeBodyBeingNull_ThrowClasspathGenerationException()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(_configPath)).Returns("null");

        // When
        var action = () => _provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }

    [Fact]
    public void GetClasspath_OnLibrariesBeingNull_ThrowClasspathGenerationException()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(_configPath)).Returns("""{"libraries": null}""");

        // When
        var action = () => _provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }
}