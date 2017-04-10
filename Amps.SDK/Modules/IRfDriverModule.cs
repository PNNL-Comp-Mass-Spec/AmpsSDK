using System.Reactive;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Modules
{
    public interface IRfDriverModule
    {
        Task<Unit> SetFrequency(int address, int frequency);
        Task<int> GetFrequencySetting(int address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="drive">Between 0-255</param>
        /// <returns></returns>
        Task<Unit> SetRfDriveSetting(int address, int drive);

        Task<int> GetRfDriveSetting(int address);

        Task<int> GetNumberRfChannels();
    }
}