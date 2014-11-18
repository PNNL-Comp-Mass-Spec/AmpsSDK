// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClockEvent.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The clock event.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Events
{
	using FalkorSDK.Channel;
	using FalkorSDK.Data.Signals;

    /// <summary>
    /// TODO The clock event.
    /// </summary>
    public class ClockEvent : SignalEvent
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClockEvent"/> class.
        /// </summary>
        /// <param name="channel">
        /// TODO The Channel.
        /// </param>
        /// <param name="startTime">
        /// TODO The start time.
        /// </param>
        /// <param name="endTime">
        /// TODO The end time.
        /// </param>
        public ClockEvent(int channel, double startTime, double endTime)
            : base(channel)
        {
            this.StartTime = startTime;
            this.StopTime = endTime;
            this.IsInfinite = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether is infinite.
        /// </summary>
        public bool IsInfinite { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public double StartTime { get; set; }

        /// <summary>
        /// Gets or sets the stop time.
        /// </summary>
        public double StopTime { get; set; }

        #endregion
    }
}