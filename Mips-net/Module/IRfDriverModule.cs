using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace Mips_net.Module
{
    public interface IRfDriverModule
    {
        Task<Unit> SetFrequency(string channel, int frequencyInHz);
        Task<Unit> SetLevel(string channel, int peakToPeakVoltage);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="drive">[0-100]</param>
        /// <returns></returns>
        Task<Unit> SetDriveLevel(string channel, int drive);

        Task<int> GetFrequency(string channel);
        Task<double> GetPositiveComponent(string channel);
        Task<double> GetNegativeComponent(string channel);
        Task<double> GetOutputDriveLevelPercent(string channel);
        Task<double> GetPeakToPeakVoltageSetpoint(string channel);
        Task<int> GetChannelPower(string channel);
        Task<IEnumerable<double>> GetParameters();
    }
}