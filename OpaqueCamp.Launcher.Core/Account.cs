namespace OpaqueCamp.Launcher.Core;

public sealed class Account
{
    public Account() : this(string.Empty)
    {
    }

    public Account(string username) : this(Guid.NewGuid(), username)
    {
    }

    public Account(Guid id, string username)
    {
        Id = id;
        Username = username;
    }

    public Guid Id { get; init; }

    public string Username { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Account account && Id.Equals(account.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}