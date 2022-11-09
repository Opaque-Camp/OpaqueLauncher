namespace OpaqueCamp.Launcher.Core;

class MaxAllocationJvmMemorySetting : JvmMemorySetting
{
    private readonly ISystemMemoryDetector _memoryDetector;

    public MaxAllocationJvmMemorySetting(int currentValue, ISystemMemoryDetector memoryDetector) : base(currentValue)
    {
        _memoryDetector = memoryDetector;
    }

    public override int RecommendedMegabytes => _memoryDetector.GetSystemMemoryMegabytes() / 2;
}
