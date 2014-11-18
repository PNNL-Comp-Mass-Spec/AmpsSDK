// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISignalTableReaderAsync.cs" company="">
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
    using System.Threading.Tasks;

    using FalkorSDK.Data.Signals;

    /// <summary>
    /// Reads signals 
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface ISignalTableReaderAsync<T> where T : SignalTable
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The read.
        /// </summary>
        /// <param name="path">
        /// TODO The path.
        /// </param>
        /// <returns>
        /// The <see cref="SignalTable"/>.
        /// </returns>
        Task<T> ReadAsync(string path);

        #endregion
    }
}