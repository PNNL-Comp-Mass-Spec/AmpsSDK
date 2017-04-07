// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalTimeTableWriterPlain.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The amps box time AnalogWaveformTable writer plain.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Linq;

namespace AmpsBoxSdk.Data
{
    /// <summary>
    /// TODO The amps box time AnalogWaveformTable writer plain.
    /// </summary>
    public class AmpsBoxTimeTableWriterPlain : ISignalTableWriter<AmpsSignalTable>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsBoxTimeTableWriterPlain"/> class.
        /// </summary>
        public AmpsBoxTimeTableWriterPlain()
        {
            Delimiter = "\t";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the delimiter.
        /// </summary>
        public string Delimiter { get; set; }

        #endregion


        /// <summary>
        /// Writes Amps signal table to file using given delimter.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="signalTable"></param>
        /// <param name="delimiter"></param>
        public void Write(string path, AmpsSignalTable signalTable, string delimiter = "\t")
        {
            using (TextWriter writer = File.CreateText(path))
            {
                writer.WriteLine("Name{0}{1}", delimiter, signalTable.Points.FirstOrDefault().Name);

                writer.WriteLine("Time{0}Channel{0}Value", delimiter);

                foreach (var psgPoint in signalTable.Points)
                {
                    writer.Write("{1}{0}", delimiter, psgPoint.TimePoint);
                    foreach (var dcBiasElement in psgPoint.DcBiasElements)
                    {
                        writer.Write("{1}{0}{2}", delimiter, dcBiasElement.Key, dcBiasElement.Value);
                    }

                    foreach (var digitalOutputElement in psgPoint.DigitalOutputElements)
                    {
                        writer.Write("{1}{0}{2}", delimiter, digitalOutputElement.Key, digitalOutputElement.Value);
                    }
                }
            }
        }
    }
}