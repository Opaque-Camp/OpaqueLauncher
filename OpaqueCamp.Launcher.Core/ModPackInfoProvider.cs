namespace OpaqueCamp.Launcher.Core;

public sealed class ModPackInfoProvider : IModPackInfoProvider
{
    public string ModPackName => "Opaque Vanilla";

    public Version UsedMinecraftVersion => new(1, 19, 2);
}
