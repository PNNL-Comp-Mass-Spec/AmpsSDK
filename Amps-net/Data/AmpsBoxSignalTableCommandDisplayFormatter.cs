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
    using System.Text;

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
            m_commandFormat = commandFormat;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsBoxSignalTableCommandDisplayFormatter"/> class.
        /// </summary>
        public AmpsBoxSignalTableCommandDisplayFormatter()
        {
            m_commandFormat = "TABLE,{0:F0};\n{1}";
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
                           "\t\tChannel\t{0}\tdouble\t{1:F0}\n",
                           signal.Key,
                           signal.Value);
                }

                foreach (var digitalOutputElement in point.DigitalOutputElements)
                {
                    timeBuilder.AppendFormat(
                           "\t\tChannel\t{0}\tTTL State\t{1:F0}\n",
                           digitalOutputElement.Key,
                           digitalOutputElement.Value);
                }

                string events = timeBuilder.ToString().TrimEnd(':');
                eventData += events;
            }

            eventData = eventData.TrimEnd(',');

            return string.Format(
                m_commandFormat, 
                eventData);
        }

        #endregion
    }
}