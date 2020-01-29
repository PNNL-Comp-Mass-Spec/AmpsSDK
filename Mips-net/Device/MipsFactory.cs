using System.IO.Ports;
using Mips.Io;

namespace Mips.Device
{
   public class MipsFactory
    {
	    public static IMipsBox CreateMipsBox(SerialPort serialPort)
	    {
		    return new MipsBox(new MipsCommunicator(serialPort));
	    }

	    public static IMipsBox CreateMipsBox(string serialNumber)
	    {
		    return new MipsBox(new MipsFtdiCommunicator(serialNumber, false));
	    }
    }
}
