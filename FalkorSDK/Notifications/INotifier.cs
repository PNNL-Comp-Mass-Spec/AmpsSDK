// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INotifier.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The Notifier interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Notifications
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// TODO The Notifier interface.
    /// </summary>
    public interface INotifier
    {
        #region Public Events

        /// <summary>
        /// Fired when a notification error is made.
        /// </summary>
        event EventHandler<NotificationErrorArgs> NotificationError;

        /// <summary>
        /// Fired when a notification status is made.
        /// </summary>
        event EventHandler<NotificationStatusArgs> NotificationStatus;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a status list of a given notifier.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<Notification> GetErrorList();

        /// <summary>
        /// Gets a status list for a given notifier
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<Notification> GetStatusList();

        #endregion
    }
}