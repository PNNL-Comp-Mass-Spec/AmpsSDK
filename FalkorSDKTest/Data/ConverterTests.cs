// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterTests.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The conversion tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDKTest.Data
{
    using System;

    using FalkorSDK.Data;

    using Xunit;

    /// <summary>
    /// TODO The conversion tests.
    /// </summary>
    public class ConversionTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// Tests generic time conversion.
        /// </summary>
        /// <param name="from">
        /// </param>
        /// <param name="to">
        /// </param>
        /// <param name="time">
        /// </param>
        /// <param name="expected">
        /// </param>
        [Fact]
        [InlineData(TimeTableUnits.Microseconds, TimeTableUnits.Milliseconds, 1, .001)]
        [InlineData(TimeTableUnits.Microseconds, TimeTableUnits.Seconds, 1, .000001)]
        [InlineData(TimeTableUnits.Milliseconds, TimeTableUnits.Seconds, 1, .001)]
        [InlineData(TimeTableUnits.Milliseconds, TimeTableUnits.Microseconds, 1, 1000)]
        [InlineData(TimeTableUnits.Seconds, TimeTableUnits.Microseconds, 1, 1000000)]
        [InlineData(TimeTableUnits.Seconds, TimeTableUnits.Milliseconds, 1, 1000)]
        public void TestTime(TimeTableUnits from, TimeTableUnits to, double time, double expected)
        {
            ITimeUnitConverter<double> converter = new SimpleTimeConverter();
            double value = converter.ConvertTo(from, to, time);
            Console.WriteLine("Time: {0} {3}   Converted: {1} {4}   Expected: {2}", time, value, expected, from, to);
            Assert.NotStrictEqual(Math.Abs(expected - value), double.Epsilon);
        }

        #endregion
    }
}