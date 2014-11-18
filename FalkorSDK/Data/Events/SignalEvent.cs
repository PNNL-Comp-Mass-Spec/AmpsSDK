// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalEvent.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The Channel event.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Events
{
    using System;

    using FalkorSDK.Data.Signals;
	using FalkorSDK.Channel;

    using ReactiveUI;

    /// <summary>
    /// TODO The Channel event.
    /// </summary>
    public abstract class SignalEvent : SignalTableEvent, IComparable
    {
	    private int channel; 

        #region Static Fields

        /// <summary>
        /// Default Channel count based on how many times a Channel has been created.
        /// </summary>
        private static int count;

        #endregion


        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalEvent"/> class. 
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="time">
        /// </param>
        protected SignalEvent(int channel, double time = 0)
        {
            this.Channel = channel;
            this.Time = time;
            this.Id = count++;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalEvent"/> class. 
        /// Default constructor
        /// </summary>
        protected SignalEvent()
        {
            this.Time = 0;
            this.Id = count++;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the absolute time of a Channel
        /// </summary>
        /// <returns>Time value in absolute seconds compared to the time of the containing Table</returns>
        public double AbsoluteTime
        {
            get
            {
                if (this.ParentEvent == null)
                {
                    return this.Time;
                }

                return this.ParentEvent.Time + this.Time;
            }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the event that controls the synchronization of this event.
        /// </summary>
        public SignalEvent ParentEvent { get; set; }

	    /// <summary>
	    /// Gets the Channel that this event is tied to.
	    /// </summary>
	    public int Channel
	    {
			get
			{
				return this.channel;
			}

		    set
		    {
			    this.RaiseAndSetIfChanged(ref this.channel, value);
		    }
	    }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// TODO The compare to.
        /// </summary>
        /// <param name="obj">
        /// TODO The obj.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CompareTo(object obj)
        {
            var otherObject = obj as SignalEvent;
            return ((IComparable)this.Time).CompareTo(otherObject.Time);
        }

        #endregion
    }
}