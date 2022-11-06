using System;
using System.IO;

namespace OpaqueLauncher;

public sealed class JavaFinder
{
    /// <summary>
    /// Returns the absolute path to <c>javaw.exe</c>.
    /// </summary>
    /// <exception cref="JavaNotFoundException">
    /// Thrown when <c>javaw.exe</c> was not found due to JAVA_HOME not being set or being set incorrectly.
    /// </exception>
    public string? FindAbsoluteJavaExePath()
    {
        var javaHome = GetJavaHome();
        var javawExe = Path.Join(javaHome, "bin", "javaw.exe");
        if (!File.Exists(javawExe))
        {
            throw new JavaNotFoundException($"javaw.exe not found - {javawExe} does not exist");
        }
        return javawExe;
    }

    private string? GetJavaHome()
    {
        string? javaHome = string.Empty;
        try
        {
            javaHome = Environment.GetEnvironmentVariable("JAVA_HOME");
        }
        catch (ArgumentNullException e)
        {
        }
        if (javaHome == string.Empty)
        {
            throw new JavaNotFoundException("JAVA_HOME not set");
        }
        return javaHome;
    }
}
