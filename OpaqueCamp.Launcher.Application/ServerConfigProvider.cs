using OpaqueCamp.Launcher.Application.Properties;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

public sealed class ServerConfigProvider : IServerConfigProvider
{
    public string ServerAddress => Settings.Default.ServerHostname;
}