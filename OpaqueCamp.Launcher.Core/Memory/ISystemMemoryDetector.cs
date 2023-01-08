namespace OpaqueCamp.Launcher.Core.Memory;

public interface ISystemMemoryDetector
{
    /// <summary>
    ///     Returns the total amount of RAM in the system in megabytes.
    /// </summary>
    /// <exception cref="SystemMemoryDetectionException">RAM detection was failed for an unexpected reason.</exception>
    int GetSystemMemoryMegabytes();
}