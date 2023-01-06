using System.Text.Json;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure.Tests;

public class JsonAccountRepositoryTest
{
    private readonly Mock<IFileSystem> _fs = new();
    private readonly Mock<IAccountJsonPathProvider> _pathProvider = new();
    private readonly JsonAccountRepository _repo;
    private const string AccountJsonPath = "account.json";

    public JsonAccountRepositoryTest()
    {
        _pathProvider.Setup(p => p.AccountJsonPath).Returns(AccountJsonPath);
        _repo = new JsonAccountRepository(_fs.Object, _pathProvider.Object);
    }

    [Fact]
    public void GetAccounts_JsonWithOneAccount_ReturnThatAccount()
    {
        // Given
        var accountId = "07bfccfb-08b8-4b79-bbf4-11a8de062691";
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(true);
        _fs.Setup(f => f.ReadAllText(AccountJsonPath)).Returns(string.Format("""
        [
            {{
                "Id": "{0}",
                "Username": "User"
            }}
        ]
        """, accountId));

        // When
        var accounts = _repo.GetAccounts().ToList();

        // Then
        accounts.Should().ContainSingle();
        var account = accounts.First();
        account.Id.Should().Be(new Guid(accountId));
        account.Username.Should().Be("User");
    }

    [Fact]
    public void GetAccounts_MissingFile_ReturnEmptyEnumerable()
    {
        // Given
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(false);

        // When
        var accounts = _repo.GetAccounts();

        // Then
        accounts.Should().BeEmpty();
    }

    [Fact]
    public void GetAccounts_CorruptedJson_ThrowAccountRepositoryException()
    {
        // Given
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(true);
        _fs.Setup(f => f.ReadAllText(AccountJsonPath)).Returns("[");

        // When
        var action = () => _repo.GetAccounts();

        // Then
        action.Should().Throw<AccountRepositoryException>();
    }

    [Fact]
    public void GetAccounts_JsonWithNull_ThrowAccountRepositoryException()
    {
        // Given
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(true);
        _fs.Setup(f => f.ReadAllText(AccountJsonPath)).Returns("null");

        // When
        var action = () => _repo.GetAccounts();

        // Then
        action.Should().Throw<AccountRepositoryException>();
    }

    [Fact]
    public void GetAccountById_JsonWithTwoAccounts_ReturnCorrectAccount()
    {
        // Given
        var accountId = "07bfccfb-08b8-4b79-bbf4-11a8de062691";
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(true);
        _fs.Setup(f => f.ReadAllText(AccountJsonPath)).Returns(string.Format("""
        [
            {{
                "Id": "{0}",
                "Username": "User"
            }},
            {{
                "Id": "07bfccfb-08b8-4b79-bbf4-11a8de062692",
                "Username": "User2"
            }}
        ]
        """, accountId));

        // When
        var account = _repo.GetAccountById(new Guid(accountId));

        // Then
        account.Id.Should().Be(new Guid(accountId));
        account.Username.Should().Be("User");
    }

    [Fact]
    public void GetAccountById_NoSuchAccount_ThrowAccountNotFoundException()
    {
        // Given
        var accountId = "07bfccfb-08b8-4b79-bbf4-11a8de062691";
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(false);

        // When
        var action = () => _repo.GetAccountById(new Guid(accountId));

        // Then
        action.Should().Throw<AccountNotFoundException>();
    }

    [Fact]
    public void GetAccountById_CorruptedJson_ThrowAccountRepositoryException()
    {
        // Given
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(true);
        _fs.Setup(f => f.ReadAllText(AccountJsonPath)).Returns("[");

        // When
        var action = () => _repo.GetAccountById(Guid.NewGuid());

        // Then
        action.Should().Throw<AccountRepositoryException>();
    }

    [Fact]
    public void AddAccount_FirstAccount_AddedToNewFile()
    {
        // Given
        var account = new Account("Player");
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(false);
        var expectedJson = JsonSerializer.Serialize(new List<Dictionary<string, string>>
            { new() { { "Id", account.Id.ToString() }, { "Username", account.Username } } });
        _fs.Setup(f => f.WriteAllText(AccountJsonPath, expectedJson)).Verifiable();

        // When
        _repo.AddAccount(account);

        // Then
        _fs.Verify();
    }

