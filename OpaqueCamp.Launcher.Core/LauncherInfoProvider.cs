namespace OpaqueCamp.Launcher.Core;

public sealed class LauncherInfoProvider : ILauncherInfoProvider
{
    public string LauncherName => "Opaque Vanilla";

    public Version LauncherVersion => new(1, 19, 2);

    public string LauncherNameAndVersion => $"{LauncherName} {LauncherVersion}";
}
