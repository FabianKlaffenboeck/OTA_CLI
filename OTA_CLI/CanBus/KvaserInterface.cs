using System.Collections.Concurrent;
using Kvaser.CanLib;

namespace OTA_CLI.CAN_Bus;

public class KvaserInterface
{
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();

    private ResponsePacket? _responsePacket;

    private List<Tuple<int, string>> _interfaces = [];
    private int _chanelId;
    private int _baudRate;
    private int _handle;
    
    private static int _canTimeout = 1000; // in ms

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

    /*
     * initializes a can device
     */
    public bool Init()
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

        // start the reader thread
        new Thread(() => ReviverLoop(_cts.Token)).Start();

        return true;
    }

    public void Stop()
    {
        Canlib.canStatus status;
        status = Canlib.canBusOff(_handle);
        CheckForError(status, "canBusOff");

        status = Canlib.canClose(_handle);
        CheckForError(status, "canClose");

        _cts.Cancel();
    }

    /*
     * write can message and wait for it to be sent
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

    /*
     * write command and wait for it to be sent and then wait for the corresponding response
     */
    public ResponsePacket? WriteCmdMsgRsp(CommandPacket cmd)
    {
        ResponsePacket? response = null;
        WriteMsg(cmd.MappedMsg);


        // TODO add timeout
        while (_responsePacket == null)
        {
            response = _responsePacket;
        }

        _responsePacket = null;
        return response;
    }

    /*
     * continuously receive new messages
     */
    private void ReviverLoop(CancellationToken token)
    {
        while (true)
        {
            // Check for cancellation request
            if (token.IsCancellationRequested)
            {
                break;
            }

            var data = new byte[8];
            int id;
            int dlc;
            int flags;
            long timestamp;
            Canlib.canStatus status;
            status = Canlib.canRead(_handle, out id, data, out dlc, out flags, out timestamp);
            if (status != Canlib.canStatus.canOK) continue;

            // if Response msg is detested
            if (id == 0x7d2)
            {
                _responsePacket = new ResponsePacket(new CanMsg(id, dlc, data, flags, timestamp));
            }
        }
    }

    /*
     * lists all connected channels
     */
    private void ListChannels()
    {
        Canlib.canStatus stat;

        stat = Canlib.canGetNumberOfChannels(out int numberOfChannels);
        if (CheckForError(stat, "canGetNumberOfChannels"))
        {
            return;
        }

        for (var i = 0; i < numberOfChannels; i++)
        {
            stat = Canlib.canGetChannelData(i, Canlib.canCHANNELDATA_DEVDESCR_ASCII, out object deviceName);
            if (CheckForError(stat, "canGetChannelData"))
            {
                return;
            }

            _interfaces.Add(new Tuple<int, string>(i, deviceName.ToString()));
        }
    }

    /*
     * When called CheckForError will check for and print any error.
     * Return true if an error has occured.
     */
    private bool CheckForError(Canlib.canStatus stat, string cmd)
    {
        if (stat == Canlib.canStatus.canOK) return false;
        Canlib.canGetErrorText(stat, out string buf);
        Console.WriteLine("[{0}] {1}: failed, stat={2}", cmd, buf, (int)stat);
        return true;
    }


    /*
     * scans for target devices
     */
    public List<TargetDevice> ScanForDevices()
    {
        List<TargetDevice> devices = [];

        return devices;
    }
}