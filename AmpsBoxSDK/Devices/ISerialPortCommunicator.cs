using System.IO.Ports;

namespace AmpsBoxSdk.Devices
{
    public interface ISerialPortCommunicator
    {
        SerialPort Port { get; }
        
    }
}