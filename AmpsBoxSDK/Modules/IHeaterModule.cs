using System.Reactive;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Modules
{
    public interface IHeaterModule
    {
        Task<Unit> TurnOnHeater();
        Task<Unit> TurnOffHeater();
        Task<Unit> SetTemperatureSetpoint(int temperature);
        Task<int> ReadTemperature();
        Task<Unit> SetPidGain(int gain);

    }
}