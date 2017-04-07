using System.Reactive;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Modules
{
    public interface IDcBiasModule
    {
        Task<Unit> SetDcBiasVoltage(int channel, int volts);
        Task<int> GetDcBiasSetpoint(int channel);
        Task<int> GetDcBiasReadback(int channel);

        Task<int> GetDcBiasCurrentReadback(int channel);

        Task<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber, int offsetVolts);

        Task<int> GetBoardDcBiasOffsetVoltage(int brdNumber);

        Task<int> GetNumberDcBiasChannels();
    }
}