namespace OpaqueCamp.Launcher.Core
{
    public sealed class JVMMemoryProvider
    {
        public int InitialMemoryAllocation { get; set; } = 1024;
        public int MaxMemoryAllocation { get; set; } = 4096;
    }
}
