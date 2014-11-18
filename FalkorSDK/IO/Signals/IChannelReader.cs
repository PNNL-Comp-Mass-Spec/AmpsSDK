// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChannelReader.cs" company="">
//   
// </copyright>
// <summary>
//   Reads signals
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using System.Collections;
    using System.Collections.Generic;

    using FalkorSDK.Data.Signals;
    using FalkorSDK.Channel;

    /// <summary>
    /// Reads signals 
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IChannelReader<out T>
        where T : Channel
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The read.
        /// </summary>
        /// <param name="path">
        /// TODO The path.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        IEnumerable<T> Read(string path);

        #endregion
    }
}