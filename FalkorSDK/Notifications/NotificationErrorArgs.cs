// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationErrorArgs.cs" company="">
//   
// </copyright>
// <summary>
//   Arguments for the notification system.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Notifications
{
    using System;

    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// Arguments for the notification system.
    /// </summary>
    public class NotificationErrorArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationErrorArgs"/> class. 
        /// Notification error arguments.
        /// </summary>
        /// <param name="notification">
        /// Notification to send
        /// </param>
        /// <param name="message">
        /// Error message to send
        /// </param>
        /// <param name="ex">
        /// Exception used
        /// </param>
        /// <param name="value">
        /// Value in the error argument
        /// </param>
        public NotificationErrorArgs(Notification notification, string message, Exception ex, dynamic value)
        {
            this.Message = message;
            this.Exception = ex;
            this.Notification = notification;
            this.Value = value;
            this.DateTime = DateTime.Now;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the Datetime of the event created.
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Gets the exception that was thrown (if exists).
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the message to be propagated.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the notification to alert the system with.
        /// </summary>
        public Notification Notification { get; private set; }

        /// <summary>
        /// Gets the value that should be propagated.
        /// </summary>
        public dynamic Value { get; private set; }

        #endregion
    }
}