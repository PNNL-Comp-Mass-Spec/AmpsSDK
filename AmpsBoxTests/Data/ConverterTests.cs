using System;
using FalkorSDK.Data;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;

namespace AmpsBoxTests.Data
{
    using Xunit;

    public class ConversionTests
    {
        
        /// <summary>
        /// Tests conversion to the internal clock for the AMPS Box
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="time"></param>
        /// <param name="expected"></param>
        [Fact]
        public void TestAmpsClockConversion(TimeTableUnits from, TimeTableUnits to, double time, double expected)
        {
           
        }
    }
}
