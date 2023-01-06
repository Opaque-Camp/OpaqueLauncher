using System.ComponentModel;
using CmlLib.Core;
using CmlLib.Core.Downloader;

namespace OpaqueCamp.Launcher.Core.Tests;

public sealed class MinecraftInstallerAndRunnerTest
{
    private readonly Mock<IMinecraftLauncherFactory> _launcherFactory = new(MockBehavior.Strict);
    private readonly Mock<IMinecraftProcess> _process = new();
    private readonly Mock<IMinecraftLauncher> _launcher = new();
    private readonly Mock<IMinecraftLaunchOptionsProvider> _launchOptionsProvider = new(MockBehavior.Strict);
    private readonly Mock<MLaunchOption> _launchOptions = new(MockBehavior.Strict);
    private readonly Mock<IMinecraftVersionMetadataProvider> _versionMetadataProvider = new(MockBehavior.Strict);
    private readonly Mock<IMinecraftVersionMetadata> _versionMetadata = new();
    private readonly Mock<IMinecraftFilesDirProvider> _dirProvider = new(MockBehavior.Strict);
    private readonly Mock<IModsInstaller> _modsInstaller = new();
    private readonly Mock<IMinecraftCrashReportReader> _crashReportReader = new();

    private readonly MinecraftInstallerAndRunner _installerAndRunner;

    public MinecraftInstallerAndRunnerTest()
    {
        _dirProvider.Setup(p => p.DirPathForMinecraftFiles).Returns("C:\\Minecraft");

        _launcher.Setup(l => l.CreateProcessAsync(It.IsAny<IMinecraftVersionMetadata>(), It.IsAny<MLaunchOption>()))
            .ReturnsAsync(_process.Object);

        _launchOptionsProvider.Setup(p => p.GetLaunchOptions()).Returns(_launchOptions.Object);

        _launcherFactory.Setup(f => f.CreateLauncher()).Returns(_launcher.Object);
        _versionMetadataProvider.Setup(p => p.GetVersionMetadataAsync())
            .ReturnsAsync(_versionMetadata.Object);

        _installerAndRunner = new MinecraftInstallerAndRunner(_launcherFactory.Object, _launchOptionsProvider.Object,
            _versionMetadataProvider.Object, _dirProvider.Object, _modsInstaller.Object, _crashReportReader.Object);
    }

    [Fact]
    public async void RunMinecraftAsync_UsesProvidedVersionAndLaunchOptions()
    {
        // When
        await _installerAndRunner.InstallAndRunAsync();

        // Then
        _launcher.Verify(l => l.CreateProcessAsync(_versionMetadata.Object, _launchOptions.Object));
    }

    [Fact]
    public async void RunMinecraftAsync_AfterDownloadComplete_CallsBack()
    {
        // Given
        var callbackCalled = false;

        void DownloadCompleteCallback()
        {
            callbackCalled = true;
            // Should be called after download complete and before starting Minecraft
            _process.Verify(p => p.Start(), Times.Never);
        }

        // When
        await _installerAndRunner.InstallAndRunAsync(null, null, DownloadCompleteCallback);

        // Then
        callbackCalled.Should().BeTrue();
    }

    [Fact]
    public async void RunMinecraftAsync_SuccessfulRun_ReturnsNull()
    {
        // Given
        _process.Setup(p => p.ExitCode).Returns(0);

        // When
        var result = await _installerAndRunner.InstallAndRunAsync();

        // Then
        result.Should().BeNull();
    }

    [Fact]
    public async void RunMinecraftAsync_Crash_ReturnsLogsFromFile()
    {
        // Given
        _process.Setup(p => p.ExitCode).Returns(1);
        _crashReportReader.Setup(r => r.ReadLastCrashReport()).Returns("crash");

        // When
        var crashLogs = await _installerAndRunner.InstallAndRunAsync();

        // Then
        crashLogs.Should().Be("crash");
    }
}