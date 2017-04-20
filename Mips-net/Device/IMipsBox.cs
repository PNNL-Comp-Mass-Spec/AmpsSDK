using System.Threading.Tasks;
using Mips.Module;

namespace Mips.Device
{
	public interface IMipsBox : IStandardModule
	{
		Task<string> GetName();
		Task<string> GetVersion();
		Task<string> GetError();
	}
}
