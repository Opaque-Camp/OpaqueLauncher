using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MinecraftStarter _minecraftStarter;

    public MainWindow(MinecraftStarter minecraftStarter)
    {
        _minecraftStarter = minecraftStarter;
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

    private void OnLaunchButtonClick(object sender, RoutedEventArgs e)
    {
        try
        {
            _minecraftStarter.StartMinecraft();
        }
        catch (MinecraftStartFailureException ex)
        {
            MessageBox.Show(this, ex.Message, "Ошибка запуска Minecraft", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}