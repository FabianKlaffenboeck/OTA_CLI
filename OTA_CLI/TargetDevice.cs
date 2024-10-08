namespace OTA_CLI;

public class TargetDevice
{
    private int? id;
    private string? currentVersion;
    private bool? newVersionAvailable;

    public TargetDevice(int? id, string? currentVersion, bool? newVersionAvailable)
    {
        this.id = id;
        this.currentVersion = currentVersion;
        this.newVersionAvailable = newVersionAvailable;
    }

    public int? Id
    {
        get => id;
        set => id = value;
    }

    public string? CurrentVersion
    {
        get => currentVersion;
        set => currentVersion = value;
    }

    public bool? NewVersionAvailable
    {
        get => newVersionAvailable;
        set => newVersionAvailable = value;
    }

    public override string ToString()
    {
        return string.Concat("Id: ", id, " ", "CurrentVersion: ", currentVersion, " ", "UpdateAvailable: ",
            newVersionAvailable);
    }
}