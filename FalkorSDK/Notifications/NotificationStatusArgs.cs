// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationStatusArgs.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The notification status args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Notifications
{
    using System;

    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// TODO The notification status args.
    /// </summary>
    public class NotificationStatusArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationStatusArgs"/> class. 
        /// Default constructor
        /// </summary>
        /// <param name="notification">
        /// Notification system that spawned the event
        /// </param>
        /// <param name="value">
        /// Value of the notificatoin status
        /// </param>
        public NotificationStatusArgs(Notification notification, dynamic value)
        {
            this.Notification = notification;
            this.Value = value;
            this.DateTime = DateTime.Now;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the datetime of the notification event created. 
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Gets the notification system.
        /// </summary>
        public Notification Notification { get; private set; }

        /// <summary>
        /// Gets the value of the notification
        /// </summary>
        public dynamic Value { get; private set; }

        #endregion
    }
}