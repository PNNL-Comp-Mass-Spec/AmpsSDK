// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeviceConfigurationWriter.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IDeviceConfigurationWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.IO.Devices
{
    using FalkorSDK.Devices;

    /// <summary>
    /// TODO The DeviceConfigurationWriter interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IDeviceConfigurationWriter<in T> where T : IFalkorDevice
    {
        /// <summary>
        /// The write.
        /// </summary>
        /// <param name="device">
        /// The device.
        /// </param>
        void Write(T device);
    }
}