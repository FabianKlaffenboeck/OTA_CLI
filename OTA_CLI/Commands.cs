namespace OTA_CLI;

public class Commands
{
    public static readonly byte LIST_DEVICES = 0x01;
    public static readonly byte FLASH_BEGIN = 0x02;
    public static readonly byte FLASH_DATA = 0x03;
    public static readonly byte FLASH_END = 0x04;
    public static readonly byte FLASH_DWL_BEGIN = 0x05;
    public static readonly byte FLASH_DWL_DATA = 0x06;
    public static readonly byte FLASH_DWL_END = 0x07;
}