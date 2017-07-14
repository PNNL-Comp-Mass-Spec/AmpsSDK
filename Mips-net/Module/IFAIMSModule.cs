using System.Reactive;
using System.Threading.Tasks;

namespace Mips_net.Module
{
	public interface IFAIMSModule
	{
		Task<Unit> SetPositiveOutput(int slope,int offset);
		Task<Unit> SetNegativeOutput(int slope, int offset);
	}
}