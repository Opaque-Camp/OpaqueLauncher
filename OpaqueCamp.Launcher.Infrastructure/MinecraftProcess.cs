using System.Diagnostics;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class MinecraftProcess : IMinecraftProcess
{
    private readonly Process _process;

    public MinecraftProcess(Process process)
    {
        _process = process;
    }

    public void Start()
    {
        _process.Start();
    }

    public async Task WaitForExitAsync()
    {
        await _process.WaitForExitAsync();
    }

    public int ExitCode => _process.ExitCode;
    public StreamReader StandardOutput => _process.StandardOutput;
    public StreamReader StandardError => _process.StandardError;
}