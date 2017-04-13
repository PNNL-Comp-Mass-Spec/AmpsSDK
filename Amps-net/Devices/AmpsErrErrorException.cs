// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsErrErrorException.cs" company="">
//   
// </copyright>
// <summary>
//   Thrown when the AMPS responds with an err message
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Devices
{
    using System;

    /// <summary>
    /// Thrown when the AMPS responds with an err message
    /// </summary>
    public class AmpsErrErrorException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsErrErrorException"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public AmpsErrErrorException(string message)
            : base(message)
        {
        }

        #endregion
    }
}