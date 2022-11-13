using OpaqueCamp.Launcher.Core;
using System.Windows;

namespace OpaqueCamp.Launcher.Application;

public partial class MinecraftCrashWindow : Window
{
    public MinecraftCrashWindow(MinecraftCrashLogs crashLogs)
    {
        InitializeComponent();
        DataContext = crashLogs;
    }
}