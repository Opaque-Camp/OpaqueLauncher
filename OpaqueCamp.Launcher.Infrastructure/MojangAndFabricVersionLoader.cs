using CmlLib.Core.Installer.FabricMC;
using CmlLib.Core.Version;
using CmlLib.Core.VersionLoader;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class MojangAndFabricVersionLoader : IVersionLoader
{
    private readonly MojangVersionLoader _mojangVersionLoader;
    private readonly FabricVersionLoader _fabricVersionLoader;

    public MojangAndFabricVersionLoader(MojangVersionLoader mojangVersionLoader,
        FabricVersionLoader fabricVersionLoader)
    {
        _mojangVersionLoader = mojangVersionLoader;
        _fabricVersionLoader = fabricVersionLoader;
    }

    public async Task<MVersionCollection> GetVersionMetadatasAsync()
    {
        var mojangVersions = await _mojangVersionLoader.GetVersionMetadatasAsync();
        var fabricVersions = await _fabricVersionLoader.GetVersionMetadatasAsync();
        return new MVersionCollection(mojangVersions.Concat(fabricVersions).ToArray());
    }

    public MVersionCollection GetVersionMetadatas()
    {
        var mojangVersions = _mojangVersionLoader.GetVersionMetadatas();
        var fabricVersions = _fabricVersionLoader.GetVersionMetadatas();
        return new MVersionCollection(mojangVersions.Concat(fabricVersions).ToArray());
    }
}