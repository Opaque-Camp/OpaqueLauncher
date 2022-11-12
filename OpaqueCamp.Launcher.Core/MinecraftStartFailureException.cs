namespace OpaqueCamp.Launcher.Core;

public sealed class MinecraftStartFailureException : Exception
{
    public MinecraftStartFailureException(Exception? innerException) : base("", innerException)
    {
    }

    public override string Message => InnerException?.Message ?? "";
}