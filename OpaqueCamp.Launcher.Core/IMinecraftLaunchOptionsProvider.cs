using CmlLib.Core;

namespace OpaqueCamp.Launcher.Core;

public interface IMinecraftLaunchOptionsProvider
{
    MLaunchOption GetLaunchOptions();
}