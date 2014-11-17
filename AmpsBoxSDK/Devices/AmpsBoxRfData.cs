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
			this.RfFrequency = new AmpsBoxChannelData();
			this.DriveLevel = new AmpsBoxChannelData();
			this.OutputVoltage = new AmpsBoxChannelData();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the channel.
		/// </summary>
		public int Channel { get; set; }

		/// <summary>
		/// Gets or sets the drive level.
		/// </summary>
		public AmpsBoxChannelData DriveLevel { get; set; }

		/// <summary>
		/// Gets or sets the output voltage.
		/// </summary>
		public AmpsBoxChannelData OutputVoltage { get; set; }

		/// <summary>
		/// Gets or sets the RF Frequency
		/// </summary>
		public AmpsBoxChannelData RfFrequency { get; set; }

		#endregion
	}
}