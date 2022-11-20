using CommunityToolkit.Mvvm.ComponentModel;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

public sealed class AccountViewModel : ObservableObject
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
            OnPropertyChanged();
        }
    }
}
