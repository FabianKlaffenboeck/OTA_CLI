using System.Text;
using CommandLine;

namespace OTA_CLI;

class Program
{
    static void Main(string[] args)
    {
        CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed<CommandLineOptions>(o =>
        {
            var progInterface = new ProgInterface(o.Interface);

            if (o.Scan)
            {
                Console.WriteLine("Scanning started, please wait...");
                foreach (var device in progInterface.StartScan())
                {
                    Console.WriteLine(device);
                }

                return;
            }


            Updater updater;

            if (o.OnlineMode)
            {
                updater = new OnlineUpdater(progInterface);
            }
            else
            {
                updater = new FileUpdater(progInterface, o.Binary);
            }

            updater.Update(o.TargetDevice);
            updater.Verify();
        });
        Console.WriteLine("Program End");
    }
}