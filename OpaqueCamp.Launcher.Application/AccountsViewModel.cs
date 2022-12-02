using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

public sealed partial class AccountsViewModel : ObservableObject
{
    private readonly IAccountRepository _accountRepository;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAccountSelected))]
    [NotifyPropertyChangedFor(nameof(SelectAccountHintLabelVisibility))]
    [NotifyPropertyChangedFor(nameof(AccountEditorVisibility))]
    [NotifyPropertyChangedFor(nameof(SelectedAccountViewModel))]
    [NotifyCanExecuteChangedFor(nameof(DeleteSelectedAccountCommand))]
    private Account? _selectedAccount;

    public AccountsViewModel(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public IEnumerable<Account> Accounts => _accountRepository.GetAccounts();

    public bool IsAccountSelected => SelectedAccount is not null;

    public Visibility SelectAccountHintLabelVisibility => IsAccountSelected ? Visibility.Collapsed : Visibility.Visible;

    public Visibility AccountEditorVisibility => IsAccountSelected ? Visibility.Visible : Visibility.Collapsed;

    public AccountViewModel? SelectedAccountViewModel
    {
        get
        {
            if (SelectedAccount is null) return null;
            var viewModel = new AccountViewModel(SelectedAccount);
            viewModel.PropertyChanged += OnSelectedAccountViewModelPropertyChanged;
            return viewModel;
        }
    }

    public void AddSimpleAccount()
    {
        _accountRepository.AddAccount(new Account("New account", AccountType.Offline));
        OnPropertyChanged(nameof(Accounts));
    }

    private void OnSelectedAccountViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _accountRepository.UpdateAccount(SelectedAccount!);
        OnPropertyChanged(nameof(Accounts));
    }

    [RelayCommand(CanExecute = nameof(IsAccountSelected))]
    private void DeleteSelectedAccount()
    {
        _accountRepository.DeleteAccount(SelectedAccount!);
        OnPropertyChanged(nameof(Accounts));
    }
}