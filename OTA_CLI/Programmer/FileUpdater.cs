using OTA_CLI.CAN_Bus;

namespace OTA_CLI;

public class FileUpdater : UpdaterInterface
{
    private KvaserInterface _kvaserInterface;
    private string _filePath = "";

    public FileUpdater(KvaserInterface kvaserInterface, string filePath)
    {
        _kvaserInterface = kvaserInterface;
        _filePath = filePath;
    }

    public bool Update(byte targetDeviceId)
    {
        var dataBytes = FileToByteArray(_filePath).ToList();
        var chunkedArray = dataBytes.Chunk(4).ToList();

        byte[] lentBytes = BitConverter.GetBytes(dataBytes.Count);

        // FIXME something went wrong
        if (lentBytes.Length > 4)
        {
            return false;
        }

        var rspStart =
            _kvaserInterface.WriteCmdMsgRsp(new CommandPacket(Commands.FLASH_BEGIN, 0, 4, targetDeviceId, lentBytes));

        byte msc = 0;
        int i = 0;
        foreach (var bytese in chunkedArray)
        {
            Console.WriteLine((i * 100) / chunkedArray.Count);
            var rspWrt = _kvaserInterface.WriteCmdMsgRsp(new CommandPacket(Commands.FLASH_DATA, 0, (byte)bytese.Length,
                targetDeviceId, [msc, bytese[0], bytese[1], bytese[2], bytese[3]]));

            msc = (byte)(msc == 255 ? 0 : msc + 1);
            ++i;
        }

        var rspEnd =
            _kvaserInterface.WriteCmdMsgRsp(new CommandPacket(Commands.FLASH_END, 0, 4, targetDeviceId, lentBytes));

        return true;
    }

    public bool Verify()
    {
        return true;
    }

    private byte[] FileToByteArray(string filePath)
    {
        byte[] dataBytes = System.IO.File.ReadAllBytes(filePath);
        return dataBytes;
    }
}