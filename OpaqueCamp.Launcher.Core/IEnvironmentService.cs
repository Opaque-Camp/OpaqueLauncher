namespace OpaqueCamp.Launcher.Core;

public interface IEnvironmentService
{
    string? GetEnvironmentVariable(string name);
}
