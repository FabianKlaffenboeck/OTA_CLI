using System;
using System.Text;
using CommandLine;
using Kvaser.CanLib;

namespace OTA_CLI;

class Program
{
    static void Main(string[] args)
    {

        Canlib.canInitializeLibrary();
        ListChannels();
        
        // CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed<CommandLineOptions>(o =>
        // {
        //     var progInterface = new ProgInterface(o.Interface);
        //
        //     if (o.Scan)
        //     {
        //         Console.WriteLine("Scanning started, please wait...");
        //         foreach (var device in progInterface.StartScan())
        //         {
        //             Console.WriteLine(device);
        //         }
        //
        //         return;
        //     }
        //
        //
        //     Updater updater;
        //
        //     if (o.OnlineMode)
        //     {
        //         updater = new OnlineUpdater(progInterface);
        //     }
        //     else
        //     {
        //         updater = new FileUpdater(progInterface, o.Binary);
        //     }
        //
        //     updater.Update(o.TargetDevice);
        //     updater.Verify();
        // });
        Console.WriteLine("Program End");
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
    
    // ListChannels prints a list of all connected CAN interfaces.
    static public void ListChannels()
    {
        Canlib.canStatus stat;
        // Get number channels
        stat = Canlib.canGetNumberOfChannels(out int number_of_channels);
        if (CheckForError("canGetNumberOfChannels", stat))
            return;
        Console.WriteLine("Found {0} channels", number_of_channels);
        // Loop and print all channels
        for (int i = 0; i < number_of_channels; i++)
        {
            stat = Canlib.canGetChannelData(i, Canlib.canCHANNELDATA_DEVDESCR_ASCII, out object device_name);
            if (CheckForError("canGetChannelData", stat))
                return;
            stat = Canlib.canGetChannelData(i, Canlib.canCHANNELDATA_CHAN_NO_ON_CARD, out object device_channel);
            if (CheckForError("canGetChannelData", stat))
                return;
            Console.WriteLine("Found channel: {0} {1} {2}", i, device_name, ((UInt32)device_channel + 1));
        }
    }
    
}