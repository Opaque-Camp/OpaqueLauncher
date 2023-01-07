using System.Windows;

namespace OpaqueCamp.Launcher.Application;

public sealed partial class MinecraftCrashWindow
{
    public MinecraftCrashWindow(string crashLogs)
    {
        InitializeComponent();
        CrashTextBlock.Text = crashLogs;
    }

    private void OnOkButtonClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OnCopyButtonClick(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(CrashTextBlock.Text);
    }
}