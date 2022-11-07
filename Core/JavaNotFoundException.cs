namespace OpaqueLauncher.Core;

using System;

public sealed class JavaNotFoundException : Exception
{
    public JavaHomeProblem ProblemKind { get; }

    /// <summary>
    /// Expected path to <c>javaw.exe</c> based on <c>JAVA_HOME</c> environment variable.
    /// If <c>JAVA_HOME</c> is not set, this property is set to <c>null</c>.
    /// </summary>
    public string? ExpectedJavawPath { get; }

    public JavaNotFoundException(JavaHomeProblem problem, string? expectedJavawPath = null) : base($"There is a problem with JAVA_HOME: {problem} (expected javaw.exe path: {expectedJavawPath})")
    {
        ProblemKind = problem;
        ExpectedJavawPath = expectedJavawPath;
    }
}
