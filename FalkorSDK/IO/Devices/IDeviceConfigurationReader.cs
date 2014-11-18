// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeviceConfigurationReader.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IDeviceConfigurationReader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.IO.Devices
{
    using FalkorSDK.Devices;

    /// <summary>
    /// TODO The DeviceConfigurationReader interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IDeviceConfigurationReader<out T> where T : IFalkorDevice
    {
        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T Read(string filePath);
    }
}