﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using CmlLib.Core.Installer.FabricMC;
using CmlLib.Core.VersionLoader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpaqueCamp.Launcher.Core;
using OpaqueCamp.Launcher.Core.Memory;
using OpaqueCamp.Launcher.Infrastructure;
using OpaqueCamp.Launcher.Infrastructure.Memory;

namespace OpaqueCamp.Launcher.Application;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public sealed partial class App
{
    private readonly IHost _host;

    public App()
    {
        // This will speed up the download process
        ServicePointManager.DefaultConnectionLimit = 256;

        _host = new HostBuilder()
            .ConfigureServices((_, services) =>
            {
                services
                    .AddTransient<IFileSystem, FileSystem>()
                    .AddTransient<IModPackInfoProvider, ModPackInfoProvider>()
                    .AddTransient<ISystemMemoryDetector, WindowsSystemMemoryDetector>()
                    .AddTransient<IJvmMemorySettings, JvmMemorySettings>()
                    .AddTransient<IJvmMemorySettingsStorage, JvmMemorySettingsStorage>()
                    .AddTransient<IServerConfigProvider, ServerConfigProvider>()
                    .AddTransient<IMinecraftFilesDirProvider, MinecraftFilesDirProvider>()
                    .AddTransient<IMinecraftLauncherFactory, CmlLibMinecraftLauncherFactory>()
                    .AddTransient<IVersionLoader, MojangAndFabricVersionLoader>()
                    .AddTransient<MojangVersionLoader>()
                    .AddTransient<FabricVersionLoader>()
                    .AddTransient<IMinecraftInstallerAndRunner, MinecraftInstallerAndRunner>()
                    .AddTransient<IMinecraftLaunchOptionsProvider, MinecraftLaunchOptionsProvider>()
                    .AddTransient<IMinecraftVersionMetadataProvider, MinecraftVersionMetadataProvider>()
                    .AddTransient<MinecraftCrashHandler>()
                    .AddTransient<IMinecraftCrashReportReader, MinecraftCrashReportReader>()
                    .AddTransient<MainWindow>()
                    .AddTransient<AboutWindowFactory>()
                    .AddTransient<AccountsWindowFactory>()
                    .AddTransient<AccountsViewModel>()
                    .AddTransient<IAccountRepository, JsonAccountRepository>()
                    .AddTransient<IAccountJsonPathProvider, SettingsAccountJsonPathProvider>()
                    .AddTransient<ICurrentAccountProvider, SettingsCurrentAccountProvider>()
                    .AddTransient<LauncherVersionProvider>()
                    .AddTransient<IModsInstaller, BundledZipModsInstaller>()
                    .AddTransient<IModZipBundlePathProvider, SettingsModZipBundlePathProvider>();
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