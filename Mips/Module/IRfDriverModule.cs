using System;
using System.Reactive;
using System.Threading.Tasks;

namespace Mips.Module
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

        Task<int> GetFrequenc(string channel);
        Task<int> GetPositiveComponent(string channel);
        Task<int> GetNegativeComponent(string channel);
        Task<int> GetOutputDriveLevelPercent(string channel);
        Task<int> GetPeakToPeakVoltageSetpoint(string channel);
        Task<int> GetChannelPower(string channel);
        Task<string> GetParameters();
    }
}