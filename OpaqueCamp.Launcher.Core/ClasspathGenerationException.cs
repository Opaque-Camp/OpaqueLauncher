namespace OpaqueCamp.Launcher.Core;

public class ClasspathGenerationException : UserFriendlyException
{
    public ClasspathGenerationException(string? friendlyMessage = null, string? verboseMessage = null,
        Exception? innerException = null) : base(friendlyMessage, verboseMessage, innerException)
    {
    }
}