namespace OpaqueLauncher.Core;

/// <summary>
/// Represents the kind of <c>JAVA_HOME</c> related problem.
/// </summary>
public enum JavaHomeProblem
{
    /// <summary>
    /// <c>JAVA_HOME</c> is not set at all.
    /// </summary>
    NotSet,

    /// <summary>
    /// <c>%JAVA_HOME%\bin\javaw.exe</c> is missing.
    /// </summary>
    JavawExeMissing
}