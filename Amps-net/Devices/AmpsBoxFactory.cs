using System;
using System.IO.Ports;
using AmpsBoxSdk.Io;
namespace AmpsBoxSdk.Devices
{
    public static class AmpsBoxFactory
    {
        public static IAmpsBox CreateAmpsBox(SerialPort serialPort)
        {
            if (serialPort == null)
            {
                throw new ArgumentNullException(nameof(serialPort));
            }
            return new AmpsBox(new AmpsBoxCommunicator(serialPort));
        }

        /// <summary>
        /// Use FTDI serial number to generate and return an AMPS box.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static IAmpsBox CreateAmpsBox(string serialNumber)
        {
            return new AmpsBox(new FTDIAmpsBoxCommunicator(serialNumber, false));
        }
    }
}