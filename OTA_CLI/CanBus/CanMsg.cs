namespace OTA_CLI.CAN_Bus;

public class CanMsg
{
    private int _id = 0;
    private int _dlc = 8;
    private int _flags = 0;
    private byte[] _data = new byte[8];
    private long _timestamp = 0;

    public CanMsg()
    {
    }

    public CanMsg(int id, int dlc, byte[] data, int flags, long timestamp)
    {
        _id = id;
        _dlc = dlc;
        _data = data;
        _flags = flags;
        _timestamp = timestamp;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public int Dlc
    {
        get => _dlc;
        set => _dlc = value;
    }

    public byte[] Data
    {
        get => _data;
        set => _data = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Flags
    {
        get => _flags;
        set => _flags = value;
    }

    public long Timestamp
    {
        get => _timestamp;
        set => _timestamp = value;
    }

    public override string ToString()
    {

        
        
        return string.Concat("Id: ", _id.ToString("X"), " DLC: ", _dlc, " Data: ", BitConverter.ToString(_data), " Flags: ", _flags, " Timestamp: ", _timestamp);
    }
}