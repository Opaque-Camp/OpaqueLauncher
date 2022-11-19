namespace OpaqueCamp.Launcher.Core;

public sealed class AccountNotFoundException : Exception
{
    public AccountNotFoundException(Guid id) : base($"Account with id {id} not found.")
    {
    }

    public AccountNotFoundException(Account account) : this(account.Id)
    {
    }
}