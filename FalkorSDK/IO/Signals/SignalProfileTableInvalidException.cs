// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalProfileTableInvalidException.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The signal profile Table invalid exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using System;

    /// <summary>
    /// TODO The signal profile Table invalid exception.
    /// </summary>
    public class SignalProfileTableInvalidException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalProfileTableInvalidException"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public SignalProfileTableInvalidException(string message)
            : base(message)
        {
        }

        #endregion
    }
}