namespace OTA_CLI;

public interface UpdaterInterface
{
    public bool Update(int targetDeviceId);
    public bool Verify();
}