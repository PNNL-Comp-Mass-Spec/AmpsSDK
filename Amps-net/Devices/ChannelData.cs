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
		public ChannelData(double minimum, double maximum, double actual, double setPoint)
		{
		    Minimum = minimum;
		    Maximum = maximum;
			this.Actual = actual;
			this.Setpoint = setPoint;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the actual.
		/// </summary>
		public double Actual { get; }

		/// <summary>
		/// Gets or sets the maximum.
		/// </summary>
		public double Maximum { get; }

		/// <summary>
		/// Gets or sets the minimum.
		/// </summary>
		public double Minimum { get; }

		/// <summary>
		/// Gets or sets the setpoint.
		/// </summary>
		public double Setpoint { get; }

		public static ChannelData Generate(int minimum, int maximum, int actual, int setPoint)
		{
			return new ChannelData(minimum, maximum, actual, setPoint);
		}

		#endregion
	}
}