// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReaderFactory.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The reader factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Generic
{
    /// <summary>
    /// TODO The reader factory.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public static class ReaderFactory<T>
        where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The create reader.
        /// </summary>
        /// <param name="type">
        /// TODO The type.
        /// </param>
        /// <returns>
        /// The <see cref="IReader"/>.
        /// </returns>
        /// <exception cref="FactoryTypeNotSpecifiedException">
        /// </exception>
        public static IReader<T> CreateReader(ConfigurationFormatType type)
        {
            IReader<T> reader = null;
            switch (type)
            {
                case ConfigurationFormatType.Xml:
                    reader = new XmlReader<T>();
                    break;
                case ConfigurationFormatType.Json:
                    reader = new JsonReader<T>();
                    break;
                default:
                    throw new FactoryTypeNotSpecifiedException("Cannot generate an object for that type.");
            }

            return reader;
        }

        /// <summary>
        /// TODO The create reader async.
        /// </summary>
        /// <param name="type">
        /// TODO The type.
        /// </param>
        /// <returns>
        /// The <see cref="IReaderAsync"/>.
        /// </returns>
        /// <exception cref="FactoryTypeNotSpecifiedException">
        /// </exception>
        public static IReaderAsync<T> CreateReaderAsync(ConfigurationFormatType type)
        {
            IReaderAsync<T> reader;

            switch (type)
            {
                case ConfigurationFormatType.Xml:
                    reader = new XmlReader<T>();
                    break;
                case ConfigurationFormatType.Json:
                    reader = new JsonReader<T>();
                    break;
                default:
                    throw new FactoryTypeNotSpecifiedException("Cannot generate an object for that type.");
            }

            return reader;
        }

        #endregion
    }
}