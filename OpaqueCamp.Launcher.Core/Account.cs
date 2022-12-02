namespace OpaqueCamp.Launcher.Core;

public sealed class Account
{
    /// <summary>
    /// Do not use, used internally by deserializer.
    /// </summary>
    public Account() : this(string.Empty, AccountType.Offline)
    {
    }

    public Account(string username, AccountType type) : this(Guid.NewGuid(), username, type)
    {
    }

    public Account(Guid id, string username, AccountType type)
    {
        Id = id;
        Username = username;
        Type = type;
    }

    public Guid Id { get; init; }

    public string Username { get; set; }

    public AccountType Type { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Account account && Id.Equals(account.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}