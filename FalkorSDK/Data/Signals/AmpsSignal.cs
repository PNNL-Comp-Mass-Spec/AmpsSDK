using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmpsBoxLib
{
    /// <summary>
    /// Encapsulates a signal (time vs. voltage) for a channel.
    /// </summary>
    public class AmpsSignal
    {
        /// <summary>
        /// Gets or sets the time in whole microseconds
        /// </summary>
        public int Time { get; set; }
        /// <summary>
        /// Gets or sets the voltage value.
        /// </summary>
        public int Voltage { get; set; }
        /// <summary>
        /// Gets or sets the channel to address the voltage on.
        /// </summary>
        public int Channel { get; set; }
        /// <summary>
        /// Gets or sets the name of the signal
        /// </summary>
        public string Name { get; set; }
    }
}
