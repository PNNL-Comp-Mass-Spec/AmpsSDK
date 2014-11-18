// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalTableReaderFactory.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The signal Table reader factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using System.IO;

    using FalkorSDK.Data.Signals;

    /// <summary>
    /// TODO The signal Table reader factory.
    /// </summary>
    public class SignalTableReaderFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a reader for the time Table path provided.
        /// </summary>
        /// <param name="path">
        /// Path of file to read.
        /// </param>
        /// <returns>
        /// Reader capable of reading the file provided
        /// </returns>
        public static ISignalTableReader<SignalTable> CreateReader(string path)
        {
            var extension = Path.GetExtension(path).ToLower();
            ISignalTableReader<SignalTable> reader = null;

            switch (extension)
            {
                case ".txt":
                    reader = new SignalTimeTableReaderPlain();
                    break;
                default:
                    break;
            }

            return reader;
        }

        #endregion
    }
}