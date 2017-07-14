using System.Reactive;
using System.Threading.Tasks;
using Mips_net.Data;
using Mips_net.Device;


namespace Mips_net.Module
{
    public interface IPulseSequenceGeneratorModule
    {
	    Task<Unit> SetSignalTable(MipsSignalTable table);
	    Task<Unit> SetClock(ClockType clockType);
	    Task<Unit> SetTrigger(SignalTableTrigger trigger);
	    Task<Unit> AbortTimeTable();
	    Task<Unit> SetMode(Modes mode);
	    Task<Unit> StartTimeTable();
	    Task<Unit> StopTable();
	    Task<int> GetClockFreuency();
	    Task<Unit> SetTableNumber(int number);
	    Task<int> GetTableNumber();
	    Task<Unit> SetAdvanceTableNumber(State state);
	    Task<State> GetAdvanceTableNumber();
	    Task<Unit> SetChannelValue(int count,string channel,int newValue);
	    Task<int> GetChannelValue(int count, string channel);
	    Task<Unit> SetChannelCount(int count, string channel, int newCount);
	    Task<Unit> SetTableDelay( int delay);
	    Task<Unit> SetSoftwareGeneration(Status value);
	    Task<Unit> EnablesRelpy(Status enables);
	    Task<Status> GetStatusReply();








	}
}