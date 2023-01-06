namespace OpaqueCamp.Launcher.Core.Tests;

public sealed class MinecraftCrashReportReaderTest
{
    private readonly Mock<IFileSystem> _fileSystem = new();
    private readonly Mock<IMinecraftFilesDirProvider> _minecraftFilesDirProvider = new();
    private readonly MinecraftCrashReportReader _reader;

    public MinecraftCrashReportReaderTest()
    {
        _minecraftFilesDirProvider.Setup(p => p.CrashReportsPath).Returns("C:\\Minecraft\\crash-reports");

        _reader = new MinecraftCrashReportReader(_fileSystem.Object, _minecraftFilesDirProvider.Object);
    }

    [Fact]
    public void ReadLastCrashReport_FewFiles_ReturnNewestOnesContent()
    {
        // Given
        var file1 = "C:\\Minecraft\\crash-reports\\crash-2023-01-01_00.00.00-client.txt";
        var file2 = "C:\\Minecraft\\crash-reports\\crash-2023-01-02_00.00.00-client.txt";

        _fileSystem.Setup(fs => fs.GetFilesInDirectory("C:\\Minecraft\\crash-reports"))
            .Returns(new[] { file1, file2 });
        _fileSystem.Setup(fs => fs.GetFileCreationTime(file1)).Returns(new DateTime(2023, 1, 1));
        _fileSystem.Setup(fs => fs.GetFileCreationTime(file2)).Returns(new DateTime(2023, 1, 2));
        _fileSystem.Setup(fs => fs.ReadAllText(file2)).Returns("content");

        // When
        var contents = _reader.ReadLastCrashReport();

        // Then
        contents.Should().Be("content");
    }

    [Fact]
    public void ReadLastCrashReport_NoFiles_ReturnNull()
    {
        // Given
        _fileSystem.Setup(fs => fs.GetFilesInDirectory("C:\\Minecraft\\crash-reports"))
            .Returns(Array.Empty<string>());

        // When
        var contents = _reader.ReadLastCrashReport();

        // Then
        contents.Should().BeNull();
    }

    [Fact]
    public void ReadLastCrashReport_NoCrashReportsDirectory_ReturnNull()
    {
        // Given
        _fileSystem.Setup(fs => fs.GetFilesInDirectory("C:\\Minecraft\\crash-reports"))
            .Throws(new DirectoryNotFoundException());

        // When
        var contents = _reader.ReadLastCrashReport();

        // Then
        contents.Should().BeNull();
    }
}