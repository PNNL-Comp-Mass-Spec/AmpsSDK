// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalTimeTableReaderPlain.cs" company="">
//   
// </copyright>
// <summary>
//   Reads a plain text time Table for voltage timing diagrams.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using FalkorSDK.Channel;
    using FalkorSDK.Data;
    using FalkorSDK.Data.Events;
    using FalkorSDK.Data.Signals;
    using FalkorSDK.Devices;

    /// <summary>
    /// Reads a plain text time Table for voltage timing diagrams.
    /// </summary>
    public class SignalTimeTableReaderPlain : ISignalTableReader<SignalTable>
    {
        #region Constants

        /// <summary>
        /// Default time to delay after the last entry in the time Table.  This is if the user
        /// does not specify a final end time.
        /// </summary>
        /// TODO: Use or remove this.
        private const int DefaultTableEndTime = 100;

        #endregion

        #region Static Fields

        /// <summary>
        /// Total tables in file
        /// </summary>
        private static int tableCount;

        #endregion

        #region Fields

        /// <summary>
        /// TODO The table.
        /// </summary>
        private readonly SignalTable table = new SignalTable();

        /// <summary>
        /// TODO The max time.
        /// </summary>
        private double maxTime;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Reads a time Table from the path.
        /// </summary>
        /// <param name="path">
        /// Path to read Table file from.
        /// </param>
        /// <exception cref="TableNameNotSpecified">
        /// Thrown if the file does not include a name
        /// </exception>
        /// <exception cref="TableLengthNotSpecified">
        /// Thrown if the file does not include a time length
        /// </exception>
        /// <returns>
        /// The <see cref="SignalTable"/>.
        /// </returns>
        public SignalTable Read(string path)
        {
            this.table.Name = "testX" + tableCount;
            tableCount++;
            var lines = File.ReadAllLines(path);
            const string Delimiter = "\t";

            string[] delimiters = { Delimiter };
            this.maxTime = 0;

            this.table.Length = 0;
            this.table.Name = string.Empty;


            foreach (var data in lines.Select(temp => temp.ToLower().Trim()).Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#")).Select(line => line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)).Where(data => data.Length >= 2))
            {
                if (data.Length == 2)
                {
                    var key = data[0].ToLower().Trim().Replace(" ", string.Empty);
                    var value = data[1].ToLower().Trim().Replace(" ", string.Empty);
                    this.ParseAndSetKeyValuePair(key, value);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(this.table.Name))
                {
                    throw new TableNameNotSpecified(
                        "The timing Table file must include a name.  It must also be before the timing Table data.  Name" + Delimiter + "value");
                }

                this.ParseAndAddAnalogStepEvent(data);
            }

            // Specify everything in microseconds now...
            // TODO: This definitely needs to be fixed.  It overrides the format that is provided in the file being read.
            this.table.TimeUnits = TimeTableUnits.Microseconds;
            return this.table;
        }

        public async Task<IEnumerable<SignalTable>> ReadMultipleTablesAsync(string path)
        {
            var tablelines = new List<List<string>>();
            var buildTables = new List<SignalTable>();
           List<string> delimiters = new List<string>(){ SignalTimeTableReaderPlainAsync.Delimiter };
            var tableBuilder = new SignalTimeTableReaderPlainAsync();
            using (var reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var currentLine = line.ToLower().Trim();
                    switch (currentLine)
                    {
                        case "tablestart":
                            tablelines = new List<List<string>>();
                            tableBuilder = new SignalTimeTableReaderPlainAsync();
                            break;
                        case "tableend":
                            var parsedTable = await tableBuilder.ParseTableAsync(tablelines);
                            buildTables.Add(parsedTable);
                            break;
                        default:
                            tablelines.Add(new List<string>(currentLine.Split(delimiters.ToArray(), StringSplitOptions.RemoveEmptyEntries)));
                            break;
                    }
                }
            }

            return buildTables;
        }

        #endregion

        #region Methods

        /// <summary>
        /// TODO The add analog step event.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        private void ParseAndAddAnalogStepEvent(IList<string> data)
        {
            double time;
            int channel;
            double voltage;
            bool isTimeOk;
            bool isChannelOk;
            bool isVoltageOk;

            switch (data.Count)
            {
                case 4:

                    // TODO: Fix this
                    isTimeOk = double.TryParse(data[0], out time);
                    isChannelOk = int.TryParse(data[2], out channel);
                    isVoltageOk = double.TryParse(data[3], out voltage);

                    if (!isTimeOk || !isChannelOk || !isVoltageOk)
                    {
                        return;
                    }

                    break;
                case 3:
                    isTimeOk = double.TryParse(data[0], out time);
                    isChannelOk = int.TryParse(data[1], out channel);
                    isVoltageOk = double.TryParse(data[2], out voltage);

                    if (!isTimeOk || !isChannelOk || !isVoltageOk)
                    {
                        return;
                    }

                    break;
                default:
                    return;
            }

            var signal = new AOChannel(channel, channel.ToString(), "", false);
            var analogEvent = new AnalogStepEvent(signal.Address, time, voltage);

            this.table.Add(analogEvent);
            this.maxTime = Math.Max(time, this.maxTime);
        }

        /// <summary>
        /// Parses a key value pair and updates the table info.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        private void ParseAndSetKeyValuePair(string key, string value)
        {
            switch (key)
            {
                case "name":
                    this.table.Name = value;
                    break;
                case "length":
                    double length;
                    if (double.TryParse(value, out length))
                    {
                        this.table.Length = length;
                    }

                    break;
                case "comment":
                    this.table.Comment += value;
                    break;
                case "units":
                    this.ParseAndSetTimeUnits(value);
                    break;
            }
        }

        /// <summary>
        /// TODO The parse and set time units.
        /// </summary>
        /// <param name="unitType">
        /// TODO The unit type.
        /// </param>
        /// <exception cref="InvalidDataException">
        /// The only current acceptable units are Milliseconds, Microseconds, and Seconds.  Any other value with raise this error.
        /// </exception>
        private void ParseAndSetTimeUnits(string unitType)
        {
            unitType = unitType.ToLower();
            switch (unitType)
            {
                case "milliseconds":
                    this.table.TimeUnits = TimeTableUnits.Milliseconds;
                    break;
                case "seconds":
                    this.table.TimeUnits = TimeTableUnits.Seconds;
                    break;
                case "microseconds":
                    this.table.TimeUnits = TimeTableUnits.Microseconds;
                    break;
                default:
                    throw new InvalidDataException("The time unit specified in time Table file is invalid.  Acceptable units are Milliseconds, Microseconds, and Seconds.");
            }
        }

        #endregion
    }
}