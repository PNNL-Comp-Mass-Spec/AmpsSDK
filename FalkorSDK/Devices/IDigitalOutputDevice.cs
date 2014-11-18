// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDigitalOutputDevice.cs" company="">
//   
// </copyright>
// <summary>
//   Digital output device
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FalkorSDK.Channel;
    using FalkorSDK.Data.Signals;

    /// <summary>
    /// Digital output device
    /// </summary>
    public interface IDigitalOutputDevice : IFalkorDevice
    {
        #region Public Properties

        /// <summary>
        /// Gets the available digital signals.
        /// </summary>
        IList<DOChannel> DigitalOutputChannels { get; }

        #endregion

        #region Public Methods and Operators

	    /// <summary>
	    /// Writes the TTL signal to the device.
	    /// </summary>
	    /// <param name="signal">
	    /// </param>
	    /// <param name="state"></param>
	    /// <returns>
	    /// The <see cref="Task"/>.
	    /// </returns>
	    Task WriteDigitalOutputAsync(int channel, bool state);

	    Task PulseDigitalOutputAsync(DOChannel channel);

	    #endregion
    }
}