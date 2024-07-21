using CommandLine;

namespace OTA_CLI;

public class CommandLineOptions
{
    [Option(shortName: 'i', longName: "interface", Required = true,
        HelpText = "The interface for connecting to the target device.", Default = -1)]
    public int Interface { get; set; }

    [Option(shortName: 's', longName: "scann", Required = false,
        HelpText = "Perform a scan on the connected interface for targets.", Default = false)]
    public bool Scan { get; set; }

    [Option(shortName: 'o', longName: "online", Required = false,
        HelpText = "Perform automatic update via the OTA Server.", Default = false)]
    public bool OnlineMode { get; set; }

    [Option(shortName: 'd', longName: "device", Required = false,
        HelpText =
            "If more devices are available on one interface (e.g. one can Bus with multiple targets) then the device id has to be specified.")]
    public int TargetDevice { get; set; }

    [Option(shortName: 'b', longName: "binary", Required = false, HelpText = "Binary to flash.", Default = "")]
    public string Binary { get; set; }
}