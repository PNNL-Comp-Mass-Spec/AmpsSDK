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
            var childrenCount = table.ChildNodes.Count();
            var rootTable = table.Root as SignalTable;
            var listOfNodes = rootTable.BreadthFirstCollection;

            string eventData = string.Empty;

            var depthDictionary = new Dictionary<int, List<SignalTable>>();

            foreach (var node in listOfNodes)
            {
                if (!depthDictionary.ContainsKey(node.Depth))
                {
                    depthDictionary.Add(node.Depth, new List<SignalTable>() { (SignalTable)node });
                }
                else
                {
                    depthDictionary[node.Depth].Add((SignalTable)node);
                }
            }

            foreach (var key in depthDictionary.Keys)
            {
                var tableNodes = depthDictionary[key];
                
            }

            List<double> times = rootTable.StartTimes.ToList();

            times = times.OrderBy(x => x).ToList();

            int maxTime = 0;

            foreach (double time in times)
            {
                var signals = table.GetSignals(time).ToList();

                var timeBuilder = new StringBuilder();

                int intTime = Convert.ToInt32(converter.ConvertTo(table.ExecutionData.TimeUnits, TimeUnits.Ticks, time));
                if (intTime > maxTime)
                {
                    maxTime = intTime;
                }

                timeBuilder.AppendFormat("{0:F0}:", intTime);
                var analogStepEvents = signals.OfType<AnalogStepElement>();

                foreach (var signal in analogStepEvents)
                {
                    timeBuilder.AppendFormat("{0}:{1:F0}:", signal.Channel, signal.Value);
                }

                char[] ap = Enumerable.Range('A', 'S' - 'A' + 1).Select(i => (char)i).ToArray();
                var digitalStepEvents = signals.OfType<DigitalStepElement>();
                foreach (var digitalStepEvent in digitalStepEvents)
                {
                    var state = Convert.ToInt32(digitalStepEvent.Value);
                    timeBuilder.AppendFormat("{0}:{1}:", ap[int.Parse(digitalStepEvent.Channel.ChannelIdentifier)], state);
                }

                string events = timeBuilder.ToString().TrimEnd(':');
                eventData += events;
                eventData += ",";
            }

            eventData = eventData.Trim(',');

            var iterationData = string.Format("{0}{1}{2}{3}", 1, ":", table.ExecutionData.Iterations, ",");
          
            var startTime = rootTable.ExecutionData.StartTime;
            var stringToReturn = string.Format(this.commandFormat, eventData, startTime + ":[", "]", iterationData);
            return stringToReturn;
        }

        #endregion
    }
}