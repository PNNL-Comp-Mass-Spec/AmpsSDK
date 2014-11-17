// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsNakErrorException.cs" company="">
//   
// </copyright>
// <summary>
//   Thrown when the AMPS responds with a nak message.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Devices
{
    using System;

    /// <summary>
    /// Thrown when the AMPS responds with a nak message.
    /// </summary>
    public class AmpsNakErrorException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsNakErrorException"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public AmpsNakErrorException(string message)
            : base(message)
        {
        }

        #endregion
    }
}