using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Mips.Device;

namespace Mips.Module
{
    public interface IArbModule
    {
        Task<string> SetArbMode(int module, ArbMode mode);
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
        Task<Unit> SetWaveform(int module, IEnumerable<int> points);
	    Task<IEnumerable<int>> GetWaveform(int module);
	    Task<Unit> SetWaveformType(int module, ArbWaveForms waveForms);
	    Task<ArbWaveForms> GetWaveformType(int module);
	    Task<Unit> SetBufferLength(int module,int length);
	    Task<int> GetBufferLength(int module);
	    Task<Unit> SetBufferRepeat(int module,int count);
	    Task<int> GetBufferRepeat(int module);
	    Task<Unit> SetAllChannelValue(int module, int value);
	    Task<Unit> SetChannelValue(int module,int channle, int value);
	    Task<Unit> SetChannelRange(int module, int channel, int start, int stop, int range);
    }
}