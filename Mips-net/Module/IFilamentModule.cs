using System.Reactive;
using System.Threading.Tasks;
using Mips_net.Device;


namespace Mips_net.Module
{
	public interface IFilamentModule
	{
		Task<State> GetFilamentEnable(string channel);
		Task<Unit> SetFilamentEnable(string channel, State status);
		Task<double> GetFilamentSetPointCurrent(string channel);
		Task<double> GetFilamentActualCurrent(string channel);
		Task<Unit> SetFilamentCurrent(string channel,int current);
		Task<double> GetFilamentSetPointVoltaget(string channel);
		Task<double> GetFilamentActualVoltage(string channel);
		Task<Unit> SetFilamentSetVoltage(string channel, int volts);
		Task<double> GetFilamentLoadVoltage(string channel);
		Task<double> GetFilamentPower(string channel);
		Task<double> GetFilamentRampRate(string channel);
		Task<Unit> SetFilamentRampRate(string channel,int ramp);
		Task<double> GetCyclingCurrent1(string channel);
		Task<Unit> SetCyclingCurrent1(string channel, int current);
		Task<double> GetCyclingCurrent2(string channel);
		Task<Unit> SetCyclingCurrent2(string channel,int current);
		Task<int> GetCycle(string channel);
		Task<Unit> SetCycle(string channel,int cycle);
		Task<State> GetCycleStatus(string channel);
		Task<Unit> SetCycleStatus(string channel,State status);
		Task<double> GetCurrentToHost(double resistor);
		Task<Unit> SetResistor(string channel, int time);
		Task<double> GetResistor();
		Task<double> GetEmissionCurrent();





	}
}