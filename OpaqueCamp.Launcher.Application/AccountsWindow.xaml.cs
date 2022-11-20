using System.Windows;
using System.Windows.Controls;

namespace OpaqueCamp.Launcher.Application;

public partial class AccountsWindow
{
    public AccountsWindow(AccountsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private AccountsViewModel ViewModel => (AccountsViewModel)DataContext;

    private void OnAddAccountButtonClick(object sender, RoutedEventArgs e)
    {
        ((Button)sender).ContextMenu.IsOpen = true;
    }

    private void OnAddSimpleAccountMenuItemClick(object sender, RoutedEventArgs e)
    {
        ViewModel.AddAccount();
    }

    private void OnRemoveAccountButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel.DeleteSelectedAccount();
    }
}