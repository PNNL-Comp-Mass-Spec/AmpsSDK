// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalTable.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The signal Table.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Signals
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    using FalkorSDK.Data.Events;

    using ReactiveUI;

    /// <summary>
    /// TODO The signal Table.
    /// </summary>
    [Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
    public class SignalTable : ReactiveObject
    {
        #region Fields

        /// <summary>
        /// TODO The length.
        /// </summary>
        private double length;

        /// <summary>
        /// TODO The name.
        /// </summary>
        private string name;

        /// <summary>
        /// TODO The comment.
        /// </summary>
        private string comment;

        /// <summary>
        /// The iterations.
        /// </summary>
        private int iterations;

        /// <summary>
        /// Holds the units the time values are stored in
        /// </summary>
        private TimeTableUnits timeUnits;

        /// <summary>
        /// The current time units.
        /// </summary>
        private TimeTableUnits currentTimeUnits;

		public ReactiveList<SignalEvent> SignalEvents { get; private set; } 

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalTable"/> class. 
        /// Constructor.
        /// </summary>
        public SignalTable()
        {
            this.Table = new MultiValueDictionary<double, SignalEvent>();
			this.SignalEvents = new ReactiveList<SignalEvent>();
            this.Id = Guid.NewGuid();
            this.WhenAnyValue(x => x.TimeUnits).Subscribe(this.OnNextTimeUnit);
            this.currentTimeUnits = this.TimeUnits;
        }

        /// <summary>
        /// TODO The on next time unit.
        /// </summary>
        /// <param name="timeTableUnits">
        /// TODO The time table units.
        /// </param>
        private void OnNextTimeUnit(TimeTableUnits timeTableUnits)
        {
           this.ConvertTime(timeTableUnits);
        }

        #endregion

        #region Public Properties

	    /// <summary>
	    /// Gets or sets the Comment for this Table
	    /// </summary>
	    public string Comment
	    {
		    get
		    {
			    return this.comment;
		    }

			set
			{
				this.RaiseAndSetIfChanged(ref this.comment, value);
			}
	    }

	    /// <summary>
	    /// Gets or sets the iterations.
	    /// </summary>
	    public int Iterations
	    {
		    get
		    {
			    return this.iterations;
		    }
		    set
		    {
			    this.RaiseAndSetIfChanged(ref this.iterations, value);
		    }
	    }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; private set; }

	    /// <summary>
	    /// Gets or sets the total length of a time Table.
	    /// </summary>
	    public double Length
	    {
		    get
		    {
			    return this.length;
		    }
		    set
		    {
			    this.RaiseAndSetIfChanged(ref this.length, value);
		    }
	    }

	    /// <summary>
	    /// Gets or sets the name of the Channel Table.
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
        /// Maintains a list of the times
        /// </summary>
        public MultiValueDictionary<double, SignalEvent> Table { get; private set; }

        /// <summary>
        /// Gets or sets the units for the time Table.
        /// </summary>
        public TimeTableUnits TimeUnits { get; set; }

        #endregion

	    public void AddStepEvent(double time, int channel, double voltage, string eventName)
	    {
		    var stepEvent = new AnalogStepEvent(channel, time, voltage) { Name = eventName };
			this.Add(stepEvent);
	    }

		public void AddStepEvent(double time, int channel, bool state, string eventName)
		{
			var stepEvent = new DigitalStepEvent(channel, time, state) { Name = eventName };
			this.Add(stepEvent);
		}

		#region Public Methods and Operators

		/// <summary>
		/// Add a signal to the time Table.
		/// </summary>
		/// <param name="signal">
		/// Channel to add
		/// </param>
		public void Add(SignalEvent signal)
        {
            var time = signal.Time;
            if (time < 0)
            {
                throw new ArgumentOutOfRangeException("signal", "The time cannot be negative.");
            }

            if (!this.Table.ContainsKey(signal.Time))
            {
                this.Table.Add(time, signal);
				this.SignalEvents.Add(signal);
                signal.WhenAny(x => x.Time, x => x).Subscribe(this.TimeChanged);
            }
            else
            {
                // TODO swap this out for a This.ChangeTime(signal) call?  Most likely but discuss first.
                IReadOnlyCollection<SignalEvent> signals = this.Table[time];

                SignalEvent eventToEdit = null;

                foreach (var tempSignal in signals)
                {
                    if (signal.Channel == tempSignal.Channel)
                    {
                        eventToEdit = signal;
                        break;
                    }
                }

                if (eventToEdit != null)
                {
                    signal.Time += 1;
                    this.Add(signal);
                }
                else
                {
                    this.Table.Add(time, signal);
					this.SignalEvents.Add(signal);
					signal.WhenAny(x => x.Time, x => x).Subscribe(this.TimeChanged);
                }
            }
        }

        /// <summary>
        /// Clears the voltage timing Table.
        /// </summary>
        public void Clear()
        {
            this.Table.Clear();
        }

        /// <summary>
        /// Retrieves the list of signals for a specific time slice.
        /// </summary>
        /// <param name="time">
        /// Time to select signals for.
        /// </param>
        /// <returns>
        /// List of signals
        /// </returns>
        public IEnumerable<SignalEvent> GetSignals(double time)
        {
            if (this.Table.ContainsKey(time))
            {
                return this.Table[time];
            }

            return new List<SignalEvent>();
        }

        /// <summary>
        /// Retrieves a list of all signals.
        /// </summary>
        /// <returns>List of signals</returns>
        public IEnumerable<SignalEvent> GetSignals()
        {
            var list = new List<SignalEvent>();
            foreach (var signals in this.Table.Values)
            {
                foreach (var signal in signals)
                {
                    list.Add(signal);
                }
            }

            return list;
        }

        /// <summary>
        /// Returns the list of time Table events.
        /// </summary>
        /// <returns>List of time Table events in microseconds.</returns>
        public ICollection<double> GetTimes()
        {
            List<double> times = new List<double>();
            foreach (double time in this.Table.Keys)
            {
                times.Add(time);
            }

            times.Sort();

            return times;
        }

        /// <summary>
        /// TODO The remove.
        /// </summary>
        /// <param name="signal">
        /// TODO The signal.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Remove(SignalEvent signal)
        {
            if (this.Table.ContainsValue(signal))
            {
                foreach (var key in this.Table.Keys)
                {
                    if (this.Table.Remove(key, signal))
                    {
	                    this.SignalEvents.Remove(signal);
                        return true;
                    }
                }

                return false;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// TODO The change time.
        /// </summary>
        /// <param name="signalEvent">
        /// TODO The signal event.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        /// TODO ChangeTime is Private, but should it be?
        private void ChangeTime(SignalEvent signalEvent)
        {
            var time = signalEvent.Time;
            if (time < 0)
            {
                throw new ArgumentOutOfRangeException("signalEvent", "The time cannot be negative.");
            }

            if (!this.Table.ContainsKey(signalEvent.Time))
            {
                this.Table.Add(time, signalEvent);
				this.SignalEvents.Add(signalEvent);
			}
            else
            {
                IReadOnlyCollection<SignalEvent> signals = this.Table[time];

                foreach (var tempSignal in signals)
                {
                    if (signalEvent.Channel == tempSignal.Channel)
                    {
                        signalEvent.Time++;
                    }
                }

                this.Table.Add(time, signalEvent);
				this.SignalEvents.Add(signalEvent);
			}
        }

        /// <summary>
        /// Converts each entry from one time unit to the new time unit
        /// </summary>
        /// <param name="to">
        /// </param>
        private void ConvertTime(TimeTableUnits to)
        {
            SimpleTimeConverter timeConverter = new SimpleTimeConverter();

            MultiValueDictionary<double, SignalEvent> signals = new MultiValueDictionary<double, SignalEvent>();

            foreach (var signal in this.GetSignals())
            {
                double time = timeConverter.ConvertTo(this.currentTimeUnits, to, signal.Time);
                if (!signals.ContainsKey(time))
                {
                    signal.Time = time;
                    signals.Add(time, signal);
                }
            }

            this.Length = timeConverter.ConvertTo(this.currentTimeUnits, to, this.Length);
            this.currentTimeUnits = to;
            this.Table = signals;
        }

        /// <summary>
        /// TODO The time changed.
        /// </summary>
        /// <param name="observedChange">
        /// </param>
        private void TimeChanged(IObservedChange<SignalEvent, double> observedChange)
        {
            var sender = observedChange.Sender;
            var value = observedChange.GetValue();
            {
                if (this.Remove(sender))
                {
                    this.ChangeTime(sender);
                }
            }
        }

        #endregion
    }
}