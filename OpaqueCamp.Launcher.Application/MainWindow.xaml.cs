using System.Windows;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly CmlLibMinecraftRunner _minecraftRunner;
    private readonly MinecraftCrashHandler _crashHandler;

    public MainWindow(CmlLibMinecraftRunner minecraftRunner, MinecraftCrashHandler crashHandler)
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
        var crashLogs = await _minecraftRunner.RunMinecraftAsync(
            e => CurrentlyDownloadedFileLabel.Content =
                $"[{e.FileKind}] {e.FileName} - {e.ProgressedFileCount}/{e.TotalFileCount}",
            i => DownloadProgressLabel.Content = $"{i}%"
        );
        if (crashLogs != null)
        {
            _crashHandler.HandleCrash(crashLogs);
        }
    }
}