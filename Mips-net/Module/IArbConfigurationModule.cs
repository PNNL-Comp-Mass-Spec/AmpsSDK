using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Mips.Device;

namespace Mips.Module
{
    public interface IArbConfigurationModule
    {
	    Task<Unit> SetArbClock(Status status);
	    Task<Unit> SetArbCompressor(Status status);
    }
}
