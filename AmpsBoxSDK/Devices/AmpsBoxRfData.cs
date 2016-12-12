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
			this.Rfint = new ChannelData();
			this.DriveLevel = new ChannelData();
			this.Outputdouble = new ChannelData();
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
		public ChannelData Outputdouble { get; set; }

		/// <summary>
		/// Gets or sets the RF int
		/// </summary>
		public ChannelData Rfint { get; set; }

		#endregion
	}
}