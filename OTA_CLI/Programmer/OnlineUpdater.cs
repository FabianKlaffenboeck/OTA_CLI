using System;
using OTA_CLI.CAN_Bus;

namespace OTA_CLI;

public class OnlineUpdater : UpdaterInterface
{
    private KvaserInterface _kvaserInterface;
    
    public OnlineUpdater(KvaserInterface kvaserInterface)
    {
        _kvaserInterface = kvaserInterface;
    }

    public bool Update(byte targetDeviceId)
    {
        return true;
    }

    public bool Verify()
    {
        return true;
    }
}