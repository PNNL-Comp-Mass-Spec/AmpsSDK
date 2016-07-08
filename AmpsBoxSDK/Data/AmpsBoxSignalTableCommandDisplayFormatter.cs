// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsBoxSignalTableCommandDisplayFormatter.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The amps box signal Table command display formatter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FalkorSDK.Data;
    using FalkorSDK.Data.Elements;
    using FalkorSDK.Data.Signals;
    using FalkorSDK.IO.Signals;

    /// <summary>
    /// TODO The amps box signal Table command display formatter.
    /// </summary>
    public class AmpsBoxSignalTableCommandDisplayFormatter : ISignalTableFormatter<AmpsSignalTable, double>
    {
        #region Fields

        /// <summary>
        /// Table command to format
        /// </summary>
        private readonly string m_commandFormat;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsBoxSignalTableCommandDisplayFormatter"/> class.
        /// </summary>
        /// <param name="commandFormat">
        /// TODO The command format.
        /// </param>
        public AmpsBoxSignalTableCommandDisplayFormatter(string commandFormat)
        {
            this.m_commandFormat = commandFormat;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsBoxSignalTableCommandDisplayFormatter"/> class.
        /// </summary>
        public AmpsBoxSignalTableCommandDisplayFormatter()
        {
            this.m_commandFormat = "TABLE,{0:F0};\n{1}";
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
        public string FormatTable(AmpsSignalTable table, ITimeUnitConverter<double> converter)
        {
            string eventData = string.Empty;

            TimeUnits units = TimeUnits.Ticks;
            foreach (var point in table.Points)
            {
                StringBuilder timeBuilder = new StringBuilder();

           //     timeBuilder.AppendFormat("\tTime\t{0:F0}\n", converter.ConvertTo(table.ExecutionData.TimeUnits, units, time));

                foreach (var signal in point.DcBiasElements)
                {
                    timeBuilder.AppendFormat(
                           "\t\tChannel\t{0}\tVoltage\t{1:F0}\n",
                           signal.ChannelIdentifier,
                           signal.Voltage);
                }

                foreach (var digitalOutputElement in point.DigitalOutputElements)
                {
                    timeBuilder.AppendFormat(
                           "\t\tChannel\t{0}\tTTL State\t{1:F0}\n",
                           digitalOutputElement.ChannelIdentifier,
                           digitalOutputElement.State);
                }

                string events = timeBuilder.ToString().TrimEnd(':');
                eventData += events;
            }

            eventData = eventData.TrimEnd(',');

            return string.Format(
                this.m_commandFormat, 
                eventData);
        }

        #endregion
    }
}