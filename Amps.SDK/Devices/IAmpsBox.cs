using AmpsBoxSdk.Modules;

namespace AmpsBoxSdk.Devices
{
    public interface IAmpsBox : IStandardModule, IPulseSequenceGeneratorModule, IDcBiasModule, IDioModule, IRfDriverModule, IEsiModule, IHeaterModule
    {
        string GetConfig();
        AmpsBoxDeviceData DeviceData { get; }
    }
}