// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PulseEvent.cs" company="">
//   
// </copyright>
// <summary>
//   Describes a pulse event (Digital or Analog) for scalar channels.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Events
{
    using FalkorSDK.Channel;

    /// <summary>
    /// Describes a pulse event (Digital or Analog) for scalar channels.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class PulseEvent<T> : SignalEvent
		where T : struct 
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PulseEvent{T}"/> class. 
        /// Creates a new pulse event with 
        /// </summary>
        /// <param name="channel">
        /// The Channel.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="length">
        /// The length.
        /// </param>
        protected PulseEvent(int channel, T value, double length)
            : base(channel)
        {
            this.Length = length;
            this.PulseValue = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the ending value of the pulse
        /// </summary>
        public T EndValue { get; set; }

        /// <summary>
        /// Gets or sets the length of the pulse.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Gets or sets the height of the pulse
        /// </summary>
        public T PulseValue { get; set; }

        /// <summary>
        /// Gets or sets the intial and ending value of the pulse.
        /// </summary>
        public T StartValue { get; set; }

        #endregion
    }
}