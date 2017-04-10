// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsBoxDeviceData.cs" company="">
//   
// </copyright>
// <summary>
//   Data for each device
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Devices
{
	using System.Collections.Generic;

    /// <summary>
    /// Data for each device
    /// </summary>
    public class AmpsBoxDeviceData
    {

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsBoxDeviceData"/> class.
        /// </summary>
        public AmpsBoxDeviceData(uint analogChannels, uint rfChannels, uint digitalChannels)
        {
            HvData = new Dictionary<uint, ChannelData>();
            RfData = new Dictionary<uint, AmpsBoxRfData>();

            this.NumberHvChannels = analogChannels;
            this.NumberRfChannels = rfChannels;
            this.NumberDigitalChannels = digitalChannels;
        }

        /// <summary>
        /// Initializes empty device data (perhaps useful for emulation at some point)
        /// </summary>
        public static AmpsBoxDeviceData Empty { get; } = new AmpsBoxDeviceData(0, 0, 0);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of supported HV channels.
        /// </summary>
        public uint NumberHvChannels { get; }

        /// <summary>
        /// Gets the number of supported RF channels.
        /// </summary>
        public uint NumberRfChannels { get; }

        public uint NumberDigitalChannels { get; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the high voltage data.
        /// </summary>
        private Dictionary<uint, ChannelData> HvData { get; set; }

        /// <summary>
        /// Gets or sets the RF data.
        /// </summary>
        private Dictionary<uint, AmpsBoxRfData> RfData { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Clears all of the data
        /// </summary>
        public void Clear()
        {
            RfData.Clear();
            HvData.Clear();
        }

        /// <summary>
        /// Gets the Hv Data for the specified channel.
        /// </summary>
        /// <param name="channel">
        /// The channel.
        /// </param>
        /// <returns>
        /// The <see cref="ChannelData"/>.
        /// </returns>
        public ChannelData GetHvData(uint channel)
        {
            if (channel > NumberHvChannels)
            {
                throw new ChannelOutOfRangeException("The RF channel requested is not supported by the device.");
            }

            return HvData[channel];
        }

        /// <summary>
        /// Gets the Rf Data for the specified channel.
        /// </summary>
        /// <param name="channel">
        /// The channel.
        /// </param>
        /// <returns>
        /// The <see cref="AmpsBoxRfData"/>.
        /// </returns>
        public AmpsBoxRfData GetRfData(uint channel)
        {
            if (channel > NumberHvChannels)
            {
                throw new ChannelOutOfRangeException("The RF channel requested is not supported by the device.");
            }

            return RfData[channel];
        }

        #endregion
    }
}