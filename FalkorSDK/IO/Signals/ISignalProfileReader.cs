// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISignalProfileReader.cs" company="">
//   
// </copyright>
// <summary>
//   Interface for reading signals
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using FalkorSDK.Data.Signals;
    using FalkorSDK.Devices;

    /// <summary>
    /// The SignalProfileReader interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    /// <typeparam name="U">
    /// </typeparam>
    public interface ISignalProfileReader<out T, in U>
        where T : SignalOutputProfile
        where U : IFalkorDevice
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The read.
        /// </summary>
        /// <param name="device">
        /// The device.
        /// </param>
        /// <param name="path">
        /// TODO The path.
        /// </param>
        /// <returns>
        /// The <see cref="SignalOutputProfile"/>.
        /// </returns>
        T Read(U device, string path);

        #endregion
    }
}