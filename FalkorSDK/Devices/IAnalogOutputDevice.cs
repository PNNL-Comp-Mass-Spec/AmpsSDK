// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnalogOutputDevice.cs" company="">
//   
// </copyright>
// <summary>
//   Interface for controlling an analog output device
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    using FalkorSDK.Channel;
    using FalkorSDK.Data.Signals;

    /// <summary>
    /// Interface for controlling an analog output device
    /// </summary>
    [InheritedExport]
    public interface IAnalogOutputDevice : IFalkorDevice
    {
        #region Public Properties

        /// <summary>
        /// Gets a list of available signals
        /// </summary>
        IList<AOChannel> AnalogOutputChannels { get; }

        #endregion

        #region Public Methods and Operators

	    /// <summary>
	    /// TODO The write rf drive async.
	    /// </summary>
	    /// <param name="address"></param>
	    /// <param name="value">
	    /// TODO The value.
	    /// </param>
	    /// <returns>
	    /// The <see cref="Task"/>.
	    /// </returns>
	    Task WriteRadiofrequencyDriveAsync(int channel, int value);

        /// <summary>
        /// Writes the value on the signal provided.
        /// </summary>
        /// <param name="signal">
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task WriteSignalAsync(int channel, double value);

        /// <summary>
        /// TODO The write signal rf.
        /// </summary>
        /// <param name="signal">
        /// TODO The signal.
        /// </param>
        /// <param name="value">
        /// TODO The value.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task WriteSignalRadioFrequencyFrequencyAsync(int channel, int value);

        #endregion
    }
}