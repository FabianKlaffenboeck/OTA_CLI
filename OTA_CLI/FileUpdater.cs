using System;

namespace OTA_CLI;

public class FileUpdater : Updater
{
    public FileUpdater(ProgInterface progInterface, string objBinary)
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