using System.Collections;
using System.Reactive;
using System.Threading.Tasks;
using Mips_net.Commands;
using Mips_net.Device;


namespace Mips_net.Module
{
    public interface ITwaveModule
    {
        Task<double> GetTWaveFrequency(string channel);
        Task<Unit> SetTWaveFrequency(string channel, int frequency);
        Task<double> GetTWavePulseVoltage(string channel);
        Task<Unit> SetTWavePulseVoltage(string channel, int voltage);
        Task<Unit> SetTWaveGuard1Voltage(string channel, int voltage);
        Task<double> GetTWaveGuard1Voltage(string channel);
        Task<Unit> SetTWaveGuard2Voltage(string channel, int voltage);
        Task<double> GetTWaveGuard2Voltage(string channel);
        Task<BitArray> GetTWaveSequence(string channel);
        Task<Unit> SetTWaveSequence(string channel, BitArray sequence);
        Task<TWaveDirection> GetTWaveDirection(string channel);
        Task<Unit> SetTWaveDirection(string channel, TWaveDirection direction);
        Task<Unit> SetTWaveCompressionCommand(CompressionTable compressionTable);
        Task<string> GetTWaveCompressionCommand();
        Task<StateCommands> GetCompressorMode();
        Task<Unit> SetCompressorMode(StateCommands mode);
        Task<int> GetCompressorOrder();
        Task<Unit> SetCompressorOrder(int order);
        Task<double> GetCompressorTriggerDelay();
        Task<Unit> SetCompressorTriggerDelay(double delayMilliseconds);
        Task<double> GetCompressionTime();
        Task<Unit> SetCompressionTime(int timeMilliseconds);
        Task<double> GetNormalTime();
        Task<Unit> SetNormalTime(int timeMilliseconds);
        Task<double> GetNonCompressTime();
        Task<Unit> SetNonCompressTime(int timemilliSeconds);
        Task<Unit> ForceMultipassTrigger();
        Task<SwitchState> GetSwitchState();
        Task<Unit> SetSwitchState(SwitchState state);
        Task<Unit> SetTWaveToCommonClockMode(bool setToMode);
        Task<Unit> SetTWaveToCompressorMode(bool setToMode);

    }
}