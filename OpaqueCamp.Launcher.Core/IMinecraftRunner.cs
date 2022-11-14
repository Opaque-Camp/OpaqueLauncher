namespace OpaqueCamp.Launcher.Core;

public interface IMinecraftRunner
{
    /// <summary>
    /// Runs the Minecraft client asynchronously.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> which completes right after Minecraft exits.
    /// The task contains <c>null</c> if Minecraft exited successfully,
    /// or a <see cref="MinecraftCrashLogs"/> instance if it crashed.
    /// </returns>
    /// <exception cref="MinecraftStartFailureException">
    /// Thrown when the Minecraft client could not be started due to various Java-related problems,
    /// such as missing Java installation or classpath generation problems.
    /// The inner exception will contain the error that caused the startup to fail.
    /// </exception>
    Task<MinecraftCrashLogs?> RunMinecraftAsync();
}