// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReaderAsync.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The ReaderAsync interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Generic
{
    using System.Threading.Tasks;

    /// <summary>
    /// TODO The ReaderAsync interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IReaderAsync<T>
        where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The read async.
        /// </summary>
        /// <param name="fileName">
        /// TODO The file name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<T> ReadAsync(string fileName);

        #endregion
    }
}