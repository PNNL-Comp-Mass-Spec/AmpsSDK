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
        #region Constants

        /// <summary>
        /// Maximum number of channels available on any board.
        /// </summary>
        private const int NUMBER_OF_MAX_CHANNELS = 8;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsBoxDeviceData"/> class.
        /// </summary>
        public AmpsBoxDeviceData()
        {
            HvData = new Dictionary<int, ChannelData>();
            RfData = new Dictionary<int, AmpsBoxRfData>();

            for (int i = 1; i <= NUMBER_OF_MAX_CHANNELS; i++)
            {

            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of supported HV channels.
        /// </summary>
        public int NumberHvChannels { get; set; }

        /// <summary>
        /// Gets the number of supported RF channels.
        /// </summary>
        public int NumberRfChannels { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the high voltage data.
        /// </summary>
        private Dictionary<int, ChannelData> HvData { get; set; }

        /// <summary>
        /// Gets or sets the RF data.
        /// </summary>
        private Dictionary<int, AmpsBoxRfData> RfData { get; set; }

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
        public ChannelData GetHvData(int channel)
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
        public AmpsBoxRfData GetRfData(int channel)
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