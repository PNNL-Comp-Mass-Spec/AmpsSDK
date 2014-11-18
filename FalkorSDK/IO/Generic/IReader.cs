// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReader.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The Reader interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Generic
{
    /// <summary>
    /// TODO The Reader interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IReader<out T>
        where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The read.
        /// </summary>
        /// <param name="fileName">
        /// TODO The file name.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T Read(string fileName);

        #endregion
    }
}