namespace OpaqueCamp.Launcher.Core;

public sealed class MinecraftCrashReportReader : IMinecraftCrashReportReader
{
    private readonly IFileSystem _fileSystem;
    private readonly IMinecraftFilesDirProvider _minecraftFilesDirProvider;

    public MinecraftCrashReportReader(IFileSystem fileSystem, IMinecraftFilesDirProvider minecraftFilesDirProvider)
    {
        _fileSystem = fileSystem;
        _minecraftFilesDirProvider = minecraftFilesDirProvider;
    }

    public string? ReadLastCrashReport()
    {
        string[] crashReports;
        try
        {
            crashReports = _fileSystem.GetFilesInDirectory(_minecraftFilesDirProvider.CrashReportsPath);
        }
        catch (DirectoryNotFoundException)
        {
            return null;
        }

        var lastReport = crashReports.MaxBy(path => _fileSystem.GetFileCreationTime(path));
        return lastReport == null ? null : _fileSystem.ReadAllText(lastReport);
    }
}