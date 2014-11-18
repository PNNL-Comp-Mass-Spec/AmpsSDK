// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FalkorErrorArgs.cs" company="">
//   
// </copyright>
// <summary>
//   Event arguments for errors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data
{
    using System;

    /// <summary>
    /// Event arguments for errors.
    /// </summary>
    public class FalkorErrorEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FalkorErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        /// <param name="ex">
        /// TODO The ex.
        /// </param>
        public FalkorErrorEventArgs(string message, Exception ex)
        {
            this.Message = message;
            this.Exception = ex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FalkorErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public FalkorErrorEventArgs(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FalkorErrorEventArgs"/> class.
        /// </summary>
        public FalkorErrorEventArgs()
            : this(string.Empty, null)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; private set; }

        #endregion
    }
}