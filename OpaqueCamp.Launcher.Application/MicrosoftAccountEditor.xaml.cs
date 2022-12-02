using System.Windows;
using CmlLib.Core.Auth.Microsoft.UI.Wpf;

namespace OpaqueCamp.Launcher.Application;

public partial class MicrosoftAccountEditor
{
    public MicrosoftAccountEditor()
    {
        InitializeComponent();
    }

    private async void LogIn(object sender, RoutedEventArgs e)
    {
        var loginWindow = new MicrosoftLoginWindow();
        var session = await loginWindow.ShowLoginDialog();
        MessageBox.Show("Login success : " + session.Username);
    }
}