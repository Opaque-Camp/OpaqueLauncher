using System;
using System.Diagnostics;
using System.IO;

namespace OpaqueLauncher.Core;

public sealed class JavaFinder
{
    /// <summary>
    /// Returns the absolute path to <c>javaw.exe</c>.
    /// </summary>
    /// <exception cref="JavaNotFoundException">
    /// Thrown when <c>javaw.exe</c> was not found due to JAVA_HOME not being set or being set incorrectly.
    /// </exception>
    public string FindAbsoluteJavaExePath()
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

    // Alternative method if JAVA_HOME doesn't exist.
    // TODO: made this method more pretty
    public string GetJavaVersion()
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "java.exe";
            psi.Arguments = " -version";
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;

            Process pr = Process.Start(psi);
            string strOutput = pr.StandardError.ReadLine().Split(' ')[2].Replace("\"", "");

            return strOutput;
        }
        catch (Exception ex)
        {
            return "Exception is " + ex.Message;
        }
    }
}
