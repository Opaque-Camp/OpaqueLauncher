namespace OpaqueCamp.Launcher.Core;

public sealed class AccountRepositoryException : Exception
{
    public AccountRepositoryException(string? message, Exception? innerException = null) : base(message, innerException)
    {
    }
}