using System.Reactive;
using System.Threading.Tasks;

namespace Mips_net.Module
{
	public interface IWiFiModule
	{
		Task<string> GetHostName();
		Task<string> GetSSID();
		Task<string> GetWiFiPassword();
		Task<Unit> SetHostName(string name);
		Task<Unit> SetSSID(string id);
		Task<Unit> SetWiFiPassword(string password);
		Task<Unit> EnablesInterface(bool enables);
	}
}