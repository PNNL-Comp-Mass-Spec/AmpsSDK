using System.Reactive;
using System.Threading.Tasks;
using Mips.Device;

namespace Mips.Module
{
    public interface IArbModule
    {
        Task<Unit> SetArbMode(int module, ArbMode mode);
        Task<ArbMode> GetArbMode(int module);
        Task<Unit> SetArbFrequency(int module, int frequencyInHz);
        Task<int> GetArbFrequency(int module);
        Task<Unit> SetArbVoltsPeakToPeak(int module, int peakToPeakVolts);
        Task<int> GetArbVoltsPeakToPeak(int module);
        Task<Unit> SetArbOffsetVoltage(int module, int value);

    }
}