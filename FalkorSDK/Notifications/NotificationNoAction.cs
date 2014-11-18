// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationNoAction.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The notification no action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Notifications
{
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// TODO The notification no action.
    /// </summary>
    public class NotificationNoAction : NotificationAction
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationNoAction"/> class.
        /// </summary>
        /// <param name="notification">
        /// TODO The notification.
        /// </param>
        public NotificationNoAction(Notification notification)
            : base(notification)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// TODO The evaluate.
        /// </summary>
        /// <param name="value">
        /// TODO The value.
        /// </param>
        public override void Evaluate(dynamic value)
        {
            // Does nothing....
        }

        #endregion
    }
}