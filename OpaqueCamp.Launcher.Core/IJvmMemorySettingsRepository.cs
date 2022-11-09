namespace OpaqueCamp.Launcher.Core;

public interface IJvmMemorySettingsRepository
{
    int GetInitialMemoryAllocationMegabytes();
    
    void SetInitialMemoryAllocationMegabytes(int value);
    
    int GetMaxMemoryAllocationMegabytes();
    
    void SetMaxMemoryAllocationMegabytes(int value);
}
