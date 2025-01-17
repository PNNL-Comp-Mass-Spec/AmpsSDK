using System;
using System.Reactive;
using System.Threading.Tasks;
using Mips.Io;
using Mips.Module;

namespace Mips.Device
{
	public interface IMipsBox : IStandardModule,IClockGenerationModule,IDcBiasModule,IDelayTrigger, IDcBiasProfileModule,
								IRfDriverModule, IDioModule,IEsiModule,IPulseSequenceGeneratorModule,IMacroModule, ITwaveModule,
								IFrequencySweepModule,IWiFiModule,IEthernetModule,IFAIMSModule, IFilamentModule, IArbModule,
								IArbCompressorModule, IArbConfigurationModule
	{
		string Name { get; }

		IObservable<Unit> TableCompleteOrAborted { get; }

		MipsBoxDeviceData DeviceData { get; }

        IMipsCommunicator Communicator { get; }
    }
}
