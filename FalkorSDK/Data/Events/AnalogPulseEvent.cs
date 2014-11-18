// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalogPulseEvent.cs" company="">
//   
// </copyright>
// <summary>
//   A pulse event for analog voltage signals.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Events
{
	using FalkorSDK.Channel;
	using FalkorSDK.Data.Signals;

    /// <summary>
    /// A pulse event for analog voltage signals.
    /// </summary>
    public sealed class AnalogPulseEvent : PulseEvent<double>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogPulseEvent"/> class. 
        /// Creates a digital pulse event
        /// </summary>
        /// <param name="channel">
        /// Channel to create the event on
        /// </param>
        /// <param name="voltage">
        /// </param>
        /// <param name="length">
        /// Length of the event
        /// </param>
        public AnalogPulseEvent(int channel, double voltage, double length)
            : base(channel, voltage, length)
        {
        }

        #endregion
    }
}