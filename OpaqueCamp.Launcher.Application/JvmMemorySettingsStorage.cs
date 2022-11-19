using OpaqueCamp.Launcher.Application.Properties;
using OpaqueCamp.Launcher.Core.Memory;

namespace OpaqueCamp.Launcher.Application;

public sealed class JvmMemorySettingsStorage : IJvmMemorySettingsStorage
{
    public int GetInitialMemoryAllocationMegabytes() => Settings.Default.JvmInitialMemoryAllocationMegabytes;

    public void SetInitialMemoryAllocationMegabytes(int value)
    {
        Settings.Default.JvmInitialMemoryAllocationMegabytes = value;
        Settings.Default.Save();
    }

    public int GetMaxMemoryAllocationMegabytes() => Settings.Default.JvmMaxMemoryAllocationMegabytes;

    public void SetMaxMemoryAllocationMegabytes(int value)
    {
        Settings.Default.JvmMaxMemoryAllocationMegabytes = value;
        Settings.Default.Save();
    }
}
