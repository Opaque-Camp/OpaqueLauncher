using System.Linq;
using System.Windows;
using CmlLib.Core.Downloader;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly CmlLibMinecraftRunner _minecraftRunner;
    private readonly MinecraftCrashHandler _crashHandler;
    private readonly AccountsWindowFactory _accountsWindowFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentAccountProvider _currentAccountProvider;

    public MainWindow(CmlLibMinecraftRunner minecraftRunner, MinecraftCrashHandler crashHandler,
        AccountsWindowFactory accountsWindowFactory, ICurrentAccountProvider currentAccountProvider,
        IAccountRepository accountRepository)
    {
        _minecraftRunner = minecraftRunner;
        _crashHandler = crashHandler;
        _accountsWindowFactory = accountsWindowFactory;
        _currentAccountProvider = currentAccountProvider;
        _accountRepository = accountRepository;

        InitializeComponent();
        PopulateCurrentAccountComboBox();

#if DEBUG
        Window.Title += " [DEBUG]";
#endif
    }

    private void PopulateCurrentAccountComboBox()
    {
        CurrentAccountComboBox.Items.Clear();
        var accounts = _accountRepository.GetAccounts().ToList();
        foreach (var account in accounts)
        {
            CurrentAccountComboBox.Items.Add(account);
        }
        if (_currentAccountProvider.CurrentAccount == null && accounts.Count != 0)
        {
            _currentAccountProvider.CurrentAccount = accounts[0];
        }
        CurrentAccountComboBox.SelectedItem = _currentAccountProvider.CurrentAccount;
    }

    private async void OnLaunchButtonClick(object sender, RoutedEventArgs e)
    {
        DownloadFileChangedHandler downloadProgressHandler = e => CurrentlyDownloadedFileLabel.Content =
                            $"[{e.FileKind}] {e.FileName} - {e.ProgressedFileCount}/{e.TotalFileCount}";
        MinecraftCrashLogs? crashLogs;
        try
        {
            crashLogs = await _minecraftRunner.RunMinecraftAsync(downloadProgressHandler, OnDownloadPercentageChange);
        }
        catch (CurrentAccountIsNullException)
        {
            MessageBox.Show(this, "Сначала создайте хотя бы одну учетную запись.", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (crashLogs != null)
        {
            _crashHandler.HandleCrash(crashLogs);
        }
    }

    private void OnDownloadPercentageChange(int i)
    {
        DownloadProgressBar.Visibility = i == 100 ? Visibility.Hidden : Visibility.Visible;
        DownloadProgressBar.Value = i;
    }

    private void OpenAboutWindow(object sender, RoutedEventArgs e)
    {
        new AboutWindow().ShowDialog();
    }

    private void OnChangeAccountButtonClick(object sender, RoutedEventArgs e)
    {
        var accountsWindow = _accountsWindowFactory.Create();
        accountsWindow.Closed += (_, _) => PopulateCurrentAccountComboBox();
        accountsWindow.ShowDialog();
    }

    private void OnCurrentAccountComboBoxSelection(object sender, RoutedEventArgs e)
    {
        var newAccount = (Account?)CurrentAccountComboBox.SelectedItem;
        if (newAccount != null)
        {
            _currentAccountProvider.CurrentAccount = newAccount;
        }
    }
}