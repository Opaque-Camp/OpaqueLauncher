namespace OpaqueCamp.Launcher.Core.Memory;

public class JvmMemorySettings : IJvmMemorySettings
{
    public JvmMemorySettings(IJvmMemorySettingsStorage memSettingsRepo, ISystemMemoryDetector systemMemoryDetector)
    {
        InitialMemoryAllocation =
            new InitialAllocationJvmMemorySetting(memSettingsRepo.GetInitialMemoryAllocationMegabytes());
        InitialMemoryAllocation.ValueChanged +=
            (_, value) => memSettingsRepo.SetInitialMemoryAllocationMegabytes(value);

        MaximumMemoryAllocation = new MaxAllocationJvmMemorySetting(memSettingsRepo.GetMaxMemoryAllocationMegabytes(),
            systemMemoryDetector);
        MaximumMemoryAllocation.ValueChanged += (_, value) => memSettingsRepo.SetMaxMemoryAllocationMegabytes(value);
    }

    public JvmMemorySetting InitialMemoryAllocation { get; }

    public JvmMemorySetting MaximumMemoryAllocation { get; }
}
