using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpaqueCamp.Launcher.Core;
using OpaqueCamp.Launcher.Core.Memory;
using OpaqueCamp.Launcher.Infrastructure.Memory;

namespace OpaqueCamp.Launcher.Application;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private readonly IHost _host;

    public App()
    {
        _host = new HostBuilder()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddTransient<IEnvironmentService, EnvironmentService>()
                    .AddTransient<IFileSystem, FileSystem>()
                    .AddTransient<ILauncherInfoProvider, LauncherInfoProvider>()
                    .AddTransient<IApplicationPathProvider, ApplicationPathProvider>()
                    .AddTransient<ISystemMemoryDetector, WindowsSystemMemoryDetector>()
                    .AddTransient<IJvmMemorySettings, JvmMemorySettings>()
                    .AddTransient<IJvmMemorySettingsStorage, JvmMemorySettingsStorage>()
                    .AddTransient<ClasspathProvider>()
                    .AddTransient<IPathProvider, PathProvider>()
                    .AddTransient<JavaFinder>()
                    .AddTransient<MinecraftStarter>()
                    .AddTransient<MainWindow>();
            })
            .Build();
    }

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();
    }
}