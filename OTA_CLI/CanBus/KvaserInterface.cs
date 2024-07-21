using Kvaser.CanLib;

namespace OTA_CLI.CAN_Bus;

public class KvaserInterface
{
    private List<Tuple<int, string>> _interfaces = [];
    private int _chanelId;
    private int _baudRate;
    private int _handle;

    private static int _canTimeout = 1000;

    // index and name
    public List<Tuple<int, string>> Interfaces
    {
        get => _interfaces;
        set => _interfaces = value ?? throw new ArgumentNullException(nameof(value));
    }

    public KvaserInterface(int chanelId, int baudRate)
    {
        _chanelId = chanelId;
        _baudRate = baudRate;
    }


    public void Deconstruct()
    {
        Canlib.canStatus status;
        status = Canlib.canBusOff(_handle);
        CheckForError(status, "canBusOff");

        status = Canlib.canClose(_handle);
        CheckForError(status, "canClose");
    }

    /*
     * initializes a can device
     */
    public bool init()
    {
        Canlib.canInitializeLibrary();
        ListChannels();

        if (_interfaces.Find(x => x.Item1 == _chanelId) == null)
        {
            return false;
        }

        Canlib.canStatus status;

        _handle = Canlib.canOpenChannel(_chanelId, Canlib.canOPEN_ACCEPT_VIRTUAL);
        CheckForError((Canlib.canStatus)_handle, "canOpenChannel");

        status = Canlib.canSetBusParams(_handle, _baudRate, 0, 0, 0, 0);
        CheckForError(status, "canSetBusParams");

        status = Canlib.canBusOn(_handle);
        CheckForError(status, "canBusOn");

        
        ReviverLoop();
        return true;
    }

    /*
     * write can message and wait for it to be send
     */
    private void WriteMsg(CanMsg msg)
    {
        Canlib.canStatus status;

        status = Canlib.canWrite(_handle, msg.Id, msg.Data, msg.Dlc, msg.Flags);
        CheckForError(status, "canWrite");

        // Maybe remove later because this is blocking
        status = Canlib.canWriteSync(_handle, _canTimeout);
        CheckForError(status, "canWriteSync");
    }

    private void ReviverLoop()
    {
        while (true)
        {
            byte[] data = new byte[8];
            int id;
            int dlc;
            int flags;
            long timestamp;
            Canlib.canStatus status;
            status = Canlib.canReadWait(_handle, out id, data, out dlc, out flags, out timestamp, 100);
            if (status == Canlib.canStatus.canOK)
            {
                Console.WriteLine(new CanMsg(id, dlc, data, flags, timestamp));
            }
        }
    }

    /*
     * lists all connected channels
     */
    private void ListChannels()
    {
        Canlib.canStatus stat;

        stat = Canlib.canGetNumberOfChannels(out int number_of_channels);
        if (CheckForError(stat, "canGetNumberOfChannels"))
        {
            return;
        }

        for (int i = 0; i < number_of_channels; i++)
        {
            stat = Canlib.canGetChannelData(i, Canlib.canCHANNELDATA_DEVDESCR_ASCII, out object device_name);
            if (CheckForError(stat, "canGetChannelData"))
            {
                return;
            }

            _interfaces.Add(new Tuple<int, string>(i, device_name.ToString()));
        }
    }

    /*
     * When called CheckForError will check for and print any error.
     * Return true if an error has occured.
     */
    private bool CheckForError(Canlib.canStatus stat, string cmd)
    {
        if (stat != Canlib.canStatus.canOK)
        {
            Canlib.canGetErrorText(stat, out string buf);
            Console.WriteLine("[{0}] {1}: failed, stat={2}", cmd, buf, (int)stat);
            return true;
        }

        return false;
    }


    /*
     * scans for target devices
     */
    public List<TargetDevice> StartScan()
    {
        List<TargetDevice> devices = [];

        devices.Add(new TargetDevice(1, "", false));

        return devices;
    }
}