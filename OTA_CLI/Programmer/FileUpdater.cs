using System;
using OTA_CLI.CAN_Bus;

namespace OTA_CLI;

public class FileUpdater : UpdaterInterface
{
    public FileUpdater(KvaserInterface kvaserInterface, string objBinary)
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