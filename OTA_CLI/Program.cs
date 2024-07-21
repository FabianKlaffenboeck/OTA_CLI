using System;
using System.Text;
using CommandLine;
using Kvaser.CanLib;
using OTA_CLI.CAN_Bus;

namespace OTA_CLI;

class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed<CommandLineOptions>(o =>
        {
            var kvaserInterface = new KvaserInterface(o.Interface, Canlib.canBITRATE_250K);

            if (!kvaserInterface.init())
            {
                Console.WriteLine("sorry, bit the selected device is not available");
                Console.WriteLine("available devices are:");
                foreach (var kInterface in kvaserInterface.Interfaces)
                {
                    Console.WriteLine("Found channel: {0} {1}", kInterface.Item1, kInterface.Item2);
                }
            }


            // scanning for available devices on the can
            if (o.Scan)
            {
                Console.WriteLine("Scanning started, please wait...");
                foreach (var device in kvaserInterface.StartScan())
                {
                    Console.WriteLine(device);
                }

                return;
            }


            // UpdaterInterface updaterInterface;
            // if (o.OnlineMode)
            // {
            //     updaterInterface = new OnlineUpdater(kvaserInterface);
            // }
            // else
            // {
            //     updaterInterface = new FileUpdater(kvaserInterface, o.Binary);
            // }
            //
            // updaterInterface.Update(o.TargetDevice);
            // updaterInterface.Verify();
        });
        Console.WriteLine("Program End");
    }
}