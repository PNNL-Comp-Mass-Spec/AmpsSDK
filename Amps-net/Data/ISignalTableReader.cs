// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISignalTableReader.cs" company="">
//   
// </copyright>
// <summary>
//   Reads signals
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Data
{
    /// <summary>
    /// Reads signals 
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface ISignalTableReader<out T> where T : AmpsSignalTable
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
        T Read(string path, string delimiter="\t");

        #endregion
    }
}