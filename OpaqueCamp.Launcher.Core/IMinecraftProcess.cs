namespace OpaqueCamp.Launcher.Core;

public interface IMinecraftProcess
{
    public void Start();
    public Task WaitForExitAsync();
    public int ExitCode { get; }
    public StreamReader StandardOutput { get; }
    public StreamReader StandardError { get; }
}