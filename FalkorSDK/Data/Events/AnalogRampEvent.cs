// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalogRampEvent.cs" company="">
//   
// </copyright>
// <summary>
//   A ramp event for analog voltages
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Events
{
	using FalkorSDK.Channel;
	using FalkorSDK.Data.Signals;

    /// <summary>
    /// A ramp event for analog voltages
    /// </summary>
    public sealed class AnalogRampEvent : SignalEvent
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogRampEvent"/> class.
        /// </summary>
        /// <param name="channel">
        /// TODO The Channel.
        /// </param>
        public AnalogRampEvent(int channel)
            : base(channel)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the ending value of the pulse
        /// </summary>
        public double EndValue { get; set; }

        /// <summary>
        /// Gets or sets the length of the event.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Gets or sets the intial and ending value of the pulse.
        /// </summary>
        public double StartValue { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Calculates the rate of change for the event.
        /// </summary>
        /// <returns>Volts per timestep</returns>
        public double GetRate()
        {
            return (this.StartValue - this.EndValue) / this.Length;
        }

        #endregion
    }
}