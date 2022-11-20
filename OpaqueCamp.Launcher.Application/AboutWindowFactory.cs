using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

public sealed class AboutWindowFactory
{
    private readonly LauncherVersionProvider _launcherVersionProvider;

    public AboutWindowFactory(LauncherVersionProvider launcherVersionProvider)
    {
        _launcherVersionProvider = launcherVersionProvider;
    }

    public AboutWindow Create()
    {
        return new(_launcherVersionProvider);
    }
}