// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleTimeConverter.cs" company="">
//   
// </copyright>
// <summary>
//   Converts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Data
{
    /// <summary>
    /// Converts time. 
    /// </summary>
    public class SimpleTimeConverter : ITimeUnitConverter<double>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Converts the time in microseconds to ticks.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <param name="time">
        /// Time in microseconds
        /// </param>
        /// <returns>
        /// Ticks
        /// </returns>
        public double ConvertTo(TimeUnits from, TimeUnits to, double time)
        {
            // TODO: Replace with TimeUnitBase hierarchy. 

            double fromScaler = 1e6;
            double toScaler = 1e6;

            switch (from)
            {
                case TimeUnits.Microseconds:
                    break;
                case TimeUnits.Milliseconds:
                    fromScaler = 1e3;
                    break;
                case TimeUnits.Seconds:
                    fromScaler = 1;
                    break;
                default:
                    break;
            }

            switch (to)
            {
                case TimeUnits.Microseconds:
                    break;
                case TimeUnits.Milliseconds:
                    toScaler = 1e3;
                    break;
                case TimeUnits.Seconds:
                    toScaler = 1;
                    break;
                default:
                    break;
            }

            return time * (toScaler / fromScaler);
        }

        #endregion
    }
}