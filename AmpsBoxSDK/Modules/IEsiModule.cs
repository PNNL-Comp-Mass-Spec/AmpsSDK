using System.Reactive;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Modules
{
    public interface IEsiModule
    {
        Task<Unit> SetPositiveHighVoltage(int volts);
        Task<Unit> SetNegativeHighVoltage(int volts);
        Task<(double, double)> GetPositiveEsi();
        Task<(double, double)> GetNegativeEsi();
    }
}