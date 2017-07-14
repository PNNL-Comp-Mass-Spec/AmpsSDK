using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace Mips_net.Module
{
    public interface IDcBiasProfileModule
    {
	    Task<Unit> SetDCbiasProfile(int profile, IEnumerable<int> channels);
	    Task<IEnumerable<double>> GetDCbiasProfile(int profile);
	    Task<Unit> OutputWithDCbiasProfile(int profile);
	    Task<Unit> CopiesToDCbiasProfile(int profile);
	    Task<Unit> ToggleProfile(int profile1,int profile2, int time);
	    Task<Unit> StopToggleProfile();

	}
}