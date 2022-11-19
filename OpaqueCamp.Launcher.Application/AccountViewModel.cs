namespace OpaqueCamp.Launcher.Application;

using CommunityToolkit.Mvvm.ComponentModel;
using Core;

public sealed partial class AccountViewModel : ObservableObject
{
    private readonly Account _account;

    public AccountViewModel(Account account)
    {
        _account = account;
    }

    public string Username
    {
        get => _account.Username;
        set
        {
            _account.Username = value;
            OnPropertyChanged(nameof(Username));
        }
    }
}
