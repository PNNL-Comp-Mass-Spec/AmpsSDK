namespace AmpsBoxSdk.Devices
{
	/// <summary>
	/// Contains RF data for a given channel.
	/// </summary>
	public class AmpsBoxRfData
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AmpsBoxRfData"/> class. 
		/// Constructor
		/// </summary>
		public AmpsBoxRfData()
		{
			this.Frequency = new ChannelData(500, 5000);
			this.DriveLevel = new ChannelData(0, 255);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the drive level.
		/// </summary>
		public ChannelData DriveLevel { get; set; }

		/// <summary>
		/// Gets or sets the output voltage.
		/// </summary>
		public double Output { get; set; }

		/// <summary>
		/// Gets or sets the RF int
		/// </summary>
		public ChannelData Frequency { get; set; }

		#endregion
	}
}