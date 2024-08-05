using OTA_CLI.CAN_Bus;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        // Arrange
        // Act
        // Assert
        Assert.Pass();
    }

    [Test]
    public void ResponsePack_from_CanMsg()
    {
        // Arrange
        byte[] dataBytes = [0xfd, 0xcd, 0xdd, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5];
        CanMsg canMsg = new CanMsg(0x7d2, 5, dataBytes, 0, 0);

        // Act
        ResponsePacket responsePacket = new ResponsePacket(canMsg);

        // Assert
        Assert.That(responsePacket.Cmd, Is.EqualTo(0xfd));
        Assert.That(responsePacket.Size, Is.EqualTo(0x06));
        Assert.That(responsePacket.Crc, Is.EqualTo(0x0d));
        Assert.Pass();
    }
}