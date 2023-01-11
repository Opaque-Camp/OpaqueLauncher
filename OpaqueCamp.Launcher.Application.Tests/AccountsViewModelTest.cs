using OpaqueCamp.Launcher.Core;
using Xunit.Sdk;

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

    [Fact]
    public void SelectedAccountProperty_NullByDefault()
    {
        // Given
        var accountRepository = Mock.Of<IAccountRepository>();
        var viewModel = new AccountsViewModel(accountRepository);

        // When
        var selectedAccount = viewModel.SelectedAccount;

        // Then
        selectedAccount.Should().BeNull();
    }

    [Fact]
    public void IsAccountSelectedProperty_UpdatesAfterAccountSelection()
    {
        // Given
        var accountRepository = Mock.Of<IAccountRepository>();
        var viewModel = new AccountsViewModel(accountRepository);

        // When
        var before = viewModel.IsAccountSelected;
        viewModel.SelectedAccount = new Account("User1");
        var after = viewModel.IsAccountSelected;

        // Then
        before.Should().BeFalse();
        after.Should().BeTrue();
    }

    [Fact]
    public void SelectedAccountProperty_FiresPropertyChangedEventForDependentProperties()
    {
        // Given
        var accountRepository = Mock.Of<IAccountRepository>();
        var viewModel = new AccountsViewModel(accountRepository);

        // When
        var changedProperties = new List<string>();
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == null)
            {
                throw new XunitException("Property name is null.");
            }

            changedProperties.Add(args.PropertyName);
        };
        viewModel.SelectedAccount = new Account("User1");

        // Then
        changedProperties.Should().Equal(
            nameof(viewModel.SelectedAccount),
            nameof(viewModel.IsAccountSelected),
            nameof(viewModel.SelectAccountHintLabelVisibility),
            nameof(viewModel.AccountEditorVisibility),
            nameof(viewModel.SelectedAccountViewModel)
        );
    }

    [Fact]
    public void SelectedAccountViewModelProperty_ReturnsCorrectViewModel()
    {
        // Given
        var accountRepository = Mock.Of<IAccountRepository>();
        var viewModel = new AccountsViewModel(accountRepository);

        // When
        var before = viewModel.SelectedAccountViewModel;
        viewModel.SelectedAccount = new Account("User1");
        var after = viewModel.SelectedAccountViewModel;

        // Then
        before.Should().BeNull();
        after.Should().NotBeNull();
        after!.Username.Should().Be("User1");
    }

    [Fact]
    public void AccountsViewModel_SelectedAccountViewModelChanged_ShouldUpdateSelectedAccount()
    {
        // Given
        var accountRepository = Mock.Of<IAccountRepository>();
        var account = new Account("User1");
        var viewModel = new AccountsViewModel(accountRepository)
        {
            SelectedAccount = account
        };

        // When
        var selectedAccountViewModel = viewModel.SelectedAccountViewModel;
        selectedAccountViewModel!.Username = "User2";

        // Then
        account.Username.Should().Be("User2");
        Mock.Get(accountRepository).Verify(r => r.UpdateAccount(account));
    }
    
    [Fact]
    public void DeleteAccountCommand_DeletesSelectedAccount()
    {
        // Given
        var accountRepository = Mock.Of<IAccountRepository>();
        var account = new Account("User1");
        var viewModel = new AccountsViewModel(accountRepository)
        {
            SelectedAccount = account
        };
        
        // When
        viewModel.DeleteSelectedAccountCommand.Execute(null);
        
        // Then
        Mock.Get(accountRepository).Verify(r => r.DeleteAccount(account));
    }
    
    [Fact]
    public void DeleteAccountCommand_CanExecute_AllowsExecutionIfAccountIsSelected()
    {
        // Given
        var accountRepository = Mock.Of<IAccountRepository>();
        var account = new Account("User1");
        var viewModel = new AccountsViewModel(accountRepository);
        
        // When
        var before = viewModel.DeleteSelectedAccountCommand.CanExecute(null);
        viewModel.SelectedAccount = account;
        var after = viewModel.DeleteSelectedAccountCommand.CanExecute(null);
        
        // Then
        before.Should().BeFalse();
        after.Should().BeTrue();
    }
}