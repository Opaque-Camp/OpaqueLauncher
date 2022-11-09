namespace OpaqueCamp.Launcher.Core;

class InitialAllocationJvmMemorySetting : JvmMemorySetting
{
    public InitialAllocationJvmMemorySetting(int currentValue) : base(currentValue)
    {
    }

    public override int RecommendedMegabytes => 1024;
}
