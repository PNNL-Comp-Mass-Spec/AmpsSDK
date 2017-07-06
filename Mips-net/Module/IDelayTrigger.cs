using System.Reactive;
using System.Threading.Tasks;
using Mips_net.Device;


namespace Mips_net.Module
{
	public interface IDelayTrigger
	{
		Task<Unit> SetTriggerChannelLevel(string channel,TriggerLevel trigger );
		Task<Unit> SetTriggerDelay(double delayTime);
		Task<double> GetTriggerDelay();
		Task<Unit> SetTriggerPeriod(double delayPeriod);
		Task<double> GetTriggerPeriod();
		Task<Unit> SetTriggerRepeatCount(int count);
		Task<int> GetTriggerRepeatCount();
		Task<Unit> SetTriggerModule(ArbMode mode);
		Task<Unit> EnableDelayTrigger(Status enable);
		Task<string> GetDelayTriggerStatus();



	}
}