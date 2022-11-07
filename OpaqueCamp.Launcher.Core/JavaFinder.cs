namespace OpaqueCamp.Launcher.Core;

using System.Diagnostics;

public sealed class JavaFinder
{
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
        try
        {
            return GetJavawExePathByJavaHome();
        }
        catch (JavaNotFoundException)
        {
            return GetJavawExePathByJavaVersionQuery();
        }
    }

    private string GetJavawExePathByJavaHome()
    {
        var javaHome = GetJavaHome();
        var javawExe = Path.Join(javaHome, "bin", "javaw.exe");
        if (!File.Exists(javawExe))
        {
            throw new JavaNotFoundException(JavaHomeProblem.JavawExeMissing, javawExe);
        }

        return javawExe;
    }

    private string GetJavaHome()
    {
        var javaHome = Environment.GetEnvironmentVariable("JAVA_HOME");
        if (string.IsNullOrEmpty(javaHome))
        {
            throw new JavaNotFoundException(JavaHomeProblem.NotSet);
        }

        return javaHome;
    }

    private string GetJavawExePathByJavaVersionQuery()
    {
        var psi = new ProcessStartInfo
        {
            FileName = "java.exe",
            Arguments = " -version",
            RedirectStandardError = true,
            UseShellExecute = false
        };

        var pr = Process.Start(psi);
        var strOutput = pr.StandardError.ReadLine().Split(' ')[2].Replace("\"", "");

        return strOutput;
    }
}
