namespace OpaqueCamp.Launcher.Core;

public sealed class AccountAlreadyExistsException : Exception
{
    public AccountAlreadyExistsException(Account account) : base($"Account {account.Username} already exists.")
    {
    }
}