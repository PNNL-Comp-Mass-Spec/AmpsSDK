using System;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;
using FalkorSDK.Data.Signals;
using FalkorSDK.Data.Events;

namespace AmpsBoxTests.Devices
{
    using FalkorSDK.Channel;

    using Xunit;

    class DeviceCommandTest
    {
        [Fact]
        public void CommandText(string version)
        {
            AmpsCommandProvider provider = AmpsCommandFactory.CreateCommandProvider(version);

            Console.WriteLine("Testing Commands: {0}", provider.GetSupportedVersions());

            AmpsBox ampsBox  = new AmpsBox();
            ampsBox.Emulated = true;

            ampsBox.AbortTimeTableAsync();
            try
            {
                ampsBox.GetDriveLevel(1);
            }
            catch { }

            try
            {
                ampsBox.GetHvChannelCount();
            }
            catch { }            try
            {
                ampsBox.GetHvOutput(1);
            }
            catch { }
            try
            {
                ampsBox.GetOutputVoltage(1);
            }
            catch { }
            try
            {
                ampsBox.GetRfChannelCount();
            }
            catch { }
            try
            {
                ampsBox.GetRfFrequency(1);
            }
            catch { }
            try
            {
                ampsBox.GetVersion();
            }
            catch { }

            SignalTable table = new SignalTable();
            table.Name    = "dummy";
            table.Length  = 900; 
            double offset     = 10;
            double clockTicks = 60;

            
            ampsBox.SaveParameters();
           

            ampsBox.ClockType       = ClockType.Internal;            
            ampsBox.TriggerType = StartTriggerTypes.EXT;
           
            ampsBox.TriggerType = StartTriggerTypes.SW;
          
            ampsBox.AbortTimeTableAsync();

            ampsBox.ClockType       = ClockType.External;
            ampsBox.ClockFrequency  = 10;
            
            ampsBox.AbortTimeTableAsync();
        }
    }
}