    [Fact]
    public void AddAccount_SecondAccount_AddedToExistingFile()
    {
        // Given
        var oldAccount = new Account("Oldie");
        var newAccount = new Account("Player");
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(true);
        _fs.Setup(f => f.ReadAllText(AccountJsonPath)).Returns(JsonSerializer.Serialize(
            new List<Dictionary<string, string>>
                { new() { { "Id", oldAccount.Id.ToString() }, { "Username", oldAccount.Username } } }));
        var expectedJson = JsonSerializer.Serialize(new List<Dictionary<string, string>>
        {
            new() { { "Id", oldAccount.Id.ToString() }, { "Username", oldAccount.Username } },
            new() { { "Id", newAccount.Id.ToString() }, { "Username", newAccount.Username } }
        });
        _fs.Setup(f => f.WriteAllText(AccountJsonPath, expectedJson)).Verifiable();

        // When
        _repo.AddAccount(newAccount);

        // Then
        _fs.Verify();
    }

    [Fact]
    public void AddAccount_CorruptedJson_ThrowAccountRepositoryException()
    {
        // Given
        var account = new Account("Player");
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(true);
        _fs.Setup(f => f.ReadAllText(AccountJsonPath)).Returns("[");

        // When
        var action = () => _repo.AddAccount(account);

        // Then
        action.Should().Throw<AccountRepositoryException>();
    }
    
    [Fact]
    public void AddAccount_AccountWhichAlreadyExists_ThrowAccountAlreadyExistsException()
    {
        // Given
        var account = new Account("Player");
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(true);
        _fs.Setup(f => f.ReadAllText(AccountJsonPath)).Returns(JsonSerializer.Serialize(
            new List<Dictionary<string, string>>
                { new() { { "Id", account.Id.ToString() }, { "Username", account.Username } } }));

        // When
        var action = () => _repo.AddAccount(account);

        // Then
        action.Should().Throw<AccountAlreadyExistsException>();
    }
    
    [Fact]
    public void UpdateAccount_ExistingAccount_Updated()
    {
        // Given
        var account = new Account("Player");
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(true);
        _fs.Setup(f => f.ReadAllText(AccountJsonPath)).Returns(JsonSerializer.Serialize(
            new List<Dictionary<string, string>>
                { new() { { "Id", account.Id.ToString() }, { "Username", account.Username } } }));
        var expectedJson = JsonSerializer.Serialize(new List<Dictionary<string, string>>
            { new() { { "Id", account.Id.ToString() }, { "Username", "NewPlayer" } } });
        _fs.Setup(f => f.WriteAllText(AccountJsonPath, expectedJson)).Verifiable();

        // When
        account.Username = "NewPlayer";
        _repo.UpdateAccount(account);

        // Then
        _fs.Verify();
    }
    
    [Fact]
    public void UpdateAccount_NoSuchAccount_ThrowAccountNotFoundException()
    {
        // Given
        var account = new Account("Player");
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(false);

        // When
        var action = () => _repo.UpdateAccount(account);

        // Then
        action.Should().Throw<AccountNotFoundException>();
    }
    
    [Fact]
    public void DeleteAccount_ExistingAccount_Deleted()
    {
        // Given
        var account = new Account("Player");
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(true);
        _fs.Setup(f => f.ReadAllText(AccountJsonPath)).Returns(JsonSerializer.Serialize(
            new List<Dictionary<string, string>>
                { new() { { "Id", account.Id.ToString() }, { "Username", account.Username } } }));
        _fs.Setup(f => f.WriteAllText(AccountJsonPath, "[]")).Verifiable();

        // When
        _repo.DeleteAccount(account);

        // Then
        _fs.Verify();
    }
    
    [Fact]
    public void DeleteAccount_NonExistingAccount_ThrowAccountNotFoundException()
    {
        // Given
        var account = new Account("Player");
        _fs.Setup(f => f.FileExists(AccountJsonPath)).Returns(false);

        // When
        var action = () => _repo.DeleteAccount(account);

        // Then
        action.Should().Throw<AccountNotFoundException>();
    }
}