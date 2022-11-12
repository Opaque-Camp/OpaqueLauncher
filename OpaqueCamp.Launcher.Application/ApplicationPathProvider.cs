using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

public sealed class ApplicationPathProvider : IApplicationPathProvider
{
    public string ApplicationPath => System.AppDomain.CurrentDomain.BaseDirectory;
}