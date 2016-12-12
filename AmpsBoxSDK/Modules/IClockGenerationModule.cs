using System.ComponentModel.Composition;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IClockGenerationModule
    {
        int GetClockPulseWidth();
        void SetClockPulseWidth();
        int GetClockintHz();
        void SetClockintHz(int hz);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfCycles">-1 indicates forever</param>
        void ClockBurst(int numberOfCycles);
    }
}