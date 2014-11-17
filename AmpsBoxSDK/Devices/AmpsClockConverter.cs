// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsClockConverter.cs" company="">
//   
// </copyright>
// <summary>
//   Converts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Devices
{
    using FalkorSDK.Data;

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
            this.ClockFrequency = frequency;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the clock frequency.
        /// </summary>
        public double ClockFrequency { get; private set; }

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
        public double ConvertTo(TimeTableUnits from, TimeTableUnits to, double time)
        {
            // Here we first conver to microseconds...just in case we need to convert to ticks
            SimpleTimeConverter subConverter = new SimpleTimeConverter();
            time = subConverter.ConvertTo(from, TimeTableUnits.Microseconds, time);

            // At this point everything is in microseconds...
            double scaler = SCALER_MICROSECONDS;
            switch (to)
            {
                case TimeTableUnits.Microseconds:
                    scaler = SCALER_MICROSECONDS;
                    break;
                case TimeTableUnits.Milliseconds:
                    scaler = SCALER_MILLISECONDS;
                    break;
                case TimeTableUnits.Seconds:
                    scaler = SCALER_SECONDS;
                    break;
                case TimeTableUnits.Ticks:
                    scaler = SCALER_SECONDS / (this.ClockFrequency / INTERNAL_TIME_SCALE_DIVIDER);
                    break;
                default:
                    break;
            }

            return time / scaler;
        }

        #endregion
    }
}