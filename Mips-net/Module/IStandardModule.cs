using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Mips.Commands;
using Mips.Device;

namespace Mips.Module
{
    public interface IStandardModule
    {
	    Task<string> GetName();
	    Task<string> GetVersion();
	    Task<ErrorCode> GetError();
	    Task<string> SetName(string name);
	    Task<string> About();
	    Task<Unit> RevisionLevel(string board, int mudule);
	    Task<Unit> Reset();
	    Task<Unit> Save();
	    Task<int> GetChannel(Modules module);
	    Task<Unit> Mute(State mute);
	    Task<Unit> Echo(Status echo);
	    Task<Unit> TriggerOut(TriggerValue triggervalue);
		Task<Unit> Delay(double delay);
	    Task<IEnumerable<string>> GetCommand();
		Task<Status> GetAnalogInputStatus();
		Task<Unit> SetAnalogInputStatus(Status status );
		Task<IEnumerable<string>> Threads();
		Task<Unit> SetThreadControl(string threadName, string value);
		Task<Unit> SetADCAddress(int board, double address);
		Task<string> ReadADC(int channel);
	    Task<string> ReadADC2(int channel);
		Task<Unit> SetIOTableMode(bool enable);
		Task<int> ReadADCChannel(int channel);
		Task<Unit> LEDOverride(bool LEDValue);
		Task<Unit> LEDColor(int color);
		Task<Unit> DisplayOff(Status status);


	}
}