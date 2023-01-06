using CmlLib.Core;
using CmlLib.Core.Auth;
using OpaqueCamp.Launcher.Core;
using OpaqueCamp.Launcher.Core.Memory;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class MinecraftLaunchOptionsProvider : IMinecraftLaunchOptionsProvider
{
    private readonly IJvmMemorySettings _jvmMemorySettings;
    private readonly IServerConfigProvider _serverConfigProvider;
    private readonly IModPackInfoProvider _modPackInfoProvider;
    private readonly ICurrentAccountProvider _currentAccountProvider;

    public MinecraftLaunchOptionsProvider(IJvmMemorySettings jvmMemorySettings,
        IServerConfigProvider serverConfigProvider, IModPackInfoProvider modPackInfoProvider,
        ICurrentAccountProvider currentAccountProvider)
    {
        _jvmMemorySettings = jvmMemorySettings;
        _serverConfigProvider = serverConfigProvider;
        _modPackInfoProvider = modPackInfoProvider;
        _currentAccountProvider = currentAccountProvider;
    }

    public MLaunchOption GetLaunchOptions() =>
        new()
        {
            MinimumRamMb = _jvmMemorySettings.InitialMemoryAllocation.Megabytes,
            MaximumRamMb = _jvmMemorySettings.MaximumMemoryAllocation.Megabytes,
            Session = MSession.GetOfflineSession(GetAccountUsername()),
            ServerIp = _serverConfigProvider.ServerAddress,
            GameLauncherName = _modPackInfoProvider.ModPackName,
            GameLauncherVersion = _modPackInfoProvider.UsedMinecraftVersion.ToString()
        };

    private string GetAccountUsername() =>
        (_currentAccountProvider.CurrentAccount ?? throw new CurrentAccountIsNullException()).Username;
}