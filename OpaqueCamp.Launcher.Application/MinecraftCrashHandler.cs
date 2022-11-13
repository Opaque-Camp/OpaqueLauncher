using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

public sealed class MinecraftCrashHandler
{
    public void HandleCrash(MinecraftCrashLogs crashLogs)
    {
        new MinecraftCrashWindow(crashLogs).ShowDialog();
    }
}