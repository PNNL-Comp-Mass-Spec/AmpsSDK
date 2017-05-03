using Mips.Io;
using RJCP.IO.Ports;

namespace Mips.Device
{
   public class MipsFactory
    {
	    public static IMipsBox CreateMipsBox(SerialPortStream serialPort)
	    {
		    return new MipsBox(new MipsCommunicator(serialPort));
	    }
	}
}
