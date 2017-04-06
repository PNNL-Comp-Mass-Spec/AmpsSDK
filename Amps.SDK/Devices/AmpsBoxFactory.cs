using System;
using System.IO.Ports;
using AmpsBoxSdk.Io;

namespace AmpsBoxSdk.Devices
{
    public static class AmpsBoxFactory
    {
        public static IAmpsBox CreateAmpsBox(SerialPort port)
        {
            return new AmpsBox(new AmpsBoxCommunicator(port));
        }
    }
}