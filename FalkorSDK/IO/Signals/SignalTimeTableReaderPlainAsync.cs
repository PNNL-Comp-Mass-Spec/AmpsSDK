// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalTimeTableReaderPlainAsync.cs" company="">
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
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using FalkorSDK.Channel;
    using FalkorSDK.Data;
    using FalkorSDK.Data.Events;
    using FalkorSDK.Data.Signals;

    /// <summary>
    /// Reads a plain text time Table for voltage timing diagrams.
    /// </summary>
    public class SignalTimeTableReaderPlainAsync : ISignalTableReaderAsync<SignalTable>
    {
        #region Constants

        /// <summary>
        /// The delimiter used to separate values in our file.
        /// </summary>
        public static readonly string Delimiter = "\t";

        #endregion

        #region Static Fields

        /// <summary>
        /// Total tables in file
        /// </summary>
        private static int tableCount;

        #endregion

        #region Fields

        /// <summary>
        /// TODO The max time.
        /// </summary>
        private double maxTime;

        /// <summary>
        /// TODO The table.
        /// </summary>
        private SignalTable table = new SignalTable();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// TODO The read.
        /// </summary>
        /// <param name="splitLines">
        /// TODO The split lines.
        /// </param>
        /// <returns>
        /// The <see cref="SignalTable"/>.
        /// </returns>
        /// <exception cref="TableLengthNotSpecified">
        /// Thrown if the table read in does not have length specified.
        /// </exception>
        /// <exception cref="TableNameNotSpecified">
        /// Thrown if the table read in does not have a name specified.
        /// </exception>
        public Task<SignalTable> ParseTableAsync(IEnumerable<List<string>> splitLines)
        {
            return Task.Run(
                () =>
                    {
                        tableCount++;

                        var keyValueTasks = from pair in splitLines.AsParallel() where pair.Count == 2 select pair;

                        var tableEntryTasks = from tableEntry in splitLines.AsParallel()
                                              where tableEntry.Count > 2
                                              select tableEntry;

                        keyValueTasks.ForAll(x => this.ParseAndSetKeyValuePair(x[0], x[1]));

                        if ((int)this.table.Length < 1)
                        {
                            throw new TableLengthNotSpecified(
                                "The timing Table file must include a time length.  Length" + Delimiter + "value");
                        }

                        if (string.IsNullOrWhiteSpace(this.table.Name))
                        {
                            throw new TableNameNotSpecified(
                                "The timing Table file must include a name.  Name" + Delimiter + "value");
                        }

                        tableEntryTasks.ForAll(x => this.ParseAndAddAnalogStepEvent(x));

                        return this.table;
                    });
        }

        /// <summary>
        /// Reads a time Table from the path.
        /// </summary>
        /// <param name="path">
        /// Path to read Table file from.
        /// </param>
        /// <returns>
        /// The <see cref="SignalTable"/>.
        /// </returns>
        public async Task<SignalTable> ReadAsync(string path)
        {
            // We could use ReadLineAsync but as the read all will not be able to do that, keeping this synchronous.  The caller can still await and not block the program.
            List<string> lines = new List<string>();

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    lines.Add(line);
                }
            }

            string[] delimiters = { Delimiter };
            this.maxTime = 0;

            return
                await
                this.ParseTableAsync(
                    lines.Select(x => x.ToLower().Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList()));
        }

        #endregion

        #region Methods

        /// <summary>
        /// TODO The add analog step event.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private Task ParseAndAddAnalogStepEvent(IList<string> data)
        {
            return Task.Run(
                () =>
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
                    });
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
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private Task ParseAndSetKeyValuePair(string key, string value)
        {
            return Task.Run(
                () =>
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
                    });
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