// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneralPurposeInputOutputOutOfRangeException.cs" company="">
//   
// </copyright>
// <summary>
//   Thrown when a value is out of range.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Signals
{
    using System;

    /// <summary>
    /// Thrown when a value is out of range.
    /// </summary>
    public class GeneralPurposeInputOutputOutOfRangeException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralPurposeInputOutputOutOfRangeException"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public GeneralPurposeInputOutputOutOfRangeException(string message)
            : base(message)
        {
        }

        #endregion
    }
}