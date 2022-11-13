namespace OpaqueCamp.Launcher.Core;

public sealed record MinecraftCrashLogs(string StandardOutput, string StandardError)
{
    public string JoinedLogs => StandardOutput + "\n\n--------------------\n\n" + StandardError;
}