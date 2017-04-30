using System.Threading.Tasks;
using AmpsBoxSdk.Modules;

namespace AmpsBoxSdk.Devices
{
    public interface IAmpsBox : IStandardModule, IPulseSequenceGeneratorModule, IDcBiasModule, IDioModule, IRfDriverModule, IEsiModule, IHeaterModule
    {
        Task<AmpsBoxDeviceData> GetAmpsConfigurationAsync();
        string Name { get; }
    }
}