namespace OpaqueCamp.Launcher.Core;

public sealed class CurrentAccountIsNullException : Exception
{
    public CurrentAccountIsNullException() : base("Current account is null.")
    {
    }
}
