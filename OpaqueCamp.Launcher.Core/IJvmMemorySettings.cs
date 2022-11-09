namespace OpaqueCamp.Launcher.Core;

public interface IJvmMemorySettings
{
    JvmMemorySetting InitialMemoryAllocation { get; }

    JvmMemorySetting MaximumMemoryAllocation { get; }
}
