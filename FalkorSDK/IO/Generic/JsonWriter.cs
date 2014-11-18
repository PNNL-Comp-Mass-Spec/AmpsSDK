// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonWriter.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The json writer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Generic
{
    using System.IO;
    using System.Runtime.Serialization.Formatters;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    /// <summary>
    /// TODO The json writer.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class JsonWriter<T> : IWriter<T>, IWriterAsync<T>
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
            using (var writer = File.CreateText(fileName))
            {
                var jWriter = new JsonTextWriter(writer) { Formatting = Formatting.Indented };

                var serializer = new JsonSerializer
                                     {
                                         PreserveReferencesHandling = PreserveReferencesHandling.All, 
                                         TypeNameAssemblyFormat = FormatterAssemblyStyle.Full, 
                                         TypeNameHandling = TypeNameHandling.All
                                     };
                serializer.Serialize(jWriter, data);
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
            using (var writer = File.CreateText(fileName))
            {
                var jWriter = new JsonTextWriter(writer) { Formatting = Formatting.Indented };

                var serializer = new JsonSerializer
                                     {
                                         PreserveReferencesHandling = PreserveReferencesHandling.All, 
                                         TypeNameAssemblyFormat = FormatterAssemblyStyle.Full, 
                                         TypeNameHandling = TypeNameHandling.All
                                     };
                serializer.Serialize(jWriter, data);
                await Task.Run(() => serializer.Serialize(jWriter, data));
            }
        }

        #endregion
    }
}