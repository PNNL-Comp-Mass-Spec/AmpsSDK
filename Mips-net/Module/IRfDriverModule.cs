using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace Mips.Module
{
    public interface IRfDriverModule
    {
        Task<Unit> SetFrequency(string channel, double frequencyInHz);
        Task<Unit> SetRfPeakToPeak(string channel,double peakToPeakVoltage);
       
        Task<Unit> SetDriveLevel(string channel, double drive);

        Task<double> GetFrequency(string channel);
        Task<double> GetRFPositive(string channel);
        Task<double> GetRFNegative(string channel);
        Task<double> GetOutputDriveLevelPercent(string channel);
        Task<double> GetPeakToPeakVoltage(string channel);
        Task<double> GetChannelPower(string channel);
        Task<IEnumerable<double>> GetParameters();
    }
}