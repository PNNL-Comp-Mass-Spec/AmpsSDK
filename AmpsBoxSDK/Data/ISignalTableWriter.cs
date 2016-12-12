// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISignalTableWriter.cs" company="">
//   
// </copyright>
// <summary>
//   Writes a list of signals to the path provided.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Data
{
    /// <summary>
    /// Writes a list of signals to the path provided.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface ISignalTableWriter<in T> where T : AmpsSignalTable
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The write.
        /// </summary>
        /// <param name="path">
        /// TODO The path.
        /// </param>
        /// <param name="signalTable">
        /// TODO The signals.
        /// </param>
        void Write(string path, T signalTable, string delimiter="\t");

        #endregion
    }
}