using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mips_net.Device
{
   public class ChannelOutOfRangeException:Exception
    {
	    public ChannelOutOfRangeException(string message)
		    : base(message)
	    {
	    }
	}
}
