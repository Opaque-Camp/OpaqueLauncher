namespace OpaqueLauncher;

using System;

public sealed class JavaNotFoundException : Exception
{
    public JavaNotFoundException(string message, Exception? inner = null) : base(message, inner)
    {
    }
}
