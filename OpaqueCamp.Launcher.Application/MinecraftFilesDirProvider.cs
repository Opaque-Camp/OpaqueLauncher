using OpaqueCamp.Launcher.Application.Properties;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

public sealed class MinecraftFilesDirProvider : IMinecraftFilesDirProvider
{
    public string DirPathForMinecraftFiles => Settings.Default.MinecraftFilesDirName;
}
