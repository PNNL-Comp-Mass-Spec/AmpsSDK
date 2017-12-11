using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Mips.Device;

namespace Mips.Module
{
    public interface IArbModule
    {
        Task<Unit> SetArbMode(string module, ArbMode mode);
        Task<ArbMode> GetArbMode(string module);
        Task<Unit> SetArbFrequency(string module, double frequencyInHz);
        Task<double> GetArbFrequency(string module);
        Task<Unit> SetArbVoltsPeakToPeak(string module, double peakToPeakVolts);
        Task<double> GetArbVoltsPeakToPeak(string module);
        Task<Unit> SetArbOffsetVoltage(string module, double value);
        Task<double> GetArbOffsetVoltage(string module);
        Task<Unit> SetAuxOutputVoltage(string module, double value);
        Task<double> GetAuxOutputVoltage(string module);
        Task<Unit> StopArb(string module);
        Task<Unit> StartArb(string module);
        Task<Unit> SetTwaveDirection(string module, TWaveDirection direction);
        Task<TWaveDirection> GetTwaveDirection(string module);
        Task<Unit> SetWaveform(string module, IEnumerable<int> points);
	    Task<IEnumerable<int>> GetWaveform(string module);
	    Task<Unit> SetWaveformType(string module, ArbWaveForms waveForms);
	    Task<ArbWaveForms> GetWaveformType(string module);
	    Task<Unit> SetBufferLength(string module, int length);
	    Task<int> GetBufferLength(string module);
	    Task<Unit> SetBufferRepeat(string module, int count);
	    Task<int> GetBufferRepeat(string module);
	    Task<Unit> SetAllChannelValue(string module, int value);
	    Task<Unit> SetChannelValue(string module, string channle, int value);
	    Task<Unit> SetChannelRange(string module, string channel, int start, int stop, int range);
    }
}