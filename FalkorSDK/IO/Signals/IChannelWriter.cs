// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChannelWriter.cs" company="">
//   
// </copyright>
// <summary>
//   Writes a list of signals to the path provided.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using System.Collections.Generic;

    using FalkorSDK.Channel;

    /// <summary>
    /// The AnalogOutputDcChannelWriter interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IChannelWriter<in T>
        where T : Channel
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The write.
        /// </summary>
        /// <param name="path">
        /// TODO The path.
        /// </param>
        /// <param name="signals">
        /// TODO The signals.
        /// </param>
        void Write(string path, IEnumerable<T> signals);

        #endregion
    }
}