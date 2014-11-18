// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDigitalInputDevice.cs" company="">
//   
// </copyright>
// <summary>
//   Digital input device interface
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using FalkorSDK.Data.Signals;

    /// <summary>
    /// Digital input device interface
    /// </summary>
    public interface IDigitalInputDevice : IFalkorDevice
    {
        #region Public Methods and Operators

        /// <summary>
        /// Reads the state of a particular signal
        /// </summary>
        /// <param name="signal">
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool ReadDigitalChannel(int channel);

        #endregion
    }
}