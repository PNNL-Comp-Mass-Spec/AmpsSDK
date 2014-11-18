// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationAction.cs" company="">
//   
// </copyright>
// <summary>
//   Base class for handling a notification action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Notifications
{
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// Base class for handling a notification action.
    /// </summary>
    public abstract class NotificationAction
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationAction"/> class.
        /// </summary>
        /// <param name="notification">
        /// TODO The notification.
        /// </param>
        public NotificationAction(Notification notification)
        {
            this.Notification = notification;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the notification.
        /// </summary>
        public Notification Notification { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// TODO The evaluate.
        /// </summary>
        /// <param name="value">
        /// TODO The value.
        /// </param>
        public abstract void Evaluate(dynamic value);

        #endregion
    }
}