using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

public sealed class JvmMemorySettingsRepository : IJvmMemorySettingsRepository
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
