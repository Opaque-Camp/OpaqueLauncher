﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpaqueCamp.Launcher.Core;

public sealed class MinecraftStarter
{
    private readonly JavaFinder _javaFinder;
    private readonly LauncherInfoProvider _launcherVersionProvider;
    private readonly DirectoryPathProvider _directoryPathProvider;
    private readonly ClasspathProvider _classpathProvider;
    private readonly JVMMemoryProvider _jvmMemoryProvider;

    public MinecraftStarter(JavaFinder javaFinder, LauncherInfoProvider launcherVersionProvider,
        DirectoryPathProvider directoryPathProvider, ClasspathProvider classpathProvider, JVMMemoryProvider jvmMemoryProvider)
    {
        _javaFinder = javaFinder;
        _launcherVersionProvider = launcherVersionProvider;
        _directoryPathProvider = directoryPathProvider;
        _classpathProvider = classpathProvider;
        _jvmMemoryProvider = jvmMemoryProvider;
    }

    void StartMinecraft()
    {
        var creds = new PlayerCredentials("lectureNice", "22d5ed98cb934e279b94eaa26f2ba401",
            "eyJhbGciOiJIUzI1NiJ9.eyJ4dWlkIjoiMjUzNTQyNDU2NDIyNDA5OCIsImFnZyI6IkFkdWx0Iiwic3ViIjoiZjFkNTgxZmYtN2NlZS00ZjZiLThlN2MtMTFmNjVjZmFhMWYzIiwibmJmIjoxNjY3NzM4MjM0LCJhdXRoIjoiWEJPWCIsInJvbGVzIjpbXSwiaXNzIjoiYXV0aGVudGljYXRpb24iLCJleHAiOjE2Njc4MjQ2MzQsImlhdCI6MTY2NzczODIzNCwicGxhdGZvcm0iOiJVTktOT1dOIiwieXVpZCI6Ijg0MzAxZjU1ODZhYmQyZGFjMDIxYmNkZWRiMDc3NjI0In0.oEU-cDcc0ps0AMZHEesPfeEqs4aDlJ2CBm6B4c16DRI");
        Process.Start("", string.Join(' ', Args(creds)));
    }

    private IEnumerable<string> Args(PlayerCredentials credentials)
    {
        var list = new List<string> { _javaFinder.GetJavawExePath() };
        list.AddRange(jvmArgs);
        list.Add($"-Xms{_jvmMemoryProvider.InitialMemoryAllocation}M");
        list.Add($"-Xmx{_jvmMemoryProvider.MaxMemoryAllocation}M");
        list.AddRange(launcherArgs);
        list.AddMany("-cp", _classpathProvider.GetClasspath());
        list.AddRange(fabricArgs);
        list.AddMany("--username", credentials.UserName);
        list.AddMany("--version",
            $"{_launcherVersionProvider.LauncherName} {_launcherVersionProvider.LauncherVersion}");
        list.AddMany("--gameDir", _directoryPathProvider.GameDirectoryPath);
        list.AddMany("--assetsDir", _directoryPathProvider.AssetsDirectoryPath);
        list.AddMany("--assetIndex", _launcherVersionProvider.LauncherVersion.MajorAndMinorString);
        list.AddMany("--uuid", credentials.Uuid);
        list.AddMany("--accessToken", credentials.AccessToken);
        list.AddMany("--userType", "mojang");
        list.AddMany("--versionType", "release");

        return list;
    }

    private static readonly List<string> jvmArgs = new()
    {
        "-XX:+UnlockExperimentalVMOptions",
        "-XX:+UseG1GC",
        "-XX:G1NewSizePercent = 20",
        "-XX:G1ReservePercent = 20",
        "-XX:MaxGCPauseMillis = 50",
        "-XX:G1HeapRegionSize = 32M",
        "-XX:+DisableExplicitGC",
        "-XX:+AlwaysPreTouch",
        "-XX:+ParallelRefProcEnabled",
        "-Dfile.encoding = UTF-8",
        "-Xss1M",
    };

    private static readonly List<string> launcherArgs = new()
    {
        "-Dminecraft.launcher.brand = java-minecraft-launcher",
        "-Dminecraft.launcher.version = 1.6.93",
    };

    private static readonly List<string> fabricArgs = new()
    {
        "-DFabricMcEmu=net.minecraft.client.main.Main",
        "net.fabricmc.loader.impl.launch.knot.KnotClient",
    };

    // Adds OptiFine Tweak for future
    private static readonly List<string> optfineArgs = new()
    {
        "--tweakClass",
        "optifine.OptiFineTweaker"
    };
}