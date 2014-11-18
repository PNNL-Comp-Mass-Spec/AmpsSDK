// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFalkorDevice.cs" company="">
//   
// </copyright>
// <summary>
//   A device usable by the Falkor System.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    /// <summary>
    /// A device usable by the Falkor System.
    /// </summary>
    [InheritedExport]
    public interface IFalkorDevice
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the Id for this device.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this device.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the status of the device
        /// </summary>
        DeviceStatus Status { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Causes the device to initialize itself using async patterns (TAP)
        /// </summary>
        /// <returns>True if the initialization worked.  False if initialize failed.</returns>
        Task<bool> InitializeAsync();

        /// <summary>
        /// Loads the device.
        /// </summary>
        /// <param name="fileLocation">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task LoadAsync(string fileLocation = "");

        /// <summary>
        /// Saves the device.
        /// </summary>
        /// <param name="fileLocation">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task SaveAsync(string fileLocation = "");

        /// <summary>
        /// Gets the plugin directory.
        /// </summary>
        string PluginDirectory { get; }

        #endregion
    }
}