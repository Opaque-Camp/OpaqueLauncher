using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpaqueLauncher;

public sealed class MinecraftStarter
{
    private readonly JavaFinder _javaFinder;
    private readonly LauncherInfoProvider _launcherVersionProvider;
    private readonly DirectoryPathProvider _directoryPathProvider;

    public MinecraftStarter(JavaFinder javaFinder, LauncherInfoProvider launcherVersionProvider, DirectoryPathProvider directoryPathProvider)
    {
        _javaFinder = javaFinder;
        _launcherVersionProvider = launcherVersionProvider;
        _directoryPathProvider = directoryPathProvider;
    }

    void StartMinecraft()
    {
        /*
         * 0] C:\Program Files\Java\jdk-18.0.2.1\bin\javaw.exe

[1] -XX:+UnlockExperimentalVMOptions

[2] -XX:+UseG1GC

[3] -XX:G1NewSizePercent=20

[4] -XX:G1ReservePercent=20

[5] -XX:MaxGCPauseMillis=50

[6] -XX:G1HeapRegionSize=32M

[7] -XX:+DisableExplicitGC

[8] -XX:+AlwaysPreTouch

[9] -XX:+ParallelRefProcEnabled

[10] -Xms2048M

[11] -Xmx4096M

[12] -Dfile.encoding=UTF-8

[13] -XX:HeapDumpPath=MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump

[14] -Xss1M

[15] -Djava.library.path=C:\Users\regis\Documents\Opaque Vanilla\.\game\versions\Opaque Vanilla 1.19.2\natives

[16] -Dminecraft.launcher.brand=java-minecraft-launcher

[17] -Dminecraft.launcher.version=1.6.84-j

[18] -cp

[19] C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\net\fabricmc\tiny-mappings-parser\0.3.0+build.17\tiny-mappings-parser-0.3.0+build.17.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\net\fabricmc\sponge-mixin\0.11.4+mixin.0.8.5\sponge-mixin-0.11.4+mixin.0.8.5.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\net\fabricmc\tiny-remapper\0.8.2\tiny-remapper-0.8.2.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\net\fabricmc\access-widener\2.1.0\access-widener-2.1.0.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\ow2\asm\asm\9.3\asm-9.3.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\ow2\asm\asm-analysis\9.3\asm-analysis-9.3.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\ow2\asm\asm-commons\9.3\asm-commons-9.3.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\ow2\asm\asm-tree\9.3\asm-tree-9.3.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\ow2\asm\asm-util\9.3\asm-util-9.3.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\net\fabricmc\intermediary\1.19.2\intermediary-1.19.2.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\net\fabricmc\fabric-loader\0.14.10\fabric-loader-0.14.10.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\mojang\logging\1.0.0\logging-1.0.0.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\mojang\blocklist\1.0.10\blocklist-1.0.10.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\ru\tln4\empty\0.1\empty-0.1.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\github\oshi\oshi-core\5.8.5\oshi-core-5.8.5.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\net\java\dev\jna\jna\5.10.0\jna-5.10.0.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\net\java\dev\jna\jna-platform\5.10.0\jna-platform-5.10.0.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\slf4j\slf4j-api\1.8.0-beta4\slf4j-api-1.8.0-beta4.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\apache\logging\log4j\log4j-slf4j18-impl\2.17.0\log4j-slf4j18-impl-2.17.0.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\ibm\icu\icu4j\70.1\icu4j-70.1.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\mojang\javabridge\1.2.24\javabridge-1.2.24.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\net\sf\jopt-simple\jopt-simple\5.0.4\jopt-simple-5.0.4.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\io\netty\netty-common\4.1.77.Final\netty-common-4.1.77.Final.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\io\netty\netty-buffer\4.1.77.Final\netty-buffer-4.1.77.Final.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\io\netty\netty-codec\4.1.77.Final\netty-codec-4.1.77.Final.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\io\netty\netty-handler\4.1.77.Final\netty-handler-4.1.77.Final.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\io\netty\netty-resolver\4.1.77.Final\netty-resolver-4.1.77.Final.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\io\netty\netty-transport\4.1.77.Final\netty-transport-4.1.77.Final.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\io\netty\netty-transport-native-unix-common\4.1.77.Final\netty-transport-native-unix-common-4.1.77.Final.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\io\netty\netty-transport-classes-epoll\4.1.77.Final\netty-transport-classes-epoll-4.1.77.Final.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\google\guava\failureaccess\1.0.1\failureaccess-1.0.1.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\google\guava\guava\31.0.1-jre\guava-31.0.1-jre.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\apache\commons\commons-lang3\3.12.0\commons-lang3-3.12.0.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\commons-io\commons-io\2.11.0\commons-io-2.11.0.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\commons-codec\commons-codec\1.15\commons-codec-1.15.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\mojang\brigadier\1.0.18\brigadier-1.0.18.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\mojang\datafixerupper\5.0.28\datafixerupper-5.0.28.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\google\code\gson\gson\2.8.9\gson-2.8.9.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\mojang\authlib\3.11.49\authlib-3.11.49.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\apache\commons\commons-compress\1.21\commons-compress-1.21.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\apache\httpcomponents\httpclient\4.5.13\httpclient-4.5.13.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\commons-logging\commons-logging\1.2\commons-logging-1.2.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\apache\httpcomponents\httpcore\4.4.14\httpcore-4.4.14.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\it\unimi\dsi\fastutil\8.5.6\fastutil-8.5.6.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\apache\logging\log4j\log4j-api\2.17.0\log4j-api-2.17.0.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\apache\logging\log4j\log4j-core\2.17.0\log4j-core-2.17.0.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl\3.3.1\lwjgl-3.3.1.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl\3.3.1\lwjgl-3.3.1-natives-windows.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl\3.3.1\lwjgl-3.3.1-natives-windows-x86.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-jemalloc\3.3.1\lwjgl-jemalloc-3.3.1.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-jemalloc\3.3.1\lwjgl-jemalloc-3.3.1-natives-windows.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-jemalloc\3.3.1\lwjgl-jemalloc-3.3.1-natives-windows-x86.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-openal\3.3.1\lwjgl-openal-3.3.1.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-openal\3.3.1\lwjgl-openal-3.3.1-natives-windows.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-openal\3.3.1\lwjgl-openal-3.3.1-natives-windows-x86.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-opengl\3.3.1\lwjgl-opengl-3.3.1.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-opengl\3.3.1\lwjgl-opengl-3.3.1-natives-windows.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-opengl\3.3.1\lwjgl-opengl-3.3.1-natives-windows-x86.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-glfw\3.3.1\lwjgl-glfw-3.3.1.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-glfw\3.3.1\lwjgl-glfw-3.3.1-natives-windows.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-glfw\3.3.1\lwjgl-glfw-3.3.1-natives-windows-x86.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-stb\3.3.1\lwjgl-stb-3.3.1.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-stb\3.3.1\lwjgl-stb-3.3.1-natives-windows.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-stb\3.3.1\lwjgl-stb-3.3.1-natives-windows-x86.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-tinyfd\3.3.1\lwjgl-tinyfd-3.3.1.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-tinyfd\3.3.1\lwjgl-tinyfd-3.3.1-natives-windows.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\org\lwjgl\lwjgl-tinyfd\3.3.1\lwjgl-tinyfd-3.3.1-natives-windows-x86.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\mojang\text2speech\1.13.9\text2speech-1.13.9.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\libraries\com\mojang\text2speech\1.13.9\text2speech-1.13.9-natives-windows.jar;C:\Users\regis\Documents\Opaque Vanilla\.\game\versions\Opaque Vanilla 1.19.2\Opaque Vanilla 1.19.2.jar

[20] -DFabricMcEmu= net.minecraft.client.main.Main 

[21] net.fabricmc.loader.impl.launch.knot.KnotClient

[22] --server

[23] oc.aboba.host

[24] --username

[25] lectureNice

[26] --version

[27] Opaque Vanilla 1.19.2

[28] --gameDir

[29] C:\Users\regis\Documents\Opaque Vanilla\.\game

[30] --assetsDir

[31] C:\Users\regis\Documents\Opaque Vanilla\.\game\assets

[32] --assetIndex

[33] 1.19

[34] --uuid

[35] 22d5ed98cb934e279b94eaa26f2ba401

[36] --accessToken

[37] eyJhbGciOiJIUzI1NiJ9.eyJ4dWlkIjoiMjUzNTQyNDU2NDIyNDA5OCIsImFnZyI6IkFkdWx0Iiwic3ViIjoiZjFkNTgxZmYtN2NlZS00ZjZiLThlN2MtMTFmNjVjZmFhMWYzIiwibmJmIjoxNjY3NzM4MjM0LCJhdXRoIjoiWEJPWCIsInJvbGVzIjpbXSwiaXNzIjoiYXV0aGVudGljYXRpb24iLCJleHAiOjE2Njc4MjQ2MzQsImlhdCI6MTY2NzczODIzNCwicGxhdGZvcm0iOiJVTktOT1dOIiwieXVpZCI6Ijg0MzAxZjU1ODZhYmQyZGFjMDIxYmNkZWRiMDc3NjI0In0.oEU-cDcc0ps0AMZHEesPfeEqs4aDlJ2CBm6B4c16DRI

[38] --clientId

[39] 

[40] --xuid

[41] 

[42] --userType

[43] mojang

[44] --versionType

[45] release

[46] --width

[47] 925

[48] --height

[49] 530
         */

        var userName = "lectureNice";
        Process.Start("", string.Join(' ', Args(userName)));
    }

    private IEnumerable<string> Args(string userName)
    {
        var list = new List<string>
        {
            _javaFinder.FindAbsoluteJavaExePath()
        };
        list.AddRange(jvmArgs);
        list.AddRange(launcherArgs);
        // TODO: classpath
        list.AddRange(fabricArgs);
        list.AddRange(new List<string> { "--username", userName });
        list.AddRange(new List<string> { "--version", $"{_launcherVersionProvider.LauncherName} {_launcherVersionProvider.LauncherVersion}" });

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
        "-Xms2048M",
        "-Xmx4096M",
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
}
