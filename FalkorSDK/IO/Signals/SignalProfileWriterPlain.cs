// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalProfileWriterPlain.cs" company="">
//   
// </copyright>
// <summary>
//   Writes a signal Table to the
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using System.IO;

    using FalkorSDK.Data.Signals;
    using FalkorSDK.Devices;

    /// <summary>
    /// Writes a signal Table to the 
    /// </summary>
    public class SignalProfileWriterPlain : ISignalProfileWriter<SignalOutputProfile, IFalkorDevice>
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
        public void Write(string path, SignalOutputProfile profile, IFalkorDevice device)
        {
            using (TextWriter writer = File.CreateText(path))
            {
                writer.WriteLine("Name\t{0}", profile.Name);
                writer.WriteLine("Device\tChannel\tName\tValue\tType");
                foreach (var signal in profile.Signals)
                {
                    writer.WriteLine(
                        "{0}\t{1}\t{2}\t{3}\t{4}", 
                        device.Name, 
                        signal.Channel, 
                        signal.Name, 
                        signal.Value, 
                        "AnalogOutput");
                }
            }
        }

        #endregion
    }
}