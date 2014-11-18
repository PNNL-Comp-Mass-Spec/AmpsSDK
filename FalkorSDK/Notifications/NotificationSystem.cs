// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationSystem.cs" company="">
//   
// </copyright>
// <summary>
//   System for handling notifications from various sub-systems.  Links specific notifications to actions to be executed.
//   Allows unpublished results to be pushed to other aspects of the application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Notifications
{
    using System;
    using System.Collections.Generic;

    using FalkorSDK.Data;

    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// System for handling notifications from various sub-systems.  Links specific notifications to actions to be executed.
    /// Allows unpublished results to be pushed to other aspects of the application.
    /// </summary>
    public class NotificationSystem
    {
        #region Fields

        /// <summary>
        /// Map for finding an error quickly given a notifier
        /// </summary>
        private Dictionary<INotifier, Dictionary<string, NotificationAction>> m_errors;

        /// <summary>
        /// Map for finding a status quickly given a notifier 
        /// </summary>
        private Dictionary<INotifier, Dictionary<string, NotificationAction>> m_status;

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when an error is handled.
        /// </summary>
        public event EventHandler<NotificationErrorArgs> ErrorHandled;

        /// <summary>
        /// Fired when a status is handled.
        /// </summary>
        public event EventHandler<NotificationStatusArgs> StatusHandled;

        /// <summary>
        /// Fired when an error occurred but was not published by the notifier.
        /// </summary>
        public event EventHandler<NotificationErrorArgs> UnpublishedError;

        /// <summary>
        /// Fired when a status occurred but was not published by the notifier.  
        /// </summary>
        public event EventHandler<NotificationStatusArgs> UnpublishedStatus;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets whether the system should be alerted of unpublished events.
        /// </summary>
        public bool ShouldReportUnpublishedEvents { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a system notification
        /// </summary>
        /// <param name="notifier">
        /// Object capable of alerting system of status and errors.
        /// </param>
        public void Add(INotifier notifier)
        {
            // Alert us when the status has changed.
            notifier.NotificationStatus += this.notifier_NotificationStatus;
            notifier.NotificationError += this.notifier_NotificationError;
        }

        #endregion

        // TODO: Do we need to pass the exception data? - possibly...For now we just evaluate the notification 
        #region Methods

        /// <summary>
        /// Registers errors for the notifier
        /// </summary>
        /// <param name="notifier">
        /// The notifier.
        /// </param>
        private void RegisterErrors(INotifier notifier)
        {
            IEnumerable<Notification> errors = notifier.GetErrorList();

            this.m_errors.Add(notifier, new Dictionary<string, NotificationAction>());

            foreach (var notification in errors)
            {
                this.m_errors[notifier].Add(notification.Title, new NotificationNoAction(notification));
            }
        }

        /// <summary>
        /// Registers status for the notifier
        /// </summary>
        /// <param name="notifier">
        /// The notifier.
        /// </param>
        private void RegisterStatus(INotifier notifier)
        {
            // Get a list of the notifications, then make sure we assign a no-action to them.
            IEnumerable<Notification> status = notifier.GetStatusList();

            this.m_status.Add(notifier, new Dictionary<string, NotificationAction>());
            foreach (var notification in status)
            {
                this.m_status[notifier].Add(notification.Title, new NotificationNoAction(notification));
            }
        }

        /// <summary>
        /// Handle an error notification.
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void notifier_NotificationError(object sender, NotificationErrorArgs e)
        {
            INotifier notifier = sender as INotifier;
            if (notifier != null)
            {
                // Make sure the notifier was registered.
                if (this.m_status.ContainsKey(notifier))
                {
                    // Then make sure that we have a notification subscribed to for that type.
                    // Some notifications could be missing...and we want to make sure that we get the right action.
                    string notificationName = e.Notification.Title;
                    if (this.m_status[notifier].ContainsKey(notificationName))
                    {
                        NotificationAction action = this.m_status[notifier][notificationName];
                        action.Evaluate(e.Value);

                        if (this.ErrorHandled != null)
                        {
                            this.ErrorHandled(this, e);
                        }
                    }
                    else if (this.ShouldReportUnpublishedEvents)
                    {
                        if (this.UnpublishedError != null)
                        {
                            this.UnpublishedError(this, e);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handle a status update notification.
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void notifier_NotificationStatus(object sender, NotificationStatusArgs e)
        {
            INotifier notifier = sender as INotifier;
            if (notifier != null)
            {
                // Make sure the notifier was registered.
                if (this.m_status.ContainsKey(notifier))
                {
                    // Then make sure that we have a notification subscribed to for that type.
                    // Some notifications could be missing...and we want to make sure that we get the right action.
                    string notificationName = e.Notification.Title;
                    if (this.m_status[notifier].ContainsKey(notificationName))
                    {
                        NotificationAction action = this.m_status[notifier][notificationName];
                        action.Evaluate(e.Value);

                        if (this.StatusHandled != null)
                        {
                            this.StatusHandled(this, e);
                        }
                    }
                    else if (this.ShouldReportUnpublishedEvents)
                    {
                        if (this.UnpublishedStatus != null)
                        {
                            this.UnpublishedStatus(this, e);
                        }
                    }
                }
            }
        }

        #endregion
    }
}