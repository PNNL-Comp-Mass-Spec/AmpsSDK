// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FalkorConfiguration.cs" company="">
//   
// </copyright>
// <summary>
//   Holds the configuration of a collection of devices for persistence.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO
{
    using System.Collections.Generic;

    /// <summary>
    /// Holds the configuration of a collection of devices for persistence.
    /// </summary>    
    public class FalkorConfiguration
    {
        #region Fields

        /// <summary>
        /// Holds a list of devices that can be enumerated through.
        /// </summary>
        private readonly List<string> m_items;

        /// <summary>
        /// Holds data about each device
        /// </summary>
        private readonly Dictionary<string, FalkorConfiguration> m_settings;

        /// <summary>
        /// Holds settings about this configuration
        /// </summary>
        private readonly Dictionary<string, object> m_values;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FalkorConfiguration"/> class. 
        /// Default constructor.
        /// </summary>
        public FalkorConfiguration()
        {
            this.m_items = new List<string>();
            this.m_values = new Dictionary<string, object>();
            this.m_settings = new Dictionary<string, FalkorConfiguration>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the values for the setting.
        /// </summary>
        public Dictionary<string, object> Attributes
        {
            get
            {
                return this.m_values;
            }
        }

        /// <summary>
        /// Gets the sub-settings.
        /// </summary>
        public Dictionary<string, FalkorConfiguration> Settings
        {
            get
            {
                return this.m_settings;
            }
        }

        /// <summary>
        /// Gets the type of configuration item this is
        /// </summary>
        public string Type { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the value to the object
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="value">
        /// </param>
        public void AddAttribute(string name, object value)
        {
            this.m_values.Add(name, value);
        }

        /// <summary>
        /// Adds the settings for the particular object
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="settings">
        /// </param>
        public void AddSetting(string name, FalkorConfiguration settings)
        {
            this.m_settings.Add(name, settings);
        }

        /// <summary>
        /// Retrieves the device settings for the specified device.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="FalkorConfiguration"/>.
        /// </returns>
        public FalkorConfiguration GetSettings(string name)
        {
            return this.m_settings[name];
        }

        #endregion
    }
}