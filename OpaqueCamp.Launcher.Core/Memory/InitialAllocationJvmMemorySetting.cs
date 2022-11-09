namespace OpaqueCamp.Launcher.Core.Memory;

class InitialAllocationJvmMemorySetting : JvmMemorySetting
{
    public InitialAllocationJvmMemorySetting(int currentValue) : base(currentValue)
    {
    }

    public override int RecommendedMegabytes => 1024;
}
