using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmpsBoxSdk.IO
{
    /// <summary>
    /// Converts 
    /// </summary>
    public class ClockTickConverter
    {
        /// <summary>
        /// Creates a clock converter object
        /// </summary>
        /// <param name="frequency"></param>
        public ClockTickConverter(int frequency)
        {
            ClockFrequency = frequency;
        }

        public int ClockFrequency { get; private set; }

        /// <summary>
        /// Converts the time in MS to ticks.
        /// </summary>
        /// <param name="time">Time in ms</param>
        /// <returns>Ticks</returns>
        public int ConvertToTicks(int time)
        {
            return ClockFrequency * time;
        }
    }
}
