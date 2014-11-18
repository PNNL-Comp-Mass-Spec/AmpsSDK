// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonReader.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The json reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Generic
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    /// <summary>
    /// TODO The json reader.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class JsonReader<T> : IReader<T>, IReaderAsync<T>
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
            try
            {
                using (var reader = File.OpenText(fileName))
                {
                    var jReader = new JsonTextReader(reader);

                    var serializer = new JsonSerializer
                                         {
                                             MissingMemberHandling = MissingMemberHandling.Ignore, 
                                             NullValueHandling = NullValueHandling.Ignore, 
                                             PreserveReferencesHandling = PreserveReferencesHandling.All, 
                                             TypeNameAssemblyFormat = FormatterAssemblyStyle.Full, 
                                             TypeNameHandling = TypeNameHandling.All, 
                                             ObjectCreationHandling = ObjectCreationHandling.Auto
                                         };

                    // TODO: Handle when there is an error with loading a device.
                    var data = serializer.Deserialize(jReader, typeof(T)) as T;
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Accepts filename and returns Task of type T where T is class.
        /// </summary>
        /// <param name="fileName">
        /// File name (full / path)
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<T> ReadAsync(string fileName)
        {
            T result;
            using (var file = File.OpenText(fileName))
            {
                var serializer = new JsonSerializer();
                result = await Task.Run(() => serializer.Deserialize(file, typeof(T)) as T);
            }

            return result;
        }

        #endregion
    }
}