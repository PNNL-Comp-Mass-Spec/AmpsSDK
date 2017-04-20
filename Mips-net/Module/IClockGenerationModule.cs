using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Mips.Module
{
    public interface IClockGenerationModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Pulse width in microseconds.</returns>
        Task<int> GetClockPulseWidth();

        Task<Unit> SetClockPulseWidth(int microseconds);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Clock frquency in Hz</returns>
        Task<int> GetClockFrequency();

        Task<Unit> SetClockFrequency(int frequencyInHz);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberCycles">-1 = forever.</param>
        /// <returns></returns>
        Task<Unit> ConfigureClockBurst(int numberCycles);
    }
}
