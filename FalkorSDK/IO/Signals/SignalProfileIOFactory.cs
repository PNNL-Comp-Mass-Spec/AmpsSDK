// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalProfileIOFactory.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The signal profile io factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using FalkorSDK.Data.Signals;
    using FalkorSDK.Devices;

    /// <summary>
    /// TODO The signal profile io factory.
    /// </summary>
    public class SignalProfileIoFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a reader for the time Table path provided.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The <see cref="ISignalProfileReader"/>.
        /// </returns>
        public static ISignalProfileReader<SignalOutputProfile, IFalkorDevice> CreateReader(StaticSignalTableFormats format)
        {
            ISignalProfileReader<SignalOutputProfile, IFalkorDevice> reader = null;
            switch (format)
            {
                case StaticSignalTableFormats.Text:
                    reader = new SignalProfileReaderPlain();
                    break;
            }

            return reader;
        }

        /// <summary>
        /// Creates a reader for the time Table path provided.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The <see cref="ISignalProfileWriter"/>.
        /// </returns>
        public static ISignalProfileWriter<SignalOutputProfile, IFalkorDevice> CreateWriter(StaticSignalTableFormats format)
        {
            ISignalProfileWriter<SignalOutputProfile, IFalkorDevice> writer = null;
            switch (format)
            {
                case StaticSignalTableFormats.Text:
                    writer = new SignalProfileWriterPlain();
                    break;
            }

            return writer;
        }

        #endregion
    }
}