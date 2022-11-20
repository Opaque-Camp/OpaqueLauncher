using System.Windows;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

public partial class MinecraftCrashWindow
{
    public MinecraftCrashWindow(MinecraftCrashLogs crashLogs)
    {
        InitializeComponent();
        DataContext = crashLogs;
    }

    private void OnOkClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void copy_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(errorBox.Text);
    }
}