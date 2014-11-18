// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryWriter.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the BinaryWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.IO.Generic
{
    using System;

    /// <summary>
    /// TODO The binary writer.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class BinaryWriter<T> : IWriter<T>
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
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Write(string fileName, T data)
        {
            throw new NotImplementedException();
        }
    }
}