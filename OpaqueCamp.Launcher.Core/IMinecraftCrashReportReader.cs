namespace OpaqueCamp.Launcher.Core;

public interface IMinecraftCrashReportReader
{
    string? ReadLastCrashReport();
}