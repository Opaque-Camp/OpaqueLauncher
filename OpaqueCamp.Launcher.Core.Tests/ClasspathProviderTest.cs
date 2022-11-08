using Moq;

namespace OpaqueCamp.Launcher.Core.Tests;

public class ClasspathProviderTest
{
    [Fact]
    public void GetClasspath_GivenJsonWithFewLibraries_ReturnClasspathString()
    {
        // Given
        var jsonProvider = new Mock<IClasspathJsonProvider>();
        jsonProvider.Setup(p => p.GetClasspathJson()).Returns("""
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
        var pathProvider = new Mock<IPathProvider>();
        pathProvider.Setup(p => p.LibraryDirectoryPath).Returns(@"C:\OpaqueVanilla\game\libraries");
        var provider = new ClasspathProvider(jsonProvider.Object, pathProvider.Object);

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
        var jsonProvider = new Mock<IClasspathJsonProvider>();
        jsonProvider.Setup(p => p.GetClasspathJson()).Throws<IOException>();
        var pathProvider = new Mock<IPathProvider>();
        var provider = new ClasspathProvider(jsonProvider.Object, pathProvider.Object);

        // When
        var action = () => provider.GetClasspath();
        
        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }

    [Fact]
    public void GetClasspath_OnInvalidClasspathJson_ThrowClasspathGenerationException()
    {
        // Given
        var jsonProvider = new Mock<IClasspathJsonProvider>();
        jsonProvider.Setup(p => p.GetClasspathJson()).Returns(@"wtf");
        var pathProvider = new Mock<IPathProvider>();
        var provider = new ClasspathProvider(jsonProvider.Object, pathProvider.Object);

        // When
        var action = () => provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }

    [Fact]
    public void GetClasspath_OnMalformedClasspathJson_ThrowClasspathGenerationException()
    {
        // Given
        var jsonProvider = new Mock<IClasspathJsonProvider>();
        jsonProvider.Setup(p => p.GetClasspathJson()).Returns("""{"libraries": null}""");
        var pathProvider = new Mock<IPathProvider>();
        var provider = new ClasspathProvider(jsonProvider.Object, pathProvider.Object);

        // When
        var action = () => provider.GetClasspath();

        // Then
        action.Should().Throw<ClasspathGenerationException>();
    }
}