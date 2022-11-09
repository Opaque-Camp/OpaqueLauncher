namespace OpaqueCamp.Launcher.Core.Memory;

public interface IJvmMemorySettings
{
    JvmMemorySetting InitialMemoryAllocation { get; }

    JvmMemorySetting MaximumMemoryAllocation { get; }
}
