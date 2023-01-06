namespace OpaqueCamp.Launcher.Core;

public interface IMinecraftVersionMetadataProvider
{
    Task<IMinecraftVersionMetadata> GetVersionMetadataAsync();
}