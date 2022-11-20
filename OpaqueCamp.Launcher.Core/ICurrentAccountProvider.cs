namespace OpaqueCamp.Launcher.Core;

public interface ICurrentAccountProvider
{
    /// <summary>
    ///     The account that the user has selected to authenticate with in Minecraft, or
    ///     <c>null/c> if no accounts were ever created.
    /// </summary>
    Account? CurrentAccount { get; set; }
}