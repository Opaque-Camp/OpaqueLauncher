namespace OpaqueCamp.Launcher.Core;

public sealed class JavaFinder
{
    private readonly IEnvironmentService _environmentService;
    private readonly IFileSystem _fileSystem;

    public JavaFinder(IEnvironmentService environmentService, IFileSystem fileSystem)
    {
        _environmentService = environmentService;
        _fileSystem = fileSystem;
    }
    
    /// <summary>
    /// Returns the absolute path to <c>javaw.exe</c>.
    /// First, the <c>JAVA_HOME</c> environment variable is checked. If it is not set or set incorrectly,
    /// it is assumed that <c>javaw.exe</c> is accessible through the regular <c>Path</c> environment variable
    /// and <c>javaw.exe -version</c> command is issued. However that might also fail if this assumption is wrong.
    /// In that case, <see cref="JavaNotFoundException"/> is thrown.
    /// </summary>
    /// <exception cref="JavaNotFoundException">
    /// Thrown if all ways to find <c>javaw.exe</c> failed.
    /// </exception>
    public string GetJavawExePath()
    {
        var result = GetJavawExePathByJavaHome() ?? FindJavaInPath();
        if (result == null)
        {
            throw new JavaNotFoundException();
        }
        return result;
    }

    private string? GetJavawExePathByJavaHome()
    {
        var javaHome = GetJavaHome();
        var javawExe = Path.Join(javaHome, "bin", "javaw.exe");
        return _fileSystem.FileExists(javawExe) ? javawExe : null;
    }

    private string? GetJavaHome() => _environmentService.GetEnvironmentVariable("JAVA_HOME");

    private string? FindJavaInPath()
    {
        var path = _environmentService.GetEnvironmentVariable("PATH");
        var pathEntries = path?.Split(Path.PathSeparator);
        return pathEntries?.Select(entry => Path.Join(entry, "javaw.exe"))
            .FirstOrDefault(_fileSystem.FileExists);
    }
}
