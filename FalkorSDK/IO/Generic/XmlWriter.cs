// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlWriter.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The xml writer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Generic
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    /// <summary>
    /// TODO The xml writer.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class XmlWriter<T> : IWriter<T>, IWriterAsync<T>
        where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The write.
        /// </summary>
        /// <param name="fileName">
        /// TODO The file name.
        /// </param>
        /// <param name="data">
        /// TODO The data.
        /// </param>
        public void Write(string fileName, T data)
        {
            using (var writer = new StreamWriter(fileName))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, data);
            }
        }

        /// <summary>
        /// TODO The write async.
        /// </summary>
        /// <param name="fileName">
        /// TODO The file name.
        /// </param>
        /// <param name="data">
        /// TODO The data.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task WriteAsync(string fileName, T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(fileName))
            {
                await Task.Run(() => serializer.Serialize(writer, data));
            }
        }

        #endregion
    }
}