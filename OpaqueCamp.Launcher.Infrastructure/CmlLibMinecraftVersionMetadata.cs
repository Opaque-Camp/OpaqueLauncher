using CmlLib.Core;
using CmlLib.Core.Version;
using CmlLib.Core.VersionMetadata;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class CmlLibMinecraftVersionMetadata : IMinecraftVersionMetadata
{
    private readonly MVersionMetadata _metadata;

    public CmlLibMinecraftVersionMetadata(MVersionMetadata metadata)
    {
        _metadata = metadata;
    }

    public string Name => _metadata.Name;

    public async Task SaveAsync(MinecraftPath destination)
    {
        await _metadata.SaveAsync(destination);
    }
}