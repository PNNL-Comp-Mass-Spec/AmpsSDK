// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalTimeTableWriterPlain.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The amps box time Table writer plain.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using System.Collections.Generic;
    using System.IO;

    using FalkorSDK.Data.Events;
    using FalkorSDK.Data.Signals;

    /// <summary>
    /// TODO The amps box time Table writer plain.
    /// </summary>
    public class AmpsBoxTimeTableWriterPlain : ISignalTableWriter<SignalTable>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsBoxTimeTableWriterPlain"/> class.
        /// </summary>
        public AmpsBoxTimeTableWriterPlain()
        {
            this.Delimiter = "\t";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the delimiter.
        /// </summary>
        public string Delimiter { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Writes the supplied time Table to the path provided.
        /// </summary>
        /// <param name="path">
        /// File location to write the signalTable to.
        /// </param>
        /// <param name="signalTable">
        /// Table to write.
        /// </param>
        public void Write(string path, SignalTable signalTable)
        {
            using (TextWriter writer = File.CreateText(path))
            {
                string delimeter = this.Delimiter;
                writer.WriteLine("Length{0}{1}", delimeter, signalTable.Length);
                writer.WriteLine("Name{0}{1}", delimeter, signalTable.Name);
                writer.WriteLine("Units{0}{1}", delimeter, signalTable.TimeUnits);

                writer.WriteLine("Time{0}Device{0}Board{0}Channel{0}Voltage", delimeter);
                ICollection<double> counts = signalTable.GetTimes();

                foreach (double time in counts)
                {
                    IEnumerable<SignalEvent> signals = signalTable.GetSignals(time);

                    foreach (SignalEvent signal in signals)
                    {
                        AnalogStepEvent output = signal as AnalogStepEvent;
                        if (output != null)
                        {
                            writer.WriteLine(
                                "{1}{0}{2}{0}{3}", 
                                delimeter, 
                                signal.Time, 
                                signal.Signal.Channel, 
                                output.Value);
                        }
                    }
                }
            }
        }

        #endregion
    }
}