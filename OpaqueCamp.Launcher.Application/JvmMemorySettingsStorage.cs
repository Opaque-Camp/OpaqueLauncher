using OpaqueCamp.Launcher.Core;
using OpaqueCamp.Launcher.Core.Memory;

namespace OpaqueCamp.Launcher.Application;

public sealed class JvmMemorySettingsStorage : IJvmMemorySettingsStorage
{
    public int GetInitialMemoryAllocationMegabytes() => Properties.Settings.Default.JvmInitialMemoryAllocationMegabytes;

    public void SetInitialMemoryAllocationMegabytes(int value)
    {
        Properties.Settings.Default.JvmInitialMemoryAllocationMegabytes = value;
    }

    public int GetMaxMemoryAllocationMegabytes() => Properties.Settings.Default.JvmMaxMemoryAllocationMegabytes;

    public void SetMaxMemoryAllocationMegabytes(int value)
    {
        Properties.Settings.Default.JvmMaxMemoryAllocationMegabytes = value;
    }
}
