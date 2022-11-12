namespace OpaqueCamp.Launcher.Core;

/// <summary>
/// Exception subclass intended for being displayed in error message boxes.
/// <br/>
/// It contains two messages - one message, so called Friendly Message,
/// is intended for end users and should not get into technical details to avoid confusion.
/// The other, Verbose Message, is intended for developers
/// and is visible in the string representation of the exception.
/// </summary>
public class UserFriendlyException : Exception
{
    private readonly string? _friendlyMessage;
    private bool _includeVerboseMessageInMessage;

    /// <summary>
    /// Exception message intended for developers. Can contain verbose information related to technical details,
    /// such as code entities.
    /// </summary>
    public string? VerboseMessage { get; }

    /// <summary>
    /// Exception message intended for end users. Should not contain technical details to avoid end user confusion.
    /// </summary>
    public override string Message =>
        _includeVerboseMessageInMessage
            ? $"{_friendlyMessage}\n\n${VerboseMessage}"
            : _friendlyMessage ?? "";

    public UserFriendlyException(string? friendlyMessage = null, string? verboseMessage = null,
        Exception? innerException = null) : base(friendlyMessage, innerException)
    {
        _friendlyMessage = friendlyMessage;
        VerboseMessage = verboseMessage;
    }

    /// <summary>
    /// Returns a string representation of the exception. Intended to be used in logs and crash reports,
    /// therefore it includes both friendly and verbose messages.
    /// </summary>
    public override string ToString()
    {
        // Changing the value of Message property just for Exception.ToString()
        _includeVerboseMessageInMessage = true;
        var asString = base.ToString();
        _includeVerboseMessageInMessage = false;
        return asString;
    }
}