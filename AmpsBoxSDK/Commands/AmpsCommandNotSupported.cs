// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsCommandNotSupported.cs" company="">
//   
// </copyright>
// <summary>
//   Exception class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Commands
{
    using System;

    /// <summary>
    /// Exception class
    /// </summary>
    public class AmpsCommandNotSupported : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsCommandNotSupported"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public AmpsCommandNotSupported(string message)
            : base(message)
        {
        }

        #endregion
    }
}