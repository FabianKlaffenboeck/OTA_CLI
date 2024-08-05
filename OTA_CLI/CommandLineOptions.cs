using CommandLine;

namespace OTA_CLI;

/*
 * just config for the commandline argument parser
 */
public class CommandLineOptions
{
    // index of adapter {0,1,2}
    [Option(shortName: 'i', longName: "interface", Required = true,
        HelpText = "The interface for connecting to the target device.", Default = -1)]
    public int Interface { get; set; }

    // scan for available target devices on can
    [Option(shortName: 's', longName: "scann", Required = false,
        HelpText = "Perform a scan on the connected interface for targets.", Default = false)]
    public bool Scan { get; set; }

    // run in online mode (download from OTA Server)
    [Option(shortName: 'o', longName: "online", Required = false,
        HelpText = "Perform automatic update via the OTA Server.", Default = false)]
    public bool OnlineMode { get; set; }

    // targeted device 
    [Option(shortName: 'd', longName: "device", Required = false,
        HelpText =
            "If more devices are available on one interface (e.g. one can Bus with multiple targets) then the device id has to be specified.")]
    public byte TargetDevice { get; set; }

    // if not in online mode, with binary should be flashed
    [Option(shortName: 'b', longName: "binary", Required = false, HelpText = "Binary to flash.", Default = "")]
    public string Binary { get; set; }
}