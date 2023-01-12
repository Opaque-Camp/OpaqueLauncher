using System.Text.Json;

namespace OpaqueCamp.Launcher.Core.Tests;

public sealed class AccountTest
{
    [Fact]
    public void Account_HasNonEmptyGuidAfterCreation()
    {
        // Given
        var account = new Account("username", AccountType.Offline);

        // When
        var guid = account.Id;

        // Then
        guid.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Account_SerializesToJson()
    {
        // Given
        var account = new Account("username", AccountType.Microsoft);

        // When
        var json = JsonSerializer.Serialize(account);

        // Then
        json.Should().Be(JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "Id", account.Id.ToString() },
            { "Username", account.Username },
            { "Type", (int)AccountType.Microsoft }
        }));
    }

    [Fact]
    public void Account_DeserializesToJson()
    {
        // Given
        var id = Guid.NewGuid();
        var json = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "Id", id.ToString() },
            { "Username", "username" },
            { "Type", (int)AccountType.Microsoft }
        });

        // When
        var account = JsonSerializer.Deserialize<Account>(json)!;

        // Then
        account.Should().NotBeNull();
        account.Id.Should().Be(id);
        account.Id.Should().NotBe(Guid.Empty);
        account.Username.Should().Be("username");
        account.Type.Should().Be(AccountType.Microsoft);
    }


    [Fact]
    public void TwoAccountsWithSameId_ComparedWithEquals_ReturnTrue()
    {
        // Given
        var id = Guid.NewGuid();
        var acc1 = new Account(id, "username1", AccountType.Offline);
        var acc2 = new Account(id, "username2", AccountType.Microsoft);

        // When
        var equal = acc1.Equals(acc2);

        // Then
        equal.Should().BeTrue();
    }

    [Fact]
    public void DictionaryWithAccounts_CorrectlyHashable()
    {
        // Given
        var acc1 = new Account("P1", AccountType.Offline);
        var acc2 = new Account("P2", AccountType.Microsoft);
        var dict = new Dictionary<Account, int> { { acc1, 1 }, { acc2, 2 } };
        
        // When
        var one = dict[acc1];
        var two = dict[acc2];
        
        // Then
        one.Should().Be(1);
        two.Should().Be(2);
    }
}