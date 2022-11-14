using System.Net;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Infrastructure;

public sealed class DownloadSpeedupService : IDownloadSpeedupService
{
    public void MakeDownloadsFaster()
    {
        ServicePointManager.DefaultConnectionLimit = 256;
    }
}