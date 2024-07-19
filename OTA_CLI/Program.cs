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
                ScanHandler(progInterface);
                return;
            }

            UploadHandler(o.Binary, o.TargetDevice, progInterface);
        });
        Console.WriteLine("Program End");
    }

    private static void ScanHandler(ProgInterface progInterface)
    {
        Console.WriteLine("Scanning started, please wait...");
        var devices = progInterface.StartScan();

        foreach (var device in devices)
        {
            Console.WriteLine(device);
        }
    }

    private static void UploadHandler(string pathToBin, int targetDeviceId, ProgInterface progInterface)
    {
        if (!File.Exists(pathToBin))
        {
            Console.WriteLine("File does not exist or path is not valid!");
            return;
        }

        var fileBytes = File.ReadAllBytes(pathToBin);

        progInterface.Upload(fileBytes, targetDeviceId);
    }
}