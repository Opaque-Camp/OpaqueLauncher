using CmlLib.Core;

namespace OpaqueCamp.Launcher.Core;

public interface IMinecraftVersionMetadata
{
    string Name { get; }

    Task SaveAsync(MinecraftPath destination);
}