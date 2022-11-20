namespace OpaqueCamp.Launcher.Core.Memory;

internal class InitialAllocationJvmMemorySetting : JvmMemorySetting
{
    public InitialAllocationJvmMemorySetting(int currentValue) : base(currentValue)
    {
    }

    public override int RecommendedMegabytes => 1024;
}
