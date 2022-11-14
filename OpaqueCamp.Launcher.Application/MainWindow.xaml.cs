using System.Windows;
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
#endif
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