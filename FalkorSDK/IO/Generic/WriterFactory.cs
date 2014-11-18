// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriterFactory.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The writer factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Generic
{
    /// <summary>
    /// TODO The writer factory.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public static class WriterFactory<T>
        where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The create writer.
        /// </summary>
        /// <param name="type">
        /// TODO The type.
        /// </param>
        /// <returns>
        /// The <see cref="IWriter"/>.
        /// </returns>
        /// <exception cref="FactoryTypeNotSpecifiedException">
        /// </exception>
        public static IWriter<T> CreateWriter(ConfigurationFormatType type)
        {
            IWriter<T> writer = null;
            switch (type)
            {
                case ConfigurationFormatType.Xml:
                    writer = new XmlWriter<T>();
                    break;
                case ConfigurationFormatType.Json:
                    writer = new JsonWriter<T>();
                    break;
                default:
                    throw new FactoryTypeNotSpecifiedException("Cannot generate an object for that type.");
            }

            return writer;
        }

        /// <summary>
        /// TODO The create writer async.
        /// </summary>
        /// <param name="type">
        /// TODO The type.
        /// </param>
        /// <returns>
        /// The <see cref="IWriterAsync"/>.
        /// </returns>
        /// <exception cref="FactoryTypeNotSpecifiedException">
        /// </exception>
        public static IWriterAsync<T> CreateWriterAsync(ConfigurationFormatType type)
        {
            IWriterAsync<T> writer = null;
            switch (type)
            {
                case ConfigurationFormatType.Xml:
                    writer = new XmlWriter<T>();
                    break;
                case ConfigurationFormatType.Json:
                    writer = new JsonWriter<T>();
                    break;
                default:
                    throw new FactoryTypeNotSpecifiedException("Cannot generate an object for that type.");
            }

            return writer;
        }

        #endregion
    }
}