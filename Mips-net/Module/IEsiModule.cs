using System.Reactive;
using System.Threading.Tasks;

namespace Mips_net.Module
{
    public interface IEsiModule
    {
        Task<Unit> SetEsiVoltage(int channel, int volts);
        Task<int> GetEsiSetpointVoltage(int channel);
        Task<int> GetEsiReadbackVoltage(int channel);
        Task<int> GetEsiReadbackCurrent(int channel);
        Task<int> GetMaximumEsiVoltage(int channel);
    }
}