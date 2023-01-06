using CmlLib.Core.VersionLoader;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class MinecraftVersionMetadataProvider : IMinecraftVersionMetadataProvider
{
    private readonly IVersionLoader _versionLoader;
    private readonly IModPackInfoProvider _modPackInfoProvider;

    public MinecraftVersionMetadataProvider(IVersionLoader versionLoader, IModPackInfoProvider modPackInfoProvider)
    {
        _versionLoader = versionLoader;
        _modPackInfoProvider = modPackInfoProvider;
    }

    public async Task<IMinecraftVersionMetadata> GetVersionMetadataAsync()
    {
        var versions = await _versionLoader.GetVersionMetadatasAsync();
        var fabricForOurMinecraftVersion = versions
            .First(v => v.Type == "fabric" && v.Name.Split('-').Last() == _modPackInfoProvider.UsedMinecraftVersion.ToString());
        return new CmlLibMinecraftVersionMetadata(fabricForOurMinecraftVersion);
    }
}