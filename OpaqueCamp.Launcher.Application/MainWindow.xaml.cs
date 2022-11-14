using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using AdonisUI.Controls;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : AdonisWindow
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
        try
        {
            var crashLogs = await _minecraftRunner.RunMinecraftAsync();
            if (crashLogs is not null)
            {
                _crashHandler.HandleCrash(crashLogs);
            }
        }
        catch (MinecraftStartFailureException ex)
        {
            AdonisUI.Controls.MessageBox.Show(this, ex.Message, "Ошибка запуска Minecraft", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Error);
        }
    }
}