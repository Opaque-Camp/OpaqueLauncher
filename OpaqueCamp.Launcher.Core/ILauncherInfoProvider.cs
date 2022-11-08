namespace OpaqueCamp.Launcher.Core
{
    public interface ILauncherInfoProvider
    {
        string LauncherName { get; }

        Version LauncherVersion { get; }

        string LauncherNameAndVersion { get; }
    }
}