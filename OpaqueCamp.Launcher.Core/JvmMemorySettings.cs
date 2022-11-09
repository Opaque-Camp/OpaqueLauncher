namespace OpaqueCamp.Launcher.Core;

public class JvmMemorySettings : IJvmMemorySettings
{
    public JvmMemorySettings(IJvmMemorySettingsRepository memSettingsRepo, ISystemMemoryDetector systemMemoryDetector)
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
