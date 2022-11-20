namespace OpaqueCamp.Launcher.Core;

public interface IMinecraftFilesDirProvider
{
    string DirPathForMinecraftFiles { get; }

    string ModsPath => Path.Join(DirPathForMinecraftFiles, "mods");
}