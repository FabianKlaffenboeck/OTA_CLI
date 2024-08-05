namespace OTA_CLI;

public interface UpdaterInterface
{
    public bool Update(byte targetDeviceId);
    public bool Verify();
}