// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsBoxSignalTableCommandFormatter.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The amps box signal Table command formatter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FalkorSDK.Data;
    using FalkorSDK.Data.Events;
    using FalkorSDK.Data.Signals;
    using FalkorSDK.IO.Signals;

    /// <summary>
    /// TODO The amps box signal Table command formatter.
    /// </summary>
    public class AmpsBoxSignalTableCommandFormatter : ISignalTableFormatter<SignalTable, double>
    {
        #region Fields

        /// <summary>
        /// Table command to format
        /// </summary>
        private readonly string commandFormat;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsBoxSignalTableCommandFormatter"/> class.
        /// </summary>
        public AmpsBoxSignalTableCommandFormatter()
        {
            this.commandFormat = "STBLDAT;{1}{3}{0}{2};";
        }

        #endregion

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
            string eventData = string.Empty;
            IEnumerable<double> times = table.GetTimes();
            times = times.OrderBy(x => x);
            int maxTime = 0;

            foreach (double time in times)
            {
                var signals = table.GetSignals(time).ToList();
                var timeBuilder = new StringBuilder();

                int intTime = Convert.ToInt32(converter.ConvertTo(table.TimeUnits, TimeUnits.Ticks, time));
                if (intTime > maxTime)
                {
                    maxTime = intTime;
                }

                timeBuilder.AppendFormat("{0:F0}:", intTime);
                var analogStepEvents = signals.OfType<AnalogStepEvent>();

                foreach (var signal in analogStepEvents)
                {
                    timeBuilder.AppendFormat("{0}:{1:F0}:", signal.Channel, signal.Value);
                }

                char[] ap = Enumerable.Range('A', 'S' - 'A' + 1).Select(i => (char)i).ToArray();
                var digitalStepEvents = signals.OfType<DigitalStepEvent>();
                foreach (var digitalStepEvent in digitalStepEvents)
                {
                    var state = Convert.ToInt32(digitalStepEvent.Value);
					timeBuilder.AppendFormat("{0}:{1}:", ap[digitalStepEvent.Channel], state);
                }

                string events = timeBuilder.ToString().TrimEnd(':');
                eventData += events;
                eventData += ",";
            }

            eventData = eventData.Trim(',');

            var iterationData = "1:" + table.Iterations + ",";
            var stringToReturn = string.Format(this.commandFormat, eventData, "0:[", "]", iterationData);
            return stringToReturn;
        }

        #endregion
    }
}