// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataContractReader.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DataContractReader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.IO.Generic
{
    using System;

    /// <summary>
    /// TODO The data contract reader.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class DataContractReader<T> : IReader<T>
        where T : class
    {
        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public T Read(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}