namespace OpaqueCamp.Launcher.Core.Memory;

public interface IJvmMemorySettingsStorage
{
    int GetInitialMemoryAllocationMegabytes();
    
    void SetInitialMemoryAllocationMegabytes(int value);
    
    int GetMaxMemoryAllocationMegabytes();
    
    void SetMaxMemoryAllocationMegabytes(int value);
}
