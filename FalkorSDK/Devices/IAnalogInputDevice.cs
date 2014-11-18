// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnalogInputDevice.cs" company="">
//   
// </copyright>
// <summary>
//   Interface for controlling an analog output device
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System.Collections.Generic;

    using FalkorSDK.Channel;
    using FalkorSDK.Data.Signals;

    /// <summary>
    /// Interface for controlling an analog output device
    /// </summary>
    public interface IAnalogInputDevice : IFalkorDevice
    {
        #region Public Properties

        /// <summary>
        /// Gets a list of available signals
        /// </summary>
        ICollection<AIChannel> AvailableSignals { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="signal">
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        double Read(AIChannel signal);

        /// <summary>
        /// Placehoolder for reading a waveform (e.g. mass spectrum)
        /// </summary>
        /// <param name="signal">
        /// </param>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        ICollection<double> ReadWaveform(AIChannel signal);

        #endregion
    }
}