namespace OpaqueCamp.Launcher.Core
{
    public interface IModPackInfoProvider
    {
        string ModPackName { get; }

        Version UsedMinecraftVersion { get; }
    }
}