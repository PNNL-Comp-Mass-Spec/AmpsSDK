// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsCommandNotRecognized.cs" company="">
//   
// </copyright>
// <summary>
//   This exception class is thrown when a command to the AMPS box was not understood.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Devices
{
    using System;

    /// <summary>
    /// This exception class is thrown when a command to the AMPS box was not understood.
    /// </summary>
    public class AmpsCommandNotRecognized : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsCommandNotRecognized"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public AmpsCommandNotRecognized(string message)
            : base(message)
        {
        }

        #endregion
    }
}