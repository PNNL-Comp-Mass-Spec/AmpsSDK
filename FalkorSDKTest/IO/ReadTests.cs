// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadTests.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The read tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDKTest.IO
{
    using System;

    using FalkorSDK.IO.Generic;

    using Xunit;

    /// <summary>
    /// TODO The read tests.
    /// </summary>

    public class ReadTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The crap test.
        /// </summary>
        /// <param name="x">
        /// TODO The x.
        /// </param>
        
       [Fact]
        public void CrapTest(int x)
        {
            Assert.Equal(x, 9);
        }

        /// <summary>
        /// TODO The read.
        /// </summary>
        [Theory]
        public async void Read()
        {
            var reader = ReaderFactory<Example>.CreateReaderAsync(ConfigurationFormatType.Json);

            var task = await reader.ReadAsync("test.json");
            Console.WriteLine(task.ToString());
        }

        #endregion
    }
}