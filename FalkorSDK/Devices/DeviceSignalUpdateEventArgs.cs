// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeviceSignalUpdateEventArgs.cs" company="">
//   
// </copyright>
// <summary>
//   Event arguments when a device updates its signal list.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System;
    using System.Collections.Generic;

    using FalkorSDK.Channel;

    /// <summary>
    /// Event arguments when a device updates its signal list.
    /// </summary>
    public class DeviceSignalUpdateEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceSignalUpdateEventArgs"/> class. 
        /// Constructor for the signal updates.
        /// </summary>
        /// <param name="device">
        /// </param>
        /// <param name="channels">
        /// </param>
        public DeviceSignalUpdateEventArgs(IFalkorDevice device, IEnumerable<Channel> channels)
        {
            this.Device = device;
            this.Signals = new List<Channel>(channels);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the device whose channels have changed.
        /// </summary>
        public IFalkorDevice Device { get; private set; }

        /// <summary>
        /// Gets the list of channels that have changed.
        /// </summary>
        public IEnumerable<Channel> Signals { get; private set; }

        #endregion
    }
}