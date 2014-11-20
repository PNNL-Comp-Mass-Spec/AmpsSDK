// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsBoxSignalTableDisplayFormatter.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The amps box signal Table display formatter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FalkorSDK.Data;
    using FalkorSDK.Data.Events;
    using FalkorSDK.Data.Signals;
    using FalkorSDK.IO.Signals;

    /// <summary>
    /// TODO The amps box signal Table display formatter.
    /// </summary>
    public class AmpsBoxSignalTableDisplayFormatter : ISignalTableFormatter<SignalTable, double>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Formats a Table into the AMPS box readable time Table.
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <param name="converter">
        /// The converter.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string FormatTable(SignalTable table, ITimeUnitConverter<double> converter)
        {
            IEnumerable<double> times = table.GetTimes();
            times = times.OrderBy(x => x);
            StringBuilder timeBuilder = new StringBuilder();

            TimeTableUnits units = TimeTableUnits.Seconds;

            foreach (double time in times)
            {
                IEnumerable<SignalEvent> signals = table.GetSignals(time);

                timeBuilder.AppendFormat("\t-----------------------------------------------\n");

                foreach (SignalEvent signal in signals)
                {
                    double xTime = converter.ConvertTo(table.TimeUnits, units, signal.Time);

                    AnalogStepEvent output = signal as AnalogStepEvent;
                    if (output != null)
                    {
                        timeBuilder.AppendFormat("\t{0}\t{1}\t{2:F0}\n", xTime, signal.Channel, output.Value);
                    }
                }
            }

            string eventData = timeBuilder.ToString();

            return string.Format(
                "Table: {0}\n\tLength:\t{1}\n\tTime Units:\t{2}\n{3}", 
                table.Name, 
                converter.ConvertTo(table.TimeUnits, units, table.Length), 
                units, 
                eventData);
        }

        #endregion
    }
}