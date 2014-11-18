// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataContractWriter.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DataContractWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.IO.Generic
{
    using System.IO;
    using System.Runtime.Serialization;

    /// <summary>
    /// TODO The data contract writer.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class DataContractWriter<T> : IWriter<T>
        where T : class
    {
        /// <summary>
        /// The write.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public void Write(string fileName, T data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(fileName))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    serializer.WriteObject(memoryStream, typeof(T));
                }
            }
        }
    }
}