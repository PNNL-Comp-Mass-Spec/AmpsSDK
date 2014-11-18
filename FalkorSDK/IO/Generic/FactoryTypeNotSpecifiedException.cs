// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FactoryTypeNotSpecifiedException.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The factory type not specified exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Generic
{
    using System;

    /// <summary>
    /// TODO The factory type not specified exception.
    /// </summary>
    public class FactoryTypeNotSpecifiedException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryTypeNotSpecifiedException"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public FactoryTypeNotSpecifiedException(string message)
            : base(message)
        {
        }

        #endregion
    }
}