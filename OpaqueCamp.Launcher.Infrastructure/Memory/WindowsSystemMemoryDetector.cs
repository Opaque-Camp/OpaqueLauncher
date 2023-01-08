using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Runtime.Versioning;
using OpaqueCamp.Launcher.Core.Memory;

namespace OpaqueCamp.Launcher.Infrastructure.Memory;

[SupportedOSPlatform("windows")]
public sealed class WindowsSystemMemoryDetector : ISystemMemoryDetector
{
    public int GetSystemMemoryMegabytes()
    {
        return IsWindows8OrNewer() ? GetMemoryForWindows8AndOlder() : GetMemoryForWindows7();
    }

    private bool IsWindows8OrNewer()
    {
        return OperatingSystem.IsWindowsVersionAtLeast(6, 3);
    }

    private int GetMemoryForWindows8AndOlder()
    {
        var searcher = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
        var searcherEnumerator = searcher.Get().GetEnumerator();
        searcherEnumerator.MoveNext();
        return (int)((ulong)searcherEnumerator.Current["TotalPhysicalMemory"] / (1024 * 1024));
    }

    private int GetMemoryForWindows7()
    {
        string output;

        var info = new ProcessStartInfo
        {
            FileName = "wmic",
            Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value",
            CreateNoWindow = true,
            RedirectStandardOutput = true
        };

        using (var process = Process.Start(info))
        {
            if (process == null)
            {
                throw new SystemMemoryDetectionException("wmic process is null");
            }

            output = process.StandardOutput.ReadToEnd();
        }

        var lines = output.Trim().Split("\n");
        var memoryAsString = lines.First(l => l.StartsWith("TotalVisibleMemorySize", StringComparison.Ordinal))
            .Split("=", StringSplitOptions.RemoveEmptyEntries).Last();

        return (int)(ulong.Parse(memoryAsString, NumberStyles.Integer, CultureInfo.InvariantCulture) / 1024);
    }
}