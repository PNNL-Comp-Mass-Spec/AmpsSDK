// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISignalTableReader.cs" company="">
//   
// </copyright>
// <summary>
//   Reads signals
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FalkorSDK.Data.Signals;

    /// <summary>
    /// Reads signals 
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface ISignalTableReader<out T> where T : SignalTable
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
        T Read(string path);

        Task<IEnumerable<SignalTable>> ReadMultipleTablesAsync(string path);

        #endregion
    }
}