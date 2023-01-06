namespace OpaqueCamp.Launcher.Core;

public interface IMinecraftFilesDirProvider
{
    string DirPathForMinecraftFiles { get; }

    string ModsPath => Path.Join(DirPathForMinecraftFiles, "mods");

    string CrashReportsPath => Path.Join(DirPathForMinecraftFiles, "crash-reports");
}