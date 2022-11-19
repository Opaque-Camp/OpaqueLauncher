namespace OpaqueCamp.Launcher.Application;

using System.Windows;
using Core;

/// <summary>
/// Логика взаимодействия для AboutWindow.xaml
/// </summary>
public partial class AboutWindow
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