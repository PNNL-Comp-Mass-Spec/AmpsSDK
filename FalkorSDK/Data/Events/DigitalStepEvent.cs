// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DigitalStepEvent.cs" company="">
//   
// </copyright>
// <summary>
//   A step event for digital voltages
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Events
{
	using System.ComponentModel.Composition;

	using ReactiveUI;

	/// <summary>
	/// A step event for digital voltages
	/// </summary>
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public sealed class DigitalStepEvent : StepEvent<bool>
    {
	    private bool value;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalStepEvent"/> class.
        /// </summary>
        /// <param name="channel">
        /// TODO The Channel.
        /// </param>
        public DigitalStepEvent(int channel)
            : base(channel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalStepEvent"/> class.
        /// </summary>
        /// <param name="channel">
        /// TODO The Channel.
        /// </param>
        /// <param name="time">
        /// TODO The time.
        /// </param>
        /// <param name="state">
        /// TODO The state.
        /// </param>
        public DigitalStepEvent(int channel, double time, bool state)
            : base(channel, time)
        {
	        this.Value = state;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalStepEvent"/> class.
        /// </summary>
        /// <param name="digitalStepEvent">
        /// TODO The digital step event.
        /// </param>
        public DigitalStepEvent(DigitalStepEvent digitalStepEvent)
            : base(digitalStepEvent.Channel, digitalStepEvent.Time)
        {
	        this.Value = digitalStepEvent.Value;
        }

		public DigitalStepEvent()
		{
			
		}

        #endregion

        #region Public Properties

	    /// <summary>
	    /// Gets or sets a value indicating whether value.
	    /// </summary>
	    public override bool Value
	    {
		    get
		    {
			    return this.value;
		    }

		    set
		    {
			    this.RaiseAndSetIfChanged(ref this.value, value);
		    }
	    }

	    #endregion
    }
}