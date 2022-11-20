using System.Diagnostics;
using System.Runtime.Versioning;
using OpaqueCamp.Launcher.Core.Memory;

namespace OpaqueCamp.Launcher.Infrastructure.Memory;

[SupportedOSPlatform("linux")]
[SupportedOSPlatform("macos")]
public sealed class UnixSystemMemoryDetector : ISystemMemoryDetector
{
    public int GetSystemMemoryMegabytes()
    {
        string output;

        var info = new ProcessStartInfo("free -m")
        {
            FileName = "/bin/bash",
            Arguments = "-c \"free -m\"",
            RedirectStandardOutput = true
        };

        using (var process = Process.Start(info))
        {
            output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
        }

        var lines = output.Split("\n");
        var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

        return (int)long.Parse(memory[1]);
    }
}
