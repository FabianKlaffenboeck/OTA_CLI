using System;
using OTA_CLI.CAN_Bus;

namespace OTA_CLI;

public class OnlineUpdater : UpdaterInterface
{
    public OnlineUpdater(KvaserInterface kvaserInterface)
    {
    }

    public bool Update(int targetDeviceId)
    {
        throw new NotImplementedException();
    }

    public bool Verify()
    {
        throw new NotImplementedException();
    }
}