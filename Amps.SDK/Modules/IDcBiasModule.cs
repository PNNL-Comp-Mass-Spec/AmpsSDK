using System.Reactive;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Modules
{
    public interface IDcBiasModule
    {
        Task<Unit> SetDcBiasVoltage(string channel, int volts);
        Task<int> GetDcBiasSetpoint(string channel);
        Task<int> GetDcBiasReadback(string channel);

        Task<int> GetDcBiasCurrentReadback(string channel);

        Task<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber, int offsetVolts);

        Task<int> GetBoardDcBiasOffsetVoltage(int brdNumber);

        Task<int> GetNumberDcBiasChannels();
    }
}