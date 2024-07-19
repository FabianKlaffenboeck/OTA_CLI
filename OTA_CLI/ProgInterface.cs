namespace OTA_CLI;

public class ProgInterface
{
    public ProgInterface(string path)
    {
        return;
    }

    public bool Connect()
    {
        return false;
    }

    public List<TargetDevice> StartScan()
    {
        var devices = new List<TargetDevice>();
        devices.Add(new TargetDevice());
        devices.Add(new TargetDevice());
        devices.Add(new TargetDevice());
        return devices;
    }
    

    public bool Upload(byte[] data, int deviceId)
    {
        return false;
    }
}