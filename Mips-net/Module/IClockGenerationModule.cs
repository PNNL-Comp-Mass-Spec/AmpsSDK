using System.Reactive;
using System.Threading.Tasks;

namespace Mips_net.Module
{
    public interface IClockGenerationModule
    {
        
        Task<int> GetClockPulseWidth();

        Task<Unit> SetClockPulseWidth(int microseconds);
       
        Task<double> GetClockFrequency();

        Task<Unit> SetClockFrequency(double frequencyInHz);

        
        Task<Unit> ConfigureClockBurst(int numberCycles);
    }
}
