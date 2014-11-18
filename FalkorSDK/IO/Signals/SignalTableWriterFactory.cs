// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalTableWriterFactory.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The signal table writer factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using System.IO;

    using FalkorSDK.Data.Signals;

    /// <summary>
    /// TODO The signal Table writer factory.
    /// </summary>
    public class SignalTableWriterFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a reader for the time Table path provided.
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="path">
        /// Path of file to read.
        /// </param>
        /// <returns>
        /// Reader capable of reading the file provided
        /// </returns>
        public static ISignalTableWriter<SignalTable> CreateWriter(SignalTableWriterType type, string path)
        {
            var s = Path.GetExtension(path);
            if (s != null)
            {
                string extension = s.ToLower();
            }

            ISignalTableWriter<SignalTable> writer = null;

            switch (type)
            {
                case SignalTableWriterType.Text:
                    writer = new AmpsBoxTimeTableWriterPlain();
                    break;
                default:
                    break;
            }

            return writer;
        }

        #endregion
    }
}