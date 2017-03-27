using System.Reactive;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Modules
{
    public interface IRfDriverModule
    {
        Task<Unit> SetFrequency(string address, int frequency);
        Task<int> GetFrequencySetting(string address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="drive">Between 0-255</param>
        /// <returns></returns>
        Task<Unit> SetRfDriveSetting(string address, int drive);

        Task<int> GetRfDriveSetting(string address);

        Task<int> GetRfChannelNumber();
    }
}