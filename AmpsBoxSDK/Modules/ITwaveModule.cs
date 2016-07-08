using System.Collections;
using System.ComponentModel.Composition;
using FalkorSDK.Data.MassSpectrometry;
using FalkorSDK.Data.Signals;
using FalkorSDK.Devices;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface ITwaveModule
    {
        int GetTWaveModuleCount();
        Frequency GetTravellingWaveFrequency(int boardNumber);
        Voltage GetTravellingWavePulseVoltage(int boardNumber);
        void SetTravellingWaveFrequency(int boardNumber, Frequency frequency);
        void SetTravellingWavePulseVoltage(int boardNumber, Voltage voltage);
        Voltage GetGuardOneOutputVoltage(int boardNumber);
        void SetGuardOneOutputVoltage(int boardNumber, Voltage voltage);
        Voltage GetGuardTwoOutputVoltage(int boardNumber);
        void SetGuardTwoOutputVoltage(int boardNumber, Voltage voltage);
        BitArray GetTWaveOutputSequence(int boardNumber);
        void SetTWaveOutputSequence(int boardNumber, BitArray array);
        void SetTWaveOutputDirection(int boardNumber, TWaveOutputDirection outputDirection);
        TWaveOutputDirection GetTWaveOutputDirection(int boardNumber);
        void SetTWaveMultiPassControl(string asciiTable);
        string GetTWaveMultipassTable();
        CompressorMode GetCompressorMode();
        void SetCompressorMode();
        int GetCompressorOrder();
        /// <summary>
        /// Must be a value between [0,127].
        /// </summary>
        /// <param name="order"></param>
        void SetCompressorOrder(int order);

        int GetCompressorTriggerDelay();
        void SetCompressorTriggerDelay(int delayInMilliseconds);
        int GetCompressionTimeMilliseconds();
        void SetCompressionTime(int milliseconds);
        int GetNormalTimeInMilliseconds();
        void SetNormalTime(int milliseconds);
        int GetNonCompressTimeMilliseconds();
        void SetNonCompressTime(int milliseconds);
        void ForceMultipassTrigger();
        SwitchState GetSwitchState();
        void SetSwitchState(SwitchState switchState);
        void SetTWaveToCommonClockMode(bool useCommonClock);
        void SetTWaveCompressorMOde(bool useCompressorMode);
    }
}