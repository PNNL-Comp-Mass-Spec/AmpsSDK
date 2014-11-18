// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWriter.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The Writer interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Generic
{
    /// <summary>
    /// TODO The Writer interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IWriter<in T>
        where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The write.
        /// </summary>
        /// <param name="fileName">
        /// TODO The file name.
        /// </param>
        /// <param name="data">
        /// TODO The data.
        /// </param>
        void Write(string fileName, T data);

        #endregion
    }
}