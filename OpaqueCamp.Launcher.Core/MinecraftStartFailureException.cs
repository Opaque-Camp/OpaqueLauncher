namespace OpaqueCamp.Launcher.Core;

public sealed class MinecraftStartFailureException : Exception
{
    public MinecraftStartFailureException(Exception? innerException) : base("Minecraft could not be started.",
        innerException)
    {
    }

    public override string Message => base.Message + " " + InnerException?.Message;
}