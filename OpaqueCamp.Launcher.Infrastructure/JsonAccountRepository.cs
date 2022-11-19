namespace OpaqueCamp.Launcher.Infrastructure;

using Core;
using System.Text.Json;

public sealed class JsonAccountRepository : IAccountRepository
{
    private readonly IFileSystem _fileSystem;
    private readonly IAccountJsonPathProvider _accountJsonPathProvider;

    public JsonAccountRepository(IFileSystem fileSystem, IAccountJsonPathProvider accountJsonPathProvider)
    {
        _fileSystem = fileSystem;
        _accountJsonPathProvider = accountJsonPathProvider;
    }

    public IEnumerable<Account> GetAccounts()
    {
        var path = _accountJsonPathProvider.AccountJsonPath;
        if (!_fileSystem.FileExists(path))
        {
            return Enumerable.Empty<Account>();
        }

        var json = _fileSystem.ReadAllText(path);
        return JsonSerializer.Deserialize<IEnumerable<Account>>(json);
    }

    public Account GetAccountById(Guid id)
    {
        return GetAccounts().FirstOrDefault(account => account.Id == id) ?? throw new AccountNotFoundException(id);
    }

    public void AddAccount(Account account)
    {
        var accounts = GetAccounts().ToList();
        accounts.Add(account);
        SaveAccounts(accounts);
    }

    private void SaveAccounts(IEnumerable<Account> accounts)
    {
        var json = JsonSerializer.Serialize(accounts);
        _fileSystem.WriteAllText(_accountJsonPathProvider.AccountJsonPath, json);
    }

    // TODO: Merge with AddAccount?
    public void UpdateAccount(Account account)
    {
        if (!GetAccounts().Any(a => a.Equals(account)))
        {
            throw new AccountNotFoundException(account);
        }
        var accounts = GetAccounts().Where(a => !a.Equals(account)).Append(account);
        SaveAccounts(accounts);
    }

    public void DeleteAccount(Account account)
    {
        SaveAccounts(GetAccounts().Where(a => !a.Equals(account)));
    }
}