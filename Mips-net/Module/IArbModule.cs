using System.Collections.Generic;
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
        Task<int> GetArbOffsetVoltage(int module);
        Task<Unit> SetAuxOutputVoltage(int module, int value);
        Task<int> GetAuxOutputVoltage(int module);
        Task<Unit> StopArb(int module);
        Task<Unit> StartArb(int module);
        Task<Unit> SetTwaveDirection(int module, TWaveDirection direction);
        Task<TWaveDirection> GetTwaveDirection(int module);
        Task<Unit> SetWaveform(int module, IList<int> points);
    }
}