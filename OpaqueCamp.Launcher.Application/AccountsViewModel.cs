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
    [NotifyPropertyChangedFor(nameof(SelectAccountHintLabelVisiblilty))]
    [NotifyPropertyChangedFor(nameof(AccountEditorVisiblilty))]
    [NotifyPropertyChangedFor(nameof(SelectedAccountViewModel))]
    [NotifyCanExecuteChangedFor(nameof(DeleteSelectedAccountCommand))]
    private Account? _selectedAccount;

    public AccountsViewModel(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public IEnumerable<Account> Accounts => _accountRepository.GetAccounts();

    public bool IsAccountSelected => SelectedAccount is not null;

    public Visibility SelectAccountHintLabelVisiblilty => IsAccountSelected ? Visibility.Collapsed : Visibility.Visible;

    public Visibility AccountEditorVisiblilty => IsAccountSelected ? Visibility.Visible : Visibility.Collapsed;

    public AccountViewModel? SelectedAccountViewModel
    {
        get
        {
            if (SelectedAccount is not null)
            {
                var viewModel = new AccountViewModel(SelectedAccount);
                viewModel.PropertyChanged += OnSelectedAccountViewModelPropertyChanged;
                return viewModel;
            }

            return null;
        }
    }

    public void AddAccount()
    {
        _accountRepository.AddAccount(new Account("Test"));
        OnPropertyChanged(nameof(Accounts));
    }

    private void OnSelectedAccountViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _accountRepository.UpdateAccount(SelectedAccount!);
        OnPropertyChanged(nameof(Accounts));
    }

    [RelayCommand(CanExecute = nameof(IsAccountSelected))]
    public void DeleteSelectedAccount()
    {
        _accountRepository.DeleteAccount(SelectedAccount!);
        OnPropertyChanged(nameof(Accounts));
    }
}