namespace OpaqueCamp.Launcher.Core.Tests;

public class ClasspathProviderTest
{
    private readonly Mock<IPathProvider> _pathProvider = new();
    private readonly Mock<IFileSystem> _fs = new();
    private const string ConfigPath = @"C:\OpaqueVanilla\config.json";

    public ClasspathProviderTest()
    {
        _pathProvider.Setup(p => p.LibraryDirectoryPath).Returns(@"C:\OpaqueVanilla\game\libraries");
        _pathProvider.Setup(p => p.ClasspathJsonPath).Returns(ConfigPath);
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
        var provider = new ClasspathProvider(_pathProvider.Object, _fs.Object);

        // When
        var classpath = provider.GetClasspath();

        // Then
        classpath.Should().Be(
            @"C:\OpaqueVanilla\game\libraries\net\fabricmc\tiny-mappings-parser\0.3.0+build.17\tiny-mappings-parser-0.3.0+build.17.jar;" +
            @"C:\OpaqueVanilla\game\libraries\org\ow2\asm\asm\9.3\asm-9.3.jar"
        );
    }

    [Fact]
    public void GetClasspath_OnClasspathJsonProviderIOException_ThrowClasspathGenerationException()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(ConfigPath)).Throws<IOException>();
        var provider = new ClasspathProvider(_pathProvider.Object, _fs.Object);

        // When
        var action = () => provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }

    [Fact]
    public void GetClasspath_OnInvalidClasspathJson_ThrowClasspathGenerationException()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(ConfigPath)).Returns("wtf");
        var provider = new ClasspathProvider(_pathProvider.Object, _fs.Object);

        // When
        var action = () => provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }

    [Fact]
    public void GetClasspath_OnMalformedClasspathJson_ThrowClasspathGenerationException()
    {
        // Given
        _fs.Setup(f => f.ReadAllText(ConfigPath)).Returns("""{"libraries": null}""");
        var provider = new ClasspathProvider(_pathProvider.Object, _fs.Object);

        // When
        var action = () => provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }
}