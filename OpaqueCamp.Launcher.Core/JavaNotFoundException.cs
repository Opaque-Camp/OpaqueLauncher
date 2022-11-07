namespace OpaqueCamp.Launcher.Core;

using System;

public sealed class JavaNotFoundException : Exception
{
    public JavaNotFoundException() : base($"javaw.exe was not found because JAVA_HOME is not set or set incorrectly and javaw.exe is missing in PATH.")
    {
    }
}
