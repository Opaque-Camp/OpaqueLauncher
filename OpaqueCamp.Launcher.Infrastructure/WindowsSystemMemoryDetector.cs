using System.Management;
using System.Runtime.Versioning;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

[SupportedOSPlatform("windows")]
public sealed class WindowsSystemMemoryDetector : ISystemMemoryDetector
{
    public int GetSystemMemoryMegabytes()
    {
        var searcher = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
        var searcherEnumerator = searcher.Get().GetEnumerator();
        searcherEnumerator.MoveNext();
        return (int)(Convert.ToInt64(searcherEnumerator.Current["TotalPhysicalMemory"]) / (1024 * 1024));
    }
}
