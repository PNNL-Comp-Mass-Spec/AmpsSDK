using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmpsBoxLib
{
    /// <summary>
    /// Holds a list of the signals in the time table provided.
    /// </summary>
    public class AmpsSignalTimeTable
    {
        /// <summary>
        /// Maintains a list of the times
        /// </summary>
        private SortedDictionary<int, ICollection<AmpsSignal>> m_table;
        /// <summary>
        /// Constructor.
        /// </summary>
        public AmpsSignalTimeTable()
        {
            m_table = new SortedDictionary<int, ICollection<AmpsSignal>>();
        }
        /// <summary>
        /// Adds the voltage to the timing table.
        /// </summary>
        /// <param name="time">Time to have the channel changed.</param>
        /// <param name="signal">Signal to add</param>
        public void Add(int time, int channel, int voltage)
        {
            AmpsSignal signal   = new AmpsSignal();
            signal.Channel      = channel;
            signal.Time         = time;
            signal.Voltage      = voltage;
            
            Add(signal);
        }
        /// <summary>
        /// Add a signal to the time table.
        /// </summary>
        /// <param name="time">Time to have the channel changed.</param>
        /// <param name="signal">Signal to add</param>
        public void Add(AmpsSignal signal)
        {
            int time = signal.Time;
            if (time < 0)
            {
                throw new Exception("The time cannot be negative.");
            }

            if (!m_table.ContainsKey(time))
            {
                m_table.Add(time, new List<AmpsSignal>());
                m_table[time].Add(signal);
            }
            else
            {
                ICollection<AmpsSignal> signals = m_table[time];                
                foreach (AmpsSignal tempSignal in signals)
                {
                    if (signal.Channel == tempSignal.Channel)
                    {
                        throw new Exception(string.Format("The signal you are trying to add already exists at that time {0}.", time));
                    }
                }                                 
                m_table[time].Add(signal);
            }
        }
        /// <summary>
        /// Retrieves the list of signals for a specific time slice.
        /// </summary>
        /// <param name="time">Time to select signals for.</param>
        /// <returns>List of signals</returns>
        public IEnumerable<AmpsSignal> GetSignals(int time)
        {
            if (m_table.ContainsKey(time))
            {
                return m_table[time];
            }
            return null;
        }
        /// <summary>
        /// Returns the list of time table events.
        /// </summary>
        /// <returns>List of time table events in microseconds.</returns>
        public ICollection<int> GetTimes()
        {
            List<int> times = new List<int>();            
            foreach (int time in m_table.Keys)
            {
                times.Add(time);
            }
            times.Sort();

            return times;
        }
        /// <summary>
        /// Clears the voltage timing table.
        /// </summary>
        public void Clear()
        {
            m_table.Clear();
        }

        public string Name { get; set; }
        /// <summary>
        /// The total length of a time table.
        /// </summary>
        public int Length { get; set; }
    }

}
