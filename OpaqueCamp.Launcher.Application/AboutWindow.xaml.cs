using System.Windows;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

/// <summary>
///     Логика взаимодействия для AboutWindow.xaml
/// </summary>
public sealed partial class AboutWindow
{
    public AboutWindow(LauncherVersionProvider versionProvider)
    {
        InitializeComponent();
        LauncherNameLabel.Content = "Opaque Launcher " + versionProvider.LauncherVersion;
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }
}