﻿using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using CmlLib.Core.Downloader;
using CommunityToolkit.Mvvm.ComponentModel;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
[ObservableObject]
public partial class MainWindow
{
    private readonly AboutWindowFactory _aboutWindowFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly AccountsWindowFactory _accountsWindowFactory;
    private readonly MinecraftCrashHandler _crashHandler;
    private readonly ICurrentAccountProvider _currentAccountProvider;
    private readonly IMinecraftInstallerAndRunner _minecraftInstallerAndRunner;

    public MainWindow(IMinecraftInstallerAndRunner minecraftInstallerAndRunner, MinecraftCrashHandler crashHandler,
        AccountsWindowFactory accountsWindowFactory, ICurrentAccountProvider currentAccountProvider,
        IAccountRepository accountRepository, AboutWindowFactory aboutWindowFactory)
    {
        _minecraftInstallerAndRunner = minecraftInstallerAndRunner;
        _crashHandler = crashHandler;
        _accountsWindowFactory = accountsWindowFactory;
        _currentAccountProvider = currentAccountProvider;
        _accountRepository = accountRepository;
        _aboutWindowFactory = aboutWindowFactory;

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
        foreach (var account in accounts) CurrentAccountComboBox.Items.Add(account);

        if (_currentAccountProvider.CurrentAccount == null && accounts.Count != 0)
            _currentAccountProvider.CurrentAccount = accounts[0];

        CurrentAccountComboBox.SelectedItem = _currentAccountProvider.CurrentAccount;
    }

    [ObservableProperty] private string downloadProgressString = "";

    private async void OnLaunchButtonClick(object sender, RoutedEventArgs _)
    {
        void DownloadProgressHandler(DownloadFileChangedEventArgs e) => DownloadProgressString =
            $"[{e.FileKind}] {e.FileName} - {e.ProgressedFileCount}/{e.TotalFileCount}";

        string? crashLogs;

        MainGrid.RowDefinitions[1].Height = new GridLength(20);
        LaunchButton.IsEnabled = false;

        try
        {
            crashLogs = await _minecraftInstallerAndRunner.InstallAndRunAsync(DownloadProgressHandler,
                OnDownloadPercentageChange, HideDownloadProgressBar);
        }
        catch (CurrentAccountIsNullException)
        {
            MessageBox.Show(this, "Сначала создайте хотя бы одну учетную запись.", "", MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }
        catch (WebException)
        {
            MessageBox.Show(this,
                "Не удалось подготовить игру к запуску из-за проблем с сетью. Проверьте наличие доступа к Интернету или повторите позже.",
                "Не удалось запустить игру", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }
        finally
        {
            HideDownloadProgressBar();
            LaunchButton.IsEnabled = true;
        }

        if (crashLogs != null) _crashHandler.HandleCrash(crashLogs);
    }

    private void HideDownloadProgressBar() => MainGrid.RowDefinitions[1].Height = new GridLength(0);

    private void OnDownloadPercentageChange(object? _, ProgressChangedEventArgs e)
    {
        DownloadProgressBar.Value = e.ProgressPercentage;
    }

    private void OpenAboutWindow(object sender, RoutedEventArgs e)
    {
        _aboutWindowFactory.Create().ShowDialog();
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
        if (newAccount != null) _currentAccountProvider.CurrentAccount = newAccount;
    }
}