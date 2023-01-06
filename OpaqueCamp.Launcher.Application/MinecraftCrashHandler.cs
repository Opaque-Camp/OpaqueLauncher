namespace OpaqueCamp.Launcher.Application;

public sealed class MinecraftCrashHandler
{
    public void HandleCrash(string crashLogs)
    {
        new MinecraftCrashWindow(crashLogs).ShowDialog();
    }
}