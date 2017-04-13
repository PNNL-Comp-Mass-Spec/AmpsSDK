using System;
using System.IO.Ports;
using AmpsBoxSdk.Io;
using RJCP.IO.Ports;

namespace AmpsBoxSdk.Devices
{
    public static class AmpsBoxFactory
    {
        public static IAmpsBox CreateAmpsBox(SerialPortStream serialPort)
        {
            return new AmpsBox(new AmpsBoxCommunicator(serialPort));
        }
    }
}