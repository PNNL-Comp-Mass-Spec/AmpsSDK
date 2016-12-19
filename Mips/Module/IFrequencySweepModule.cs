using System.Reactive;
using System.Threading.Tasks;

namespace Mips.Module
{
    public interface IFrequencySweepModule
    {
        Task<Unit> SetStartFrequency(string channel, int frequency);
        Task<int> GetStartFrequency(string channel);
        Task<Unit> SetStopFrequency(string channel, int stopFrequency);
        Task<int> GetStopFrequency(string channel);
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
        Task<string> GetStatus(string channel);
    }
}