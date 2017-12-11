using System.Reactive;
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
