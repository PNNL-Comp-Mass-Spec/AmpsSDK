// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISignalProfileWriter.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The SignalProfileWriter interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using FalkorSDK.Data.Signals;
    using FalkorSDK.Devices;

    /// <summary>
    /// TODO The SignalProfileWriter interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    /// <typeparam name="U">
    /// </typeparam>
    public interface ISignalProfileWriter<in T, in U> 
        where T : SignalOutputProfile
        where U : IFalkorDevice
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The write.
        /// </summary>
        /// <param name="path">
        /// TODO The path.
        /// </param>
        /// <param name="profile">
        /// TODO The profile.
        /// </param>
        /// <param name="device">
        /// The device.
        /// </param>
        void Write(string path, T profile, U device);

        #endregion
    }
}