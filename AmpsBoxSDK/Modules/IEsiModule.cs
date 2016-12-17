using System;
using System.ComponentModel.Composition;
using System.Reactive;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Modules
{
    public interface IEsiModule
    {
        Task<Unit> SetPositiveHighVoltage(int volts);
        Task<Unit> SetNegativeHighVoltage(int volts);
        Task<Tuple<double, double>> GetPositiveEsi();
        Task<Tuple<double, double>> GetNegativeEsi();
    }
}