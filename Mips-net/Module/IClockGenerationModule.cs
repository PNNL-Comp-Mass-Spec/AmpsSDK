using System.Reactive;
using System.Threading.Tasks;

namespace Mips_net.Module
{
    public interface IClockGenerationModule
    {
        
        Task<int> GetClockPulseWidth();

        Task<Unit> SetClockPulseWidth(int microseconds);
       
        Task<int> GetClockFrequency();

        Task<Unit> SetClockFrequency(int frequencyInHz);

        
        Task<Unit> ConfigureClockBurst(int numberCycles);
    }
}
