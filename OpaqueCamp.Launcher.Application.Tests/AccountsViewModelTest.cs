using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application.Tests;

public sealed class AccountsViewModelTest
{
    [Fact]
    public void AccountsProperty_TakesAccountsFromRepository()
    {
        // Given
        var expectedAccounts = new Account[] { new("User1"), new("User2") };
        var accountRepository = Mock.Of<IAccountRepository>(r => r.GetAccounts() == expectedAccounts);
        var viewModel = new AccountsViewModel(accountRepository);

        // When
        var actualAccounts = viewModel.Accounts.ToList();

        // Then
        actualAccounts.Should().Equal(actualAccounts);
    }
}