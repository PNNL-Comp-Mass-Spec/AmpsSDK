// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleTimeConverter.cs" company="">
//   
// </copyright>
// <summary>
//   Converts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data
{
    /// <summary>
    /// Converts 
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
        public double ConvertTo(TimeTableUnits from, TimeTableUnits to, double time)
        {
            double fromScaler = 1e6;
            double toScaler = 1e6;

            switch (from)
            {
                case TimeTableUnits.Microseconds:
                    break;
                case TimeTableUnits.Milliseconds:
                    fromScaler = 1e3;
                    break;
                case TimeTableUnits.Seconds:
                    fromScaler = 1;
                    break;
                default:
                    break;
            }

            switch (to)
            {
                case TimeTableUnits.Microseconds:
                    break;
                case TimeTableUnits.Milliseconds:
                    toScaler = 1e3;
                    break;
                case TimeTableUnits.Seconds:
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