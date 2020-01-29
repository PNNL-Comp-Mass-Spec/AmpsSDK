using System;

namespace Mips.Device
{
   public class ChannelOutOfRangeException:Exception
    {
	    public ChannelOutOfRangeException(string message)
		    : base(message)
	    {
	    }
	}
}
