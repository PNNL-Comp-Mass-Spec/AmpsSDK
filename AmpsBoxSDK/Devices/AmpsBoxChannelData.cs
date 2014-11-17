namespace AmpsBoxSdk.Devices
{
	/// <summary>
	/// Data for each channel
	/// </summary>
	public class AmpsBoxChannelData
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AmpsBoxChannelData"/> class.
		/// </summary>
		public AmpsBoxChannelData()
		{
			this.Maximum = 2000;
			this.Minimum = 0;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the actual.
		/// </summary>
		public int Actual { get; set; }

		/// <summary>
		/// Gets or sets the channel.
		/// </summary>
		public int Channel { get; set; }

		/// <summary>
		/// Gets or sets the maximum.
		/// </summary>
		public int Maximum { get; set; }

		/// <summary>
		/// Gets or sets the minimum.
		/// </summary>
		public int Minimum { get; set; }

		/// <summary>
		/// Gets or sets the setpoint.
		/// </summary>
		public int Setpoint { get; set; }

		#endregion
	}
}