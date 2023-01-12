using System.Text.Json;
using System.Text.Json.Serialization;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class JsonAccountRepository : IAccountRepository
{
    private readonly IAccountJsonPathProvider _accountJsonPathProvider;
    private readonly IFileSystem _fileSystem;

    public JsonAccountRepository(IFileSystem fileSystem, IAccountJsonPathProvider accountJsonPathProvider)
    {
        _fileSystem = fileSystem;
        _accountJsonPathProvider = accountJsonPathProvider;
    }

    /// <summary>
    ///     Reads the list of accounts from the account JSON file.
    /// </summary>
    /// <returns>Deserialized list of accounts.</returns>
    /// <exception cref="AccountRepositoryException">
    ///     thrown when reading from JSON fails due to a corrupted JSON file.
    /// </exception>
    public IEnumerable<Account> GetAccounts()
    {
        var path = _accountJsonPathProvider.AccountJsonPath;
        if (!_fileSystem.FileExists(path)) return Enumerable.Empty<Account>();

        var json = _fileSystem.ReadAllText(path);
        try
        {
            return JsonSerializer.Deserialize<IEnumerable<Account>>(json,
                       new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } }) ??
                   throw new AccountRepositoryException("Deserialized accounts list is null.");
        }
        catch (JsonException e)
        {
            throw new AccountRepositoryException("Account storage JSON is corrupted.", e);
        }
    }

    public Account GetAccountById(Guid id)
    {
        return GetAccounts().FirstOrDefault(account => account.Id == id) ?? throw new AccountNotFoundException(id);
    }

    public void AddAccount(Account account)
    {
        var accounts = GetAccounts().ToList();
        if (accounts.Contains(account))
        {
            throw new AccountAlreadyExistsException(account);
        }

        accounts.Add(account);
        SaveAccounts(accounts);
    }

    // TODO: Merge with AddAccount?
    public void UpdateAccount(Account account)
    {
        if (!GetAccounts().Any(a => a.Equals(account))) throw new AccountNotFoundException(account);
        var accounts = GetAccounts().Where(a => !a.Equals(account)).Append(account);
        SaveAccounts(accounts);
    }

    public void DeleteAccount(Account account)
    {
        if (!GetAccounts().Any(a => a.Equals(account))) throw new AccountNotFoundException(account);
        SaveAccounts(GetAccounts().Where(a => !a.Equals(account)));
    }

    private void SaveAccounts(IEnumerable<Account> accounts)
    {
        var json = JsonSerializer.Serialize(accounts,
            new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
        _fileSystem.WriteAllText(_accountJsonPathProvider.AccountJsonPath, json);
    }
}