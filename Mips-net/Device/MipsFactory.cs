using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
