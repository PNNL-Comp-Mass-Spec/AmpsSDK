using System.Reactive;
using System.Threading.Tasks;

namespace Mips.Module
{
    public interface IEsiModule
    {
        Task<Unit> SetEsiVoltage(string channel, int volts);
        Task<int> GetEsiSetpointVoltage(string channel);
        Task<int> GetEsiReadbackVoltage(string channel);
        Task<int> GetEsiReadbackCurrent(string channel);
        Task<int> GetMaximumEsiVoltage(string channel);
    }
}