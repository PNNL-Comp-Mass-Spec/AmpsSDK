// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsResponseEventArgs.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The amps response event args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Devices
{
    using System;

    /// <summary>
    /// TODO The amps response event args.
    /// </summary>
    public class AmpsResponseEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsResponseEventArgs"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public AmpsResponseEventArgs(string message)
        {
            Message = message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        #endregion
    }
}