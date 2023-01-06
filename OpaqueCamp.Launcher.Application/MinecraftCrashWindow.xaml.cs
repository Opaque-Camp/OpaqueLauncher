using System.Windows;

namespace OpaqueCamp.Launcher.Application;

public partial class MinecraftCrashWindow
{
    public MinecraftCrashWindow(string crashLogs)
    {
        InitializeComponent();
        CrashTextBlock.Text = crashLogs;
    }

    private void OnOkClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void copy_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(CrashTextBlock.Text);
    }
}