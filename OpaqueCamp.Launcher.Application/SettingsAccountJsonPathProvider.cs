using OpaqueCamp.Launcher.Application.Properties;
using OpaqueCamp.Launcher.Infrastructure;

namespace OpaqueCamp.Launcher.Application;

public sealed class SettingsAccountJsonPathProvider : IAccountJsonPathProvider
{
    public string AccountJsonPath => Settings.Default.AccountJsonPath;
}