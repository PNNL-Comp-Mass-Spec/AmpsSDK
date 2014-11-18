// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISignalTableWriter.cs" company="">
//   
// </copyright>
// <summary>
//   Writes a list of signals to the path provided.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using FalkorSDK.Data.Signals;

    /// <summary>
    /// Writes a list of signals to the path provided.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface ISignalTableWriter<in T> where T : SignalTable
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
        void Write(string path, T signalTable);

        #endregion
    }
}