// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DigitalPulseEvent.cs" company="">
//   
// </copyright>
// <summary>
//   A pulse event for digital signals.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Events
{
	using FalkorSDK.Channel;
	using FalkorSDK.Data.Signals;

    /// <summary>
    /// A pulse event for digital signals.
    /// </summary>
    public sealed class DigitalPulseEvent : PulseEvent<bool>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalPulseEvent"/> class. 
        /// Creates a digital pulse event
        /// </summary>
        /// <param name="channel">
        /// Channel to create the event on
        /// </param>
        /// <param name="state">
        /// State to create the event in
        /// </param>
        /// <param name="length">
        /// Length of the event
        /// </param>
        public DigitalPulseEvent(int channel, bool state, double length)
            : base(channel, state, length)
        {
        }

        #endregion
    }
}