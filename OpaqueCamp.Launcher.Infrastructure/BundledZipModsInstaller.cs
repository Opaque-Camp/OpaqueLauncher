﻿using System.IO.Compression;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class BundledZipModsInstaller : IModsInstaller
{
    private readonly IModZipBundlePathProvider _modZipBundlePathProvider;
    private readonly IMinecraftFilesDirProvider _minecraftFilesDirProvider;

    public BundledZipModsInstaller(IModZipBundlePathProvider modZipBundlePathProvider,
        IMinecraftFilesDirProvider minecraftFilesDirProvider)
    {
        _modZipBundlePathProvider = modZipBundlePathProvider;
        _minecraftFilesDirProvider = minecraftFilesDirProvider;
    }

    public async Task InstallModsAsync()
    {
        await Task.Run(() => ZipFile.ExtractToDirectory(_modZipBundlePathProvider.ModZipBundlePath,
            _minecraftFilesDirProvider.ModsPath));
    }
}