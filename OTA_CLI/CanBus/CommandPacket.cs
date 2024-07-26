namespace OTA_CLI.CAN_Bus;

public class CommandPacket
{
    private byte _cmd;
    private byte _crc;
    private byte _size;
    private byte _targetId;
    private byte[] _data = new byte[5];

    private CanMsg _mappedMsg = new CanMsg();

    public CommandPacket()
    {
        _mappedMsg.Id = 0x7d1;
        RecompileMsg();
    }

    public CommandPacket(CanMsg msg)
    {
        _mappedMsg.Id = 0x7d1;
        _cmd = msg.Data[0];
        _crc = (byte)(msg.Data[1] & 0b00011111);
        _size = (byte)((msg.Data[1] >> 5) & 0b00000111);
        _targetId = msg.Data[2];

        var dataBytePointer = 0;
        for (var i = 3; i < 8; i++)
        {
            _data[dataBytePointer++] = msg.Data[i];
        }

        RecompileMsg();
    }

    public CommandPacket(byte cmd, byte crc, byte size, byte targetId, byte[] data)
    {
        _mappedMsg.Id = 0x7d1;
        Cmd = cmd;
        Crc = crc;
        Size = size;
        TargetId = targetId;
        Data = data;
        RecompileMsg();
    }

    public byte Cmd
    {
        get => _cmd;
        set
        {
            _cmd = value;
            RecompileMsg();
        }
    }

    public byte Crc
    {
        get => _crc;
        set
        {
            _crc = value;
            RecompileMsg();
        }
    }

    public byte Size
    {
        get => _size;
        set
        {
            _size = value;
            RecompileMsg();
        }
    }

    public byte TargetId
    {
        get => _targetId;
        set
        {
            _targetId = value;
            RecompileMsg();
        }
    }

    public byte[] Data
    {
        get => _data;
        set
        {
            if (value.Length > 5)
            {
                throw new ArgumentNullException(nameof(value));
            }

            for (var i = 0; i < value.Length; i++)
            {
                _data[i] = value[i];
            }

            RecompileMsg();
        }
    }

    public CanMsg MappedMsg => _mappedMsg;

    private void RecompileMsg()
    {
        _mappedMsg.Data[0] = _cmd;
        _mappedMsg.Data[1] = (byte)((_crc & 0b00011111) | ((_size & 0b00000111) << 5));
        _mappedMsg.Data[2] = _targetId;

        var dataBytePointer = 0;
        for (var i = 3; i < 8; i++)
        {
            _mappedMsg.Data[i] = _data[dataBytePointer++];
        }
    }
}