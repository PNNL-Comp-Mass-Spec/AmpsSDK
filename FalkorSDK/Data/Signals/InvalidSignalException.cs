// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidSignalException.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The invalid signal exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Signals
{
    using System;

    /// <summary>
    /// TODO The invalid signal exception.
    /// </summary>
    public class InvalidSignalException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSignalException"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public InvalidSignalException(string message)
            : base(message)
        {
        }

        #endregion
    }
}