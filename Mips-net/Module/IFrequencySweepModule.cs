using System.Reactive;
using System.Threading.Tasks;

namespace Mips.Module
{
    public interface IFrequencySweepModule
    {
        Task<Unit> SetSweepStartFrequency(string channel, double frequency);
        Task<double> GetSweepStartFrequency(string channel);
        Task<Unit> SetSweepStopFrequency(string channel, double stopFrequency);
        Task<double> GetSweepStopFrequency(string channel);
        Task<Unit> SetSweepTime(string channel, double timeInSeconds);
        Task<double> GetSweepTime(string channel);
        /// <summary>
        /// 3 starts both
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        Task<Unit> StartSweep(string channel);
        /// <summary>
        /// 3 stops both
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        Task<Unit> StopSweep(string channel);
        Task<string> GetSweepStatus(string channel);
    }
}