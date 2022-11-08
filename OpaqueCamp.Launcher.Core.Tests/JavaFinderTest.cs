namespace OpaqueCamp.Launcher.Core.Tests;

public sealed class JavaFinderTest
{
    private readonly Mock<IEnvironmentService> _environmentService;
    private readonly Mock<IFileSystem> _fs;

    public JavaFinderTest()
    {
        _environmentService = new Mock<IEnvironmentService>();
        _fs = new Mock<IFileSystem>();
    }
    
    [Fact]
    public void GetJavawExePath_WithJavaHomeEnvVarSet_ReturnJavawExeFromIt()
    {
        // Given
        _environmentService
            .Setup(s => s.GetEnvironmentVariable("JAVA_HOME"))
            .Returns(@"C:\Java");
        _fs.Setup(f => f.FileExists(@"C:\Java\bin\javaw.exe")).Returns(true);
        var javaFinder = new JavaFinder(_environmentService.Object, _fs.Object);

        // When
        var path = javaFinder.GetJavawExePath();
        
        // Then
        path.Should().Be(@"C:\Java\bin\javaw.exe");
    }

    [Fact]
    public void GetJavawExePath_WithJavaOnlyInPath_ReturnJavawExeFromPath()
    {
        // Given
        _environmentService
            .Setup(s => s.GetEnvironmentVariable("PATH"))
            .Returns(string.Join(Path.PathSeparator, @"D:\Something", @"C:\Java\bin"));
        _fs.Setup(f => f.FileExists(@"C:\Java\bin\javaw.exe")).Returns(true);
        var javaFinder = new JavaFinder(_environmentService.Object, _fs.Object);

        // When
        var path = javaFinder.GetJavawExePath();
        
        // Then
        path.Should().Be(@"C:\Java\bin\javaw.exe");
    }
    
    [Fact]
    public void GetJavawExePath_WithNoUsefulEnvVars_ThrowJavaNotFoundException()
    {
        // Given
        var javaFinder = new JavaFinder(_environmentService.Object, _fs.Object);

        // When
        var action = () => javaFinder.GetJavawExePath();
        
        // Then
        action.Should().Throw<JavaNotFoundException>();
    }
}