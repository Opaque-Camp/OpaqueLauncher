namespace OpaqueCamp.Launcher.Core.Memory;

public sealed class SystemMemoryDetectionException : Exception
{
    public SystemMemoryDetectionException(string message) : base(message)
    {
    }
}