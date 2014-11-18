// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeviceStatusEventArgs.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The device status args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System;

    /// <summary>
    /// TODO The device status args.
    /// </summary>
    public class DeviceStatusEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceStatusEventArgs"/> class.
        /// </summary>
        /// <param name="status">
        /// TODO The status.
        /// </param>
        public DeviceStatusEventArgs(DeviceStatus status)
            : this(string.Empty, status)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceStatusEventArgs"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        /// <param name="status">
        /// TODO The status.
        /// </param>
        public DeviceStatusEventArgs(string message, DeviceStatus status)
        {
            this.Status = status;
            this.Message = message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        public DeviceStatus Status { get; private set; }

        #endregion
    }
}