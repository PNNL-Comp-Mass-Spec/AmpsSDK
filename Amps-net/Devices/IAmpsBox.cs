using System;
using System.Reactive;
using System.Threading.Tasks;
using AmpsBoxSdk.Modules;

namespace AmpsBoxSdk.Devices
{
    public interface IAmpsBox : IStandardModule, IPulseSequenceGeneratorModule, IDcBiasModule, IDioModule, IRfDriverModule, IEsiModule, IHeaterModule
    {
        Task<AmpsBoxDeviceData> GetAmpsConfigurationAsync();
        string Name { get; }

        IObservable<Unit> TableCompleteOrAborted { get; }

        IObservable<Unit> ModeReady { get; }
    }
}