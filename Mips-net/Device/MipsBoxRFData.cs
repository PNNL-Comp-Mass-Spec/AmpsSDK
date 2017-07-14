using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mips_net.Device
{
    public class MipsBoxRFData
    {
	    public MipsBoxRFData()
	    {
		    Frequency = new ChannelData(500, 5000, 0, 0);
		    DriveLevel = new ChannelData(0, 255, 0, 0);
	    }

	    
	    public ChannelData DriveLevel { get; set; }

	    /// <summary>
	    /// Gets or sets the output voltage.
	    /// </summary>
	    public double Output { get; set; }

	    /// <summary>
	    /// Gets or sets the RF int
	    /// </summary>
	    public ChannelData Frequency { get; set; }

}
}
