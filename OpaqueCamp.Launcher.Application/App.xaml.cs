using System;
using System.Windows;
using System.Windows.Threading;
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
                    .AddTransient<MinecraftCrashHandler>()
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

    private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // TODO: Move
        MessageBox.Show(
            "К нашему сожалению, лаунчер наткнулся на непредвиденную ошибку.\n\n" +
            "Просим сообщить об этой оказии @Protocs в нашем сервере Discord. В папке с лаунчером будет создан файл \"crash-info.txt\" с деталями ошибки, который стоит прикрепить к сообщению.",
            "Упс...",
            MessageBoxButton.OK,
            MessageBoxImage.Error
        );
        _host.Services.GetRequiredService<IFileSystem>()
            .WriteAllText($"crash-info-{DateTime.Now:yyyy-MM-dd-hh-mm-ss}.txt", e.Exception.ToString());
    }
}