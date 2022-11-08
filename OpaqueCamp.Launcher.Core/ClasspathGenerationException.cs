namespace OpaqueCamp.Launcher.Core;

public class ClasspathGenerationException : Exception
{
    public ClasspathGenerationException(string? message) : base(message)
    {
    }

    public ClasspathGenerationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
