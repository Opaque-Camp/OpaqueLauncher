using System.Reflection;

namespace OpaqueCamp.Launcher.Core;

public sealed class LauncherVersionProvider
{
    public Version LauncherVersion => Assembly.GetEntryAssembly()!.GetName().Version!;
}