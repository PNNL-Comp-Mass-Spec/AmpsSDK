namespace AmpsBoxSdk.Devices
{
    using FalkorSDK.Channel;
    using FalkorSDK.Data.Signals;

    using Infrastructure.Domain.Shared;

    /// <summary>
	/// Data for each channel
	/// </summary>
	public class ChannelData : IValueObject<ChannelData>
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ChannelData"/> class.
		/// </summary>
		public ChannelData(double minimumVoltage, double maximumVoltage)
		{
		    this.Minimum = minimumVoltage;
		    this.Maximum = maximumVoltage;
		}

        public ChannelData()
        {
            
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

        public bool SameValueAs(ChannelData other)
        {
            throw new System.NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
	}
}