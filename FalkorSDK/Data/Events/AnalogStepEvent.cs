// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalogStepEvent.cs" company="">
//   
// </copyright>
// <summary>
//   A step event for analog voltages.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Events
{
	using System.ComponentModel.Composition;

	using FalkorSDK.Channel;

    using ReactiveUI;

    /// <summary>
    /// A step event for analog voltages.
    /// </summary>
    [Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class AnalogStepEvent : StepEvent<double>
    {
        #region Fields

        /// <summary>
        /// TODO The value.
        /// </summary>
        private double value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogStepEvent"/> class.
        /// </summary>
        /// <param name="signal">
        /// TODO The signal.
        /// </param>
        /// <param name="time">
        /// TODO The time.
        /// </param>
        /// <param name="voltage">
        /// TODO The voltage.
        /// </param>
        public AnalogStepEvent(int channel, double time = 0, double voltage = 0)
            : base(channel, time)
        {
	        this.Value = voltage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogStepEvent"/> class.
        /// </summary>
        /// <param name="signalEvent">
        /// TODO The signal event.
        /// </param>
        public AnalogStepEvent(AnalogStepEvent signalEvent)
            : base(signalEvent.Channel, signalEvent.Time)
        {
	        this.Value = signalEvent.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogStepEvent"/> class.
        /// </summary>
        public AnalogStepEvent()
        {
        }

        #endregion

        #region Public Properties

	    /// <summary>
	    /// Gets or sets the value.
	    /// </summary>
	    public override double Value
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