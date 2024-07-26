using CommandLine;
using Kvaser.CanLib;
using OTA_CLI.CAN_Bus;

namespace OTA_CLI;

class Program
{
    static void Main(string[] args)
    {
        CommandLineOptions? options = null;

        Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(o => { options = o; });

        if (options == null)
        {
            Console.WriteLine("Sorry bit something went wrong");
            return;
        }

        var kvaserInterface = new KvaserInterface(options.Interface, Canlib.canBITRATE_250K);

        if (!kvaserInterface.Init())
        {
            Console.WriteLine("sorry, but the selected device is not available");
            Console.WriteLine("available devices are:");
            foreach (var kInterface in kvaserInterface.Interfaces)
            {
                Console.WriteLine("Found channel: {0} {1}", kInterface.Item1, kInterface.Item2);
            }

            return;
        }

        // scanning for available targetDevices on the can
        if (options.Scan)
        {
            Console.WriteLine("Scanning started, please wait...");
            foreach (var device in kvaserInterface.ScanForDevices())
            {
                Console.WriteLine(device);
            }

            return;
        }


        UpdaterInterface updater;
        if (options.OnlineMode)
        {
            updater = new OnlineUpdater(kvaserInterface);
        }
        else
        {
            updater = new FileUpdater(kvaserInterface, options.Binary);
        }

        updater.Update(options.TargetDevice);
        updater.Verify();


        Console.WriteLine("waiting for key");
        Console.ReadKey();
        kvaserInterface.Stop();

        Console.WriteLine("Program End");
    }
}