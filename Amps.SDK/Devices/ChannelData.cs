namespace AmpsBoxSdk.Devices
{
    /// <summary>
	/// Data for each channel
	/// </summary>
	public class ChannelData 
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ChannelData"/> class.
		/// </summary>
		public ChannelData(double minimumdouble, double maximumdouble)
		{
		    Minimum = minimumdouble;
		    Maximum = maximumdouble;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the actual.
		/// </summary>
		public double Actual { get; private set; }

		/// <summary>
		/// Gets or sets the maximum.
		/// </summary>
		public double Maximum { get; private set; }

		/// <summary>
		/// Gets or sets the minimum.
		/// </summary>
		public double Minimum { get; private set; }

		/// <summary>
		/// Gets or sets the setpoint.
		/// </summary>
		public double Setpoint { get; private set; }

		#endregion
	}
}