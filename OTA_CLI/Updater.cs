namespace OTA_CLI;

public interface Updater
{
    public bool Update(int targetDeviceId);
    public bool Verify();
}