// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWriterAsync.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The WriterAsync interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Generic
{
    using System.Threading.Tasks;

    /// <summary>
    /// TODO The WriterAsync interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IWriterAsync<in T>
        where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The write async.
        /// </summary>
        /// <param name="fileName">
        /// TODO The file name.
        /// </param>
        /// <param name="data">
        /// TODO The data.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task WriteAsync(string fileName, T data);

        #endregion
    }
}