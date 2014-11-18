// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlReader.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The xml reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Generic
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    /// <summary>
    /// TODO The xml reader.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class XmlReader<T> : IReader<T>, IReaderAsync<T>
        where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The read.
        /// </summary>
        /// <param name="fileName">
        /// TODO The file name.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Read(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                var serializer = new XmlSerializer(typeof(T));
                var data = serializer.Deserialize(reader) as T;
                return data;
            }
        }

        /// <summary>
        /// TODO The read async.
        /// </summary>
        /// <param name="fileName">
        /// TODO The file name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<T> ReadAsync(string fileName)
        {
            var serializer = new XmlSerializer(typeof(T));
            T result;

            using (var reader = new StreamReader(fileName))
            {
                result = await Task.Run(() => serializer.Deserialize(reader) as T);
            }

            return result;
        }

        #endregion
    }
}