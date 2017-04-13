using System.Collections;
using System.Reactive;
using System.Threading.Tasks;
using Mips.Device;

namespace Mips.Module
{
    public interface ITwaveModule
    {
        Task<int> GetTWaveFrequency(string channel);
        Task<Unit> SetTWaveFrequency(string channel, int frequency);
        Task<int> GetTWavePulseVoltage(string channel);
        Task<Unit> SetTWavePulseVoltage(string channel, int voltage);
        Task<Unit> SetTWaveGuard1Voltage(string channel, int voltage);
        Task<int> GetTWaveGuard1Voltage(string channel);
        Task<Unit> SetTWaveGuard2Voltage(string channel, int voltage);
        Task<int> GetTWaveGuard2Voltage(string channel);
        Task<BitArray> GetTWaveSequence(string channel);
        Task<Unit> SetTWaveSequence(string channel, BitArray sequence);
        Task<TWaveDirection> GetTWaveDirection(string channel);
        Task<Unit> SetTWaveDirection(string channel, TWaveDirection direction);
        Task<Unit> SetTWaveMultipassControlTable(string table);
        Task<string> GetTWaveMultipassTableString();
        Task<CompressorMode> GetCompressorMode();
        Task<Unit> SetCompressorMode(CompressorMode mode);
        Task<int> GetCompressorOrder();
        Task<Unit> SetCompressorOrder(int order);
        Task<int> GetCompressorTriggerDelay();
        Task<Unit> SetCompressorTriggerDelay(int delayMilliseconds);
        Task<int> GetCompressionTime();
        Task<Unit> SetCompressionTime(int timeMilliseconds);
        Task<int> GetNormalTime();
        Task<Unit> SetNormalTime(int timeMilliseconds);
        Task<int> GetNonCompressTime();
        Task<Unit> SetNonCompressTime(int timemilliSeconds);
        Task<Unit> ForceMultipassTrigger();
        Task<SwitchState> GetSwitchState();
        Task<Unit> SetSwitchState(SwitchState state);
        Task<Unit> SetTWaveToCommonClockMode(bool setToMode);
        Task<Unit> SetTWaveToCompressorMode(bool setToMode);

    }
}