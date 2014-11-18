// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteTests.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The example.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDKTest.IO
{
    using FalkorSDK.IO.Generic;

    using Xunit;

    /// <summary>
    /// TODO The example.
    /// </summary>
    public class Example
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        public int Y { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// TODO The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return "X " + this.X + " Y " + this.Y;
        }

        #endregion
    }

    /// <summary>
    /// TODO The write tests.
    /// </summary>
    public class WriteTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The test write.
        /// </summary>
        /// <param name="type">
        /// TODO The type.
        /// </param>
        /// <param name="path">
        /// TODO The path.
        /// </param>
        [Fact]
        public void TestWrite(ConfigurationFormatType type, string path)
        {
            // 	IAnalogToDigitalConverterProperties example = new Example();
            // 	var writer = WriterFactory<Example>.CreateWriter(type);
            // 	writer.WriteDigitalOutputAsync(path, example);
        }

        /// <summary>
        /// TODO The test write async.
        /// </summary>
        /// <param name="type">
        /// TODO The type.
        /// </param>
        /// <param name="path">
        /// TODO The path.
        /// </param>
        [Fact]
        public async void TestWriteAsync(ConfigurationFormatType type, string path)
        {
            var ex = new Example { X = 10, Y = 10 };
            var writer = WriterFactory<Example>.CreateWriterAsync(type);
            await writer.WriteAsync(path, ex);
        }

        #endregion
    }
}