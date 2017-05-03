using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mips.Device;

namespace Mips.Device
{

   public class MipsBoxDeviceData
    {
	    public MipsBoxDeviceData(uint analogChannels, uint rfChannels, uint digitalChannels)
	    {
		    HvData = new Dictionary<uint, ChannelData>();
		    RfData = new Dictionary<uint, MipsBoxRFData>();
		    DioChannels = new Dictionary<uint, string>();

		    this.NumberHvChannels = analogChannels;
		    this.NumberRfChannels = rfChannels;
		    this.NumberDigitalChannels = digitalChannels;

		    int start = Convert.ToInt32('A');
		    int end = start + (int)NumberDigitalChannels;
		    char[] ap = Enumerable.Range(start, end - start).Select(i => (char)i).ToArray();
		    for (int i = 0; i < ap.Length; i++)
		    {
			    DioChannels.Add((uint)i, ap[i].ToString());
		    }

		}
	    public static MipsBoxDeviceData Empty { get; } = new MipsBoxDeviceData(0, 0, 0);
	    public uint NumberHvChannels { get; }
	    public uint NumberRfChannels { get; }

	    public uint NumberDigitalChannels { get; }
	    private Dictionary<uint, ChannelData> HvData { get; set; }

	   
	    private Dictionary<uint, MipsBoxRFData> RfData { get; set; }

	    private Dictionary<uint, string> DioChannels { get; set; }

	    
	
	    public void Clear()
	    {
		    RfData.Clear();
		    HvData.Clear();
	    }

	   
	    public ChannelData GetHvData(uint channel)
	    {
		    if (channel > NumberHvChannels)
		    {
			    throw new ChannelOutOfRangeException("The RF channel requested is not supported by the device.");
		    }

		    return HvData[channel];
	    }

	    
	    public MipsBoxRFData GetRfData(uint channel)
	    {
		    if (channel > NumberHvChannels)
		    {
			    throw new ChannelOutOfRangeException("The RF channel requested is not supported by the device.");
		    }

		    return RfData[channel];
	    }

	    public string GetDioChannel(uint channel)
	    {
		    if (channel > NumberDigitalChannels)
		    {
			    throw new ChannelOutOfRangeException("The RF channel requested is not supported by the device.");
		    }

		    return DioChannels[channel];
	    }

}
}
