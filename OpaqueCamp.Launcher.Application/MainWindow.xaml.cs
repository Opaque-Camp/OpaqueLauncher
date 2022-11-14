using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using OpaqueCamp.Launcher.Core;
using CmlLib.Core;
using CmlLib.Core.Auth;
using System;

namespace OpaqueCamp.Launcher.Application;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly IMinecraftRunner _minecraftRunner;
    private readonly MinecraftCrashHandler _crashHandler;

    public MainWindow(IMinecraftRunner minecraftRunner, MinecraftCrashHandler crashHandler)
    {
        _minecraftRunner = minecraftRunner;
        _crashHandler = crashHandler;

        InitializeComponent();

#if DEBUG
        Window.Title += " [DEBUG]";
        debugWindowLabel.Visibility = Visibility.Visible;
#endif
    }

    // Double-click
    private void OpenDebugWindow(object sender, MouseButtonEventArgs e)
    {
        var debugWindow = new DebugWindow();
        debugWindow.Show();
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private async void OnLaunchButtonClick(object sender, RoutedEventArgs e)
    {
        System.Net.ServicePointManager.DefaultConnectionLimit = 256;
        var path = new MinecraftPath("opaque-vanilla");
        var launcher = new CMLauncher(path);
        launcher.FileChanged += (e) =>
        {
            CurrentlyDownloadedFileLabel.Content = $"[{e.FileKind}] {e.FileName} - {e.ProgressedFileCount}/{e.TotalFileCount}";
        };
        launcher.ProgressChanged += (s, e) =>
        {
            DownloadProgressLabel.Content = $"{e.ProgressPercentage}%";
        };
        var process = await launcher.CreateProcessAsync("1.19.2", new MLaunchOption
        {
            MaximumRamMb = 2048,
            Session = MSession.GetOfflineSession("OpaqueLauncher"),
            ServerIp = "oc.aboba.host"
        });
        process.Start();
    }
}