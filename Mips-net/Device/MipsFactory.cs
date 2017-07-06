
using FalkorSDK.IO.Ports;
using Mips_net.Io;
using RJCP.IO.Ports;

namespace Mips_net.Device
{
   public class MipsFactory
    {
	    public static IMipsBox CreateMipsBox(SerialPortStream serialPort)
	    {
		    return new MipsBox(new MipsCommunicator(serialPort));
	    }
	}
}
