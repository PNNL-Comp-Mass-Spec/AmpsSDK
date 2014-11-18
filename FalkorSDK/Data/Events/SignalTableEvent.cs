// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalTableEvent.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the SignalTableEvent type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.Data.Events
{
    using ReactiveUI;

    /// <summary>
    /// TODO The signal table event.
    /// </summary>
    public abstract class SignalTableEvent : ReactiveObject
    {
	    private double time;

	    private string name;


	    /// <summary>
	    /// Gets or sets the time of this event.
	    /// </summary>
	    public double Time
	    {
		    get
		    {
			    return this.time;
		    }

		    set
		    {
			    this.RaiseAndSetIfChanged(ref this.time, value);
		    }
	    }

	    /// <summary>
	    /// 
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
    }
}