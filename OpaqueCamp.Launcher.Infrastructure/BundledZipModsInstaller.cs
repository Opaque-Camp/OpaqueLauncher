using System.IO.Compression;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class BundledZipModsInstaller : IModsInstaller
{
    private readonly IMinecraftFilesDirProvider _minecraftFilesDirProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IModZipBundlePathProvider _modZipBundlePathProvider;

    public BundledZipModsInstaller(IModZipBundlePathProvider modZipBundlePathProvider,
        IMinecraftFilesDirProvider minecraftFilesDirProvider, IFileSystem fileSystem)
    {
        _modZipBundlePathProvider = modZipBundlePathProvider;
        _minecraftFilesDirProvider = minecraftFilesDirProvider;
        _fileSystem = fileSystem;
    }

    public async Task InstallModsAsync()
    {
        if (!IsModFolderEmpty())
        {
            return;
        }

        await Task.Run(() =>
        {
            try
            {
                ZipFile.ExtractToDirectory(_modZipBundlePathProvider.ModZipBundlePath,
                    _minecraftFilesDirProvider.ModsPath);
            }
            catch (IOException e)
            {
                if (!e.Message.Contains("already exists"))
                {
                    throw;
                }
            }
        });
    }

    private bool IsModFolderEmpty()
    {
        return _fileSystem.IsDirectoryEmpty(_minecraftFilesDirProvider.ModsPath);
    }
}