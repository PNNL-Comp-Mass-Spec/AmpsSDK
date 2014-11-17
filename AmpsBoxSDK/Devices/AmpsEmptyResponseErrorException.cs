// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsEmptyResponseErrorException.cs" company="">
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
    public class AmpsEmptyResponseErrorException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsEmptyResponseErrorException"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public AmpsEmptyResponseErrorException(string message)
            : base(message)
        {
        }

        #endregion
    }
}