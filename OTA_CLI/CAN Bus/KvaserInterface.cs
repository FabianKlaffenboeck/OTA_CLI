using Kvaser.CanLib;

namespace OTA_CLI.CAN_Bus;

public class KvaserInterface
{
    private List<Tuple<int, string>> _interfaces = [];
    private int _deviceId = -1;

    public List<Tuple<int, string>> Interfaces
    {
        get => _interfaces;
        set => _interfaces = value ?? throw new ArgumentNullException(nameof(value));
    }

    public KvaserInterface(int deviceId)
    {
        _deviceId = deviceId;
    }

    public bool init()
    {
        ListChannels();

        if (_interfaces.Find(x => x.Item1 == _deviceId) == null)
        {
            return false;
        }


        return true;
    }


    private void ListChannels()
    {
        Canlib.canStatus stat;

        stat = Canlib.canGetNumberOfChannels(out int number_of_channels);
        if (CheckForError("canGetNumberOfChannels", stat))
        {
            return;
        }

        for (int i = 0; i < number_of_channels; i++)
        {
            stat = Canlib.canGetChannelData(i, Canlib.canCHANNELDATA_DEVDESCR_ASCII, out object device_name);
            if (CheckForError("canGetChannelData", stat))
            {
                return;
            }

            _interfaces.Add(new Tuple<int, string>(i, device_name.ToString()));
        }
    }

    // When called CheckForError will check for and print any error.
    // Return true if an error has occured.
    static public bool CheckForError(string cmd, Canlib.canStatus stat)
    {
        if (stat != Canlib.canStatus.canOK)
        {
            Canlib.canGetErrorText(stat, out string buf);
            Console.WriteLine("[{0}] {1}: failed, stat={2}", cmd, buf, (int)stat);
            return true;
        }

        return false;
    }

    public List<TargetDevice> StartScan()
    {
        List<TargetDevice> devices = [];

        devices.Add(new TargetDevice(1, "", false));

        return devices;
    }
}