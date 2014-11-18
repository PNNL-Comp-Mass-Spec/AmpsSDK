// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalOutputProfile.cs" company="">
//   
// </copyright>
// <summary>
//   Holds a set of signals.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Signals
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reactive.Linq;

    using FalkorSDK.Channel;
    using FalkorSDK.Data.Events;
    using FalkorSDK.Devices;

    using ReactiveUI;

    /// <summary>
    /// Holds a new profile for a given device with that devices available signals.
    /// </summary>
    public class SignalOutputProfile : ReactiveObject
    {
        #region Fields

        /// <summary>
        /// TODO The name.
        /// </summary>
        private string name;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalOutputProfile"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="device">
        /// The device.
        /// </param>
        public SignalOutputProfile(IInputOutputDevice device)
        {
            // TODO: Parameter device is not being used; use or delete it
            this.Signals = new ReactiveList<AnalogStepEvent>();
        }

        /// <summary>
        /// The on channel removed.
        /// </summary>
        /// <param name="aoChannel">
        /// The analog output channel.
        /// </param>
        private void OnChannelRemoved(AOChannel aoChannel)
        {
            AnalogStepEvent signalToRemove = null;
            foreach (var analogStepEvent in this.Signals)
            {
                if (analogStepEvent.Channel == aoChannel.Address)
                {
                    signalToRemove = analogStepEvent;
                }
            }

            if (signalToRemove != null)
            {
                this.Signals.Remove(signalToRemove);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalOutputProfile"/> class.
        /// </summary>
        /// <param name="device">
        /// TODO The device.
        /// </param>
        /// <param name="signals">
        /// TODO The signals.
        /// </param>
        public SignalOutputProfile(IFalkorDevice device, IEnumerable<AnalogStepEvent> signals)
        {
            // TODO: Parameter signals is not being used; use or delete it
            this.Device = device;
            this.Signals = new ReactiveList<AnalogStepEvent>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the device.
        /// </summary>
        public IFalkorDevice Device { get; private set; }

        /// <summary>
        /// Gets or sets the name of the channel profile.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.name, value);
            }
        }

        /// <summary>
        /// List of channel events that can occur
        /// </summary>
        public ReactiveList<AnalogStepEvent> Signals { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a new channel to the profile.
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="voltage">
        /// </param>
        public void AddSignal(int channel, double? voltage)
        {
            if (voltage != null)
            {
                var signalEvent = new AnalogStepEvent(channel, 0, voltage.Value);
                this.Signals.Add(signalEvent);
            }
        }

        /// <summary>
        /// Adds the step event to the profile.
        /// </summary>
        /// <param name="stepEvent">
        /// </param>
        public void AddSignal(AnalogStepEvent stepEvent)
        {
            this.Signals.Add(stepEvent);
        }

        #endregion
    }
}