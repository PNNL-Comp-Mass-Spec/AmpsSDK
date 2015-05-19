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

        private string eventData = string.Empty;

        private int tableName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsBoxSignalTableCommandFormatter"/> class.
        /// </summary>
        public AmpsBoxSignalTableCommandFormatter()
        {
            this.commandFormat = "STBLDAT;{1}{3}{0}{2};";
            this.tableName = 0;
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
         
            Process(rootTable, converter);
            eventData = eventData.Trim(',');

            int count = 0;
            foreach (var str in eventData)
            {
                if (str == '[' || str == ']')
                {
                    count++;
                }
            }

            if (count % 2 == 1)
            {
                eventData = eventData.Remove(eventData.LastIndexOf("]"));
            }

            var iterationData = string.Format("{0}{1}{2}{3}", 0, ":", table.ExecutionData.Iterations, ",");
            
            var startTime = rootTable.ExecutionData.StartTime;
            var stringToReturn = string.Format(this.commandFormat, eventData, startTime + ":[", "]", iterationData);
            return stringToReturn;
        }

        #endregion

        /// <summary>
        /// Process takes a node and converter and seeks to determine a base condition or recurse through the graph building up the nested signal table string.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="converter"></param>
        private void Process(SignalTable node, ITimeUnitConverter<double> converter)
        {
            var waveformTimes = new Queue<double>(node.StartTimes.OrderBy(x => x)); // Generate queue based on start times.
            var children = node.Children.Select(x => ((SignalTable)x).ExecutionData.StartTime).OrderBy(x => x);
            var childrenStartTimes = new Queue<double>(children); // Generate queue based on start times of children.
            

            // Loop that checks between the waveform start times and the children signal table start times. If the waveform time is less, then that branch is selected.
            // If the children start time is less, then the branch to handle that is taken. This may require some degree of recursion.
            while (waveformTimes.Any() || childrenStartTimes.Any())
            {
                var timeBuilder = new StringBuilder();
                if (childrenStartTimes.Any() && waveformTimes.Any())
                {
                    if (waveformTimes.Peek() < childrenStartTimes.Peek())
                    {
                        AppendWaveform(waveformTimes, node, converter, timeBuilder);
                    }
                    else if (childrenStartTimes.Peek() < waveformTimes.Peek())
                    {
                        AppendSignalTable(childrenStartTimes, node, converter, timeBuilder);
                    }
                }
                else if (waveformTimes.Any())
                {
                    AppendWaveform(waveformTimes, node, converter, timeBuilder);
                }

                else if (childrenStartTimes.Any())
                {
                    AppendSignalTable(childrenStartTimes, node, converter, timeBuilder);
                }
            }
           eventData = eventData.Trim(',');
           eventData += "],";

        }

        private void AppendWaveform(Queue<double> waveformTimes, SignalTable node, ITimeUnitConverter<double> converter, StringBuilder timeBuilder)
        {
            var time = waveformTimes.Dequeue();
            int maxTime = 0;

            var signals = node.GetSignals(time).ToList();

            int intTime =
                Convert.ToInt32(
                    converter.ConvertTo(node.ExecutionData.TimeUnits, TimeUnits.Ticks, time));
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
                timeBuilder.AppendFormat(
                    "{0}:{1}:",
                    ap[int.Parse(digitalStepEvent.Channel.ChannelIdentifier)],
                    state);
            }

            string events = timeBuilder.ToString().TrimEnd(':');
            eventData += events;
            eventData += ",";
        }

        private void AppendSignalTable(Queue<double> childrenStartTimes, SignalTable node, ITimeUnitConverter<double> converter, StringBuilder timeBuilder)
        {
            var time = childrenStartTimes.Dequeue();
            var signalTable =
                node.Children.OfType<SignalTable>().FirstOrDefault(x => x.ExecutionData.StartTime == time);

            int intTime =
              Convert.ToInt32(
                  converter.ConvertTo(signalTable.ExecutionData.TimeUnits, TimeUnits.Ticks, time));
            tableName++;
            timeBuilder.AppendFormat("{0}:[{1}:", intTime, tableName);
            eventData += timeBuilder.ToString();
            Process(signalTable, converter);
        }
    }
}