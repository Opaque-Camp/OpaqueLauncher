using OpaqueCamp.Launcher.Application.Properties;
using OpaqueCamp.Launcher.Infrastructure;

namespace OpaqueCamp.Launcher.Application;

public sealed class SettingsModZipBundlePathProvider : IModZipBundlePathProvider
{
    public string ModZipBundlePath => Settings.Default.ModZipBundlePath;
}