using System;
using System.Collections;
using System.Collections.Generic;

namespace FalkorSDK.Devices
{
    /// <summary>
    /// Device configuration data for a given device to load the hardware configuration from disk.
    /// </summary>
    public class DeviceConfigurationData
    {
        public DeviceConfigurationData(IFalkorDevice device)
        {
            Name    = device.Name;
            Type    = device.GetType();
        }
        public DeviceConfigurationData(string name, Type type)
        {
            Name    = name;
            Type    = type;
        }

        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the assembly to load the configuration from.
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// Stores the settings for each device
        /// </summary>
        public IEnumerable<FalkorSetting> Settings { get; set; }
    }
}
