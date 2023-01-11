using OpaqueCamp.Launcher.Core;
using Xunit.Sdk;

namespace OpaqueCamp.Launcher.Application.Tests;

public sealed class AccountsViewModelTest
{
    private readonly IAccountRepository _accountRepository = Mock.Of<IAccountRepository>();
    private readonly AccountsViewModel _viewModel;

    public AccountsViewModelTest()
    {
        _viewModel = new AccountsViewModel(_accountRepository);
    }

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
        // When
        var selectedAccount = _viewModel.SelectedAccount;

        // Then
        selectedAccount.Should().BeNull();
    }

    [Fact]
    public void IsAccountSelectedProperty_UpdatesAfterAccountSelection()
    {
        // When
        var before = _viewModel.IsAccountSelected;
        _viewModel.SelectedAccount = new Account("User1");
        var after = _viewModel.IsAccountSelected;

        // Then
        before.Should().BeFalse();
        after.Should().BeTrue();
    }

    [Fact]
    public void SelectedAccountProperty_FiresPropertyChangedEventForDependentProperties()
    {
        // When
        var changedProperties = new List<string>();
        _viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == null)
            {
                throw new XunitException("Property name is null.");
            }

            changedProperties.Add(args.PropertyName);
        };
        _viewModel.SelectedAccount = new Account("User1");

        // Then
        changedProperties.Should().Equal(
            nameof(_viewModel.SelectedAccount),
            nameof(_viewModel.IsAccountSelected),
            nameof(_viewModel.SelectAccountHintLabelVisibility),
            nameof(_viewModel.AccountEditorVisibility),
            nameof(_viewModel.SelectedAccountViewModel)
        );
    }

    [Fact]
    public void SelectedAccountViewModelProperty_ReturnsCorrectViewModel()
    {
        // When
        var before = _viewModel.SelectedAccountViewModel;
        _viewModel.SelectedAccount = new Account("User1");
        var after = _viewModel.SelectedAccountViewModel;

        // Then
        before.Should().BeNull();
        after.Should().NotBeNull();
        after!.Username.Should().Be("User1");
    }

    [Fact]
    public void AccountsViewModel_SelectedAccountViewModelChanged_ShouldUpdateSelectedAccount()
    {
        // Given
        var account = new Account("User1");
        _viewModel.SelectedAccount = account;

        // When
        var selectedAccountViewModel = _viewModel.SelectedAccountViewModel;
        selectedAccountViewModel!.Username = "User2";

        // Then
        account.Username.Should().Be("User2");
        Mock.Get(_accountRepository).Verify(r => r.UpdateAccount(account));
    }
    
    [Fact]
    public void DeleteAccountCommand_DeletesSelectedAccount()
    {
        // Given
        var account = new Account("User1");
        _viewModel.SelectedAccount = account;
        
        // When
        _viewModel.DeleteSelectedAccountCommand.Execute(null);
        
        // Then
        Mock.Get(_accountRepository).Verify(r => r.DeleteAccount(account));
    }
    
    [Fact]
    public void DeleteAccountCommand_CanExecute_AllowsExecutionIfAccountIsSelected()
    {
        // Given
        var account = new Account("User1");
        var viewModel = new AccountsViewModel(_accountRepository);
        
        // When
        var before = viewModel.DeleteSelectedAccountCommand.CanExecute(null);
        viewModel.SelectedAccount = account;
        var after = viewModel.DeleteSelectedAccountCommand.CanExecute(null);
        
        // Then
        before.Should().BeFalse();
        after.Should().BeTrue();
    }
}