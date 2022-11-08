using System.Diagnostics;
using System.Management;

namespace OpaqueCamp.Launcher.Core
{
    public sealed class JVMMemoryProvider
    {
        public int InitialMemoryAllocation { get; set; } = 1024;
        public int MaxMemoryAllocation { get; set; } = 4096;


        
        public void AutoMaxMemory()
        {
            // WMI
            if (OperatingSystem.IsWindows())
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_ComputerSystem");

                foreach (var mobject in searcher.Get())
                {
                    MaxMemoryAllocation = (int)(Convert.ToInt64(mobject["TotalPhysicalMemory"]) / (1024 * 1024) / 2);
                }
            }


            // Needs testing if the launcher will be cross-platform
            if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                var output = "";

                var info = new ProcessStartInfo("free -m");
                info.FileName = "/bin/bash";
                info.Arguments = "-c \"free -m\"";
                info.RedirectStandardOutput = true;

                using(var process = Process.Start(info))
                {
                    output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);
                }

                var lines = output.Split("\n");
                var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                MaxMemoryAllocation = (int)(Int64.Parse(memory[1]) / (1024 * 1024) / 2);
            }
        }
    }
}
