namespace OpaqueCamp.Launcher.Core;

public sealed class MinecraftStartFailureException : Exception
{
    public MinecraftStartFailureException(string message = "", Exception? innerException = null) : base(message,
        innerException)
    {
    }

    public override string Message => InnerException?.Message ?? "";
}