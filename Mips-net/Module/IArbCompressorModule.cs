using System.Reactive;
using System.Threading.Tasks;
using Mips.Device;

namespace Mips.Module
{
	public interface IArbCompressorModule
	{
		Task<Unit> SetMultiPassCommand(CompressionCommandTable commnad);
		Task<CompressionCommandTable> GetMultiPassCommand();
		Task<CompressorMode> GetArbCompressorMode();
		Task<Unit> SetArbCompressorMode(CompressorMode mode);
		Task<int> GetArbCompressorOrder();
		Task<Unit> SetArbCompressorOrder(int order);
		Task<double> GetArbTriggerDelay();
		Task<Unit> SetArbTriggerDelay(double delay);
		Task<double> GetArbCompressionTime();
		Task<Unit> SetArbCompressionTime(double time);
		Task<double> GetArbNormalTime();
		Task<Unit> SetArbNormalTime(double time);
		Task<double> GetArbNonCompressionTime();
		Task<Unit> SetArbNonCompressionTime(double time);
		Task<Unit> SetArbTrigger();
		Task<SwitchState> GetArbSwitchState();
		Task<Unit> SetArbSwitchState(SwitchState state);






	}
}