namespace OpaqueCamp.Launcher.Core;

public sealed class EnvironmentService : IEnvironmentService
{
    public string? GetEnvironmentVariable(string name) => Environment.GetEnvironmentVariable(name);
}
