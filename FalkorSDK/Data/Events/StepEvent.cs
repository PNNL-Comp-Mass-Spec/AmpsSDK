// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StepEvent.cs" company="">
//   
// </copyright>
// <summary>
//   A step event for abstract siganls
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Events
{
    using FalkorSDK.Channel;

    /// <summary>
    /// A step event for abstract siganls
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class StepEvent<T> : SignalEvent
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StepEvent{T}"/> class.
        /// </summary>
        /// <param name="channel">
        /// TODO The Channel.
        /// </param>
        protected StepEvent(int channel)
            : base(channel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepEvent{T}"/> class.
        /// </summary>
        /// <param name="signal">
        /// TODO The Channel.
        /// </param>
        /// <param name="time">
        /// TODO The time.
        /// </param>
        /// <param name="value">
        /// TODO The value.
        /// </param>
        protected StepEvent(int channel, double time)
            : base(channel, time)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepEvent{T}"/> class.
        /// </summary>
        protected StepEvent()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the value of a voltage step.
        /// </summary>
        public abstract T Value { get; set; }

        #endregion
    }
}