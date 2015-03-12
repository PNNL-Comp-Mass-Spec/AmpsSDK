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
			this.RfFrequency = new ChannelData();
			this.DriveLevel = new ChannelData();
			this.OutputVoltage = new ChannelData();
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
		public ChannelData OutputVoltage { get; set; }

		/// <summary>
		/// Gets or sets the RF Frequency
		/// </summary>
		public ChannelData RfFrequency { get; set; }

		#endregion
	}
}