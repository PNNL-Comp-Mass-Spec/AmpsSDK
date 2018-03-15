using System;
using System.Reactive;
using System.Threading.Tasks;
using Mips.Module;

namespace Mips.Device
{
	public interface IMipsBox : IStandardModule,IClockGenerationModule,IDcBiasModule,IDelayTrigger, IDcBiasProfileModule,
								IRfDriverModule, IDioModule,IEsiModule,IPulseSequenceGeneratorModule,IMacroModule, ITwaveModule,
								IFrequencySweepModule,IWiFiModule,IEthernetModule,IFAIMSModule, IFilamentModule, IArbModule,
								IArbCompressorModule, IArbConfigurationModule
	{
		Task GetConfig();
		string Name { get; }
		IObservable<Unit> TableCompleteOrAborted { get; }
		Lazy<MipsBoxDeviceData> DeviceData { get; }
	}
}
