// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsClockConverter.cs" company="">
//   
// </copyright>
// <summary>
//   Converts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AmpsBoxSdk.Data;

namespace AmpsBoxSdk.Devices
{
    /// <summary>
    /// Converts 
    /// </summary>
    public class AmpsClockConverter : ITimeUnitConverter<double>
    {
        #region Constants

        /// <summary>
        /// TODO The interna l_ tim e_ scal e_ divider.
        /// </summary>
        private const double INTERNAL_TIME_SCALE_DIVIDER = 1024;

        /// <summary>
        /// TODO The microsecond s_ pe r_ second.
        /// </summary>
        private const double MICROSECONDS_PER_SECOND = 1000000;

        /// <summary>
        /// TODO The scale r_ microseconds.
        /// </summary>
        private const double SCALER_MICROSECONDS = 1;

        /// <summary>
        /// TODO The scale r_ milliseconds.
        /// </summary>
        private const double SCALER_MILLISECONDS = 1e3;

        /// <summary>
        /// TODO The scale r_ seconds.
        /// </summary>
        private const double SCALER_SECONDS = 1e6;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsClockConverter"/> class. 
        /// Creates a clock converter object
        /// </summary>
        /// <param name="frequency">
        /// </param>
        public AmpsClockConverter(double frequency)
        {
            this.Clockint = frequency;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the clock frequency.
        /// </summary>
        public double Clockint { get; private set; }

        #endregion

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
            // Here we first convert to microseconds...just in case we need to convert to ticks
            SimpleTimeConverter subConverter = new SimpleTimeConverter();
            time = subConverter.ConvertTo(from, TimeUnits.Microseconds, time);

            // At this point everything is in microseconds...
            double scaler = SCALER_MICROSECONDS;
            switch (to)
            {
                case TimeUnits.Microseconds:
                    scaler = SCALER_MICROSECONDS;
                    break;
                case TimeUnits.Milliseconds:
                    scaler = SCALER_MILLISECONDS;
                    break;
                case TimeUnits.Seconds:
                    scaler = SCALER_SECONDS;
                    break;
                case TimeUnits.Ticks:
                    scaler = SCALER_SECONDS / this.Clockint * 1000;
                    break;
                default:
                    break;
            }

            return time / scaler;
        }

        #endregion
    }
}