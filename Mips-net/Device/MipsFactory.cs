using System.IO.Ports;
using Mips_net.Io;

namespace Mips_net.Device
{
   public class MipsFactory
    {
	    public static IMipsBox CreateMipsBox(SerialPort serialPort)
	    {
		    return new MipsBox(new MipsCommunicator(serialPort));
	    }
	}
}
