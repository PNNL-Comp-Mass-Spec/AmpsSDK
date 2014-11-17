using System;
using FalkorSDK.Data;
using NUnit.Framework;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;

namespace AmpsBoxTests.Data
{
    [TestFixture]
    public class ConversionTests
    {
        
        /// <summary>
        /// Tests conversion to the internal clock for the AMPS Box
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="time"></param>
        /// <param name="expected"></param>
        [Test]
        [TestCase(TimeTableUnits.Microseconds, TimeTableUnits.Milliseconds, 1, .001)]
        [TestCase(TimeTableUnits.Microseconds, TimeTableUnits.Seconds, 1, .000001)]
        [TestCase(TimeTableUnits.Milliseconds, TimeTableUnits.Seconds, 1, .001)]
        [TestCase(TimeTableUnits.Milliseconds, TimeTableUnits.Microseconds, 1, 1000)]
        [TestCase(TimeTableUnits.Seconds, TimeTableUnits.Microseconds, 1, 1000000)]
        [TestCase(TimeTableUnits.Seconds, TimeTableUnits.Milliseconds, 1, 1000)]
        [TestCase(TimeTableUnits.Microseconds, TimeTableUnits.Ticks, 64, 1)]
        [TestCase(TimeTableUnits.Milliseconds, TimeTableUnits.Ticks, .064, 1)]
        [TestCase(TimeTableUnits.Seconds, TimeTableUnits.Ticks, .000064, 1)]
        public void TestAmpsClockConversion(TimeTableUnits from, TimeTableUnits to, double time, double expected)
        {
            ITimeUnitConverter<double> converter = new AmpsClockConverter(AmpsCommandProvider.DEFAULT_INTERNAL_CLOCK);
            double value                         = converter.ConvertTo(from, to, time);
            Console.WriteLine("Time: {0} {3}   Converted: {1} {4}   Expected: {2}",     time,
                                                                                        value,
                                                                                        expected,
                                                                                        from,
                                                                                        to);
            Assert.LessOrEqual(Math.Abs(expected - value), double.Epsilon);
        }
    }
}
