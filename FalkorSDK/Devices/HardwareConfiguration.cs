// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HardwareConfiguration.cs" company="">
//   
// </copyright>
// <summary>
//   Captures information about the hardware configuration previously defined
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System.Collections.Generic;

    /// <summary>
    /// Captures information about the hardware configuration previously defined
    /// </summary>
    public class HardwareConfiguration
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HardwareConfiguration"/> class.
        /// </summary>
        /// <param name="devices">
        /// TODO The devices.
        /// </param>
        public HardwareConfiguration(IEnumerable<IFalkorDevice> devices)
        {
            this.DeviceData = devices;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the device data.
        /// </summary>
        public IEnumerable<IFalkorDevice> DeviceData { get; set; }

        /// <summary>
        /// Gets or sets the physical device data.
        /// </summary>
        public PhysicalDeviceConfiguration PhysicalDeviceData { get; set; }

        #endregion
    }
}