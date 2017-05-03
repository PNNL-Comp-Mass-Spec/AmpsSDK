using System.Reactive;
using System.Threading.Tasks;

namespace Mips.Module
{
	public interface IEthernetModule
	{
		Task<string> GetIP();
		Task<Unit> SetIP(string IP);
		Task<int> GetPortNumber();
		Task<Unit> SetPortNumber(int port);
		Task<string> GetGatewayIP();
		Task<Unit> SetGatewayIP(string ip);



	}
}