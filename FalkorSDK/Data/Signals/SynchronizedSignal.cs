// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynchronizedSignal.cs" company="">
//   
// </copyright>
// <summary>
//   Manages links between multiple signals
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Signals
{
    using System.Collections.Generic;

    using FalkorSDK.Data.Events;

    /// <summary>
    /// Manages links between multiple signals
    /// </summary>
    public class SynchronizedSignal
    {
        #region Fields

        /// <summary>
        /// TODO The m_events.
        /// </summary>
        private readonly List<SignalEvent> m_events;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedSignal"/> class.
        /// </summary>
        /// <param name="sourceSignal">
        /// TODO The source signal.
        /// </param>
        public SynchronizedSignal(SignalEvent sourceSignal)
        {
            this.m_events = new List<SignalEvent>();
            this.SourceEvent = sourceSignal;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a list of signals
        /// </summary>
        public IEnumerable<SignalEvent> Signals
        {
            get
            {
                return this.m_events;
            }
        }

        /// <summary>
        /// Gets or sets the signal that 
        /// </summary>
        public SignalEvent SourceEvent { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Makes  the below signal relative
        /// </summary>
        /// <param name="sinkEvent">
        /// The sink Event.
        /// </param>
        /// <param name="relativeTime">
        /// </param>
        public void AddSignalEvent(SignalEvent sinkEvent, double relativeTime)
        {
            // TODO: Parameter relativeTime is not being used; use or delete it
            // We have this as a method so that we can control any logic of adding the signal to the 
            // enumeration if we later go with relative timing.
            this.m_events.Add(sinkEvent);

            sinkEvent.Time = sinkEvent.Time - this.SourceEvent.Time;
            sinkEvent.ParentEvent = this.SourceEvent;
        }

        /// <summary>
        /// Removes the relationship between the sink event and the source event
        /// </summary>
        /// <param name="sinkEvent">
        /// The sink Event.
        /// </param>
        public void RemoveSignal(SignalEvent sinkEvent)
        {
            sinkEvent.Time = this.SourceEvent.Time + sinkEvent.Time;
            sinkEvent.ParentEvent = null;
        }

        #endregion
    }
}