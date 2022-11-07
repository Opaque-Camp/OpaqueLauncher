using System.Management;

namespace OpaqueCamp.Launcher.Core
{
    public sealed class JVMMemoryProvider
    {
        public int InitialMemoryAllocation { get; set; } = 1024;
        public int MaxMemoryAllocation { get; set; } = 4096;


        // Fuck this WMI shit. Are there other ways?
        public void AutoMaxMemory()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_ComputerSystem");

            foreach (var mobject in searcher.Get())
            {
                MaxMemoryAllocation = (int)(Convert.ToInt64(mobject["TotalPhysicalMemory"]) / (1024*1024) / 2);
            }
        }
    }
}
