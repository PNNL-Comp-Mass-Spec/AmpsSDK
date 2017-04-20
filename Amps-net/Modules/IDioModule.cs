using System.Reactive;
using System.Threading.Tasks;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    public interface IDioModule
    {
        Task<Unit> SetDigitalState(string channel, bool state);
        Task<Unit> PulseDigitalSignal(string channel);

        Task<bool> GetDigitalState(string channel);
        Task<Unit> SetDigitalDirection(string channel, DigitalDirection digitalDirection);
        Task<DigitalDirection> GetDigitalDirection(string channel);
        Task<int> GetNumberDigitalChannels();
    }
}