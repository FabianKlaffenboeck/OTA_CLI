namespace OTA_CLI.CAN_Bus;

public class ResponsePacket
{
    private byte _cmd;
    private byte _crc;
    private byte _size;
    private byte _senderId;
    private byte[] _data = new byte[5];

    private CanMsg _mappedMsg = new CanMsg();

    public ResponsePacket()
    {
        _mappedMsg.Id = 0x7d2;
    }

    public ResponsePacket(CanMsg msg)
    {
        _mappedMsg.Id = 0x7d1;
        _cmd = msg.Data[0];
        _crc = (byte)(msg.Data[1] & 0b00011111);
        _size = (byte)((msg.Data[1] >> 5) & 0b00000111);
        _senderId = msg.Data[2];

        var dataBytePointer = 0;
        for (var i = 3; i < 8; i++)
        {
            _data[dataBytePointer++] = msg.Data[i];
        }
    }

    public ResponsePacket(byte cmd, byte crc, byte size, byte senderId, byte[] data)
    {
        _mappedMsg.Id = 0x7d1;
        _cmd = cmd;
        _crc = crc;
        _size = size;
        _senderId = senderId;
        _data = data;
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

    public byte SenderId
    {
        get => _senderId;
        set
        {
            _senderId = value;
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

            _data = value ?? throw new ArgumentNullException(nameof(value));
            RecompileMsg();
        }
    }

    public CanMsg MappedMsg => _mappedMsg;

    private void RecompileMsg()
    {
        _mappedMsg.Data[0] = _cmd;
        _mappedMsg.Data[1] = (byte)((_crc & 0b00011111) | ((_size & 0b00000111) << 5));
        _mappedMsg.Data[2] = _senderId;

        var dataBytePointer = 0;
        for (var i = 3; i < 8; i++)
        {
            _mappedMsg.Data[i] = _data[dataBytePointer++];
        }
    }
}