namespace OTA_CLI.CAN_Bus;

public class CanMsg
{
    private int _id;
    private int _dlc;
    private int _flag;
    private byte[] _data;

    public CanMsg(int id, int dlc, byte[] data, int flag)
    {
        _id = id;
        _dlc = dlc;
        _data = data;
        _flag = flag;
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

    public int Flag
    {
        get => _flag;
        set => _flag = value;
    }

    public override string ToString()
    {
        return string.Concat("Id: ", _id, "DLC: ", _dlc, "Flag: ", _flag, "Data: ", _data);
    }
}