using System.Reactive;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Modules
{
    public interface IEsiModule
    {
        Task<Unit> SetPositiveHighVoltage(int volts);
        Task<Unit> SetNegativeHighVoltage(int volts);
        Task<(double Voltage, double Current)> GetPositiveEsi();
        Task<(double Voltage, double Current)> GetNegativeEsi();
    }
}