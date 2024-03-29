﻿using System.Text.Json;

namespace OpaqueCamp.Launcher.Core.Tests;

public sealed class AccountTest
{
    [Fact]
    public void Account_HasNonEmptyGuidAfterCreation()
    {
        // Given
        var account = new Account("username");

        // When
        var guid = account.Id;

        // Then
        guid.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Account_SerializesToJson()
    {
        // Given
        var account = new Account("username");

        // When
        var json = JsonSerializer.Serialize(account);

        // Then
        json.Should().Be(JsonSerializer.Serialize(new Dictionary<string, string>
            { { "Id", account.Id.ToString() }, { "Username", account.Username } }));
    }

    [Fact]
    public void Account_DeserializesToJson()
    {
        // Given
        var id = Guid.NewGuid();
        var json = JsonSerializer.Serialize(new Dictionary<string, string>
            { { "Id", id.ToString() }, { "Username", "username" } });

        // When
        var account = JsonSerializer.Deserialize<Account>(json)!;

        // Then
        account.Should().NotBeNull();
        account.Id.Should().Be(id);
        account.Id.Should().NotBe(Guid.Empty);
        account.Username.Should().Be("username");
    }


    [Fact]
    public void TwoAccountsWithSameId_ComparedWithEquals_ReturnTrue()
    {
        // Given
        var id = Guid.NewGuid();
        var acc1 = new Account(id, "username1");
        var acc2 = new Account(id, "username2");

        // When
        var equal = acc1.Equals(acc2);

        // Then
        equal.Should().BeTrue();
    }

    [Fact]
    public void DictionaryWithAccounts_CorrectlyHashable()
    {
        // Given
        var acc1 = new Account("P1");
        var acc2 = new Account("P2");
        var dict = new Dictionary<Account, int> { { acc1, 1 }, { acc2, 2 } };
        
        // When
        var one = dict[acc1];
        var two = dict[acc2];
        
        // Then
        one.Should().Be(1);
        two.Should().Be(2);
    }
}