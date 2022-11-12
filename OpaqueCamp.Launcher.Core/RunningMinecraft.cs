using System.Diagnostics;

namespace OpaqueCamp.Launcher.Core;

public sealed class RunningMinecraft
{
    public event EventHandler? OnSuccessfulExit;
    public event EventHandler<MinecraftCrashLogs>? OnCrash;

    public RunningMinecraft(Process process)
    {
        process.Exited += (_, _) => OnExit(process);
    }

    private void OnExit(Process process)
    {
        if (process.ExitCode == 0)
        {
            OnSuccessfulExit?.Invoke(this, EventArgs.Empty);
            return;
        }

        OnCrash?.Invoke(this,
            new MinecraftCrashLogs(process.StandardOutput.ReadToEnd(), process.StandardError.ReadToEnd()));
    }
}