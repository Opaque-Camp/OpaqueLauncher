namespace OpaqueCamp.Launcher.Core.Memory;

public abstract class JvmMemorySetting
{
    private int _megabytes;

    public JvmMemorySetting(int currentValue)
    {
        _megabytes = currentValue;
    }

    public int Megabytes
    {
        get => _megabytes;
        set
        {
            _megabytes = value;
            ValueChanged?.Invoke(this, value);
        }
    }

    public abstract int RecommendedMegabytes { get; }

    public event EventHandler<int>? ValueChanged;

    public void ResetToRecommended()
    {
        Megabytes = RecommendedMegabytes;
    }
}