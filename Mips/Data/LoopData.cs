using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mips.Data
{
	[DataContract]
   public class LoopData
    {
	    public LoopData()
	    {
		    LoopCount = 1;
		    LoopToName = string.Empty;
		    DoLoop = false;
	    }

	    public LoopData(int loopCount, string loopToName, bool doLoop)
	    {
		    LoopCount = loopCount;
		    LoopToName = loopToName;
		    DoLoop = doLoop;
	    }
	    [DataMember]
	    public bool DoLoop { get; }
	    [DataMember]
	    public string LoopToName { get; }
	    [DataMember]
	    public int LoopCount { get; }
	}
}
