using System;
using NUnit.Framework;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;
using FalkorSDK.Data.Signals;
using FalkorSDK.Data.Events;

namespace AmpsBoxTests.Devices
{
    [TestFixture]
    class DeviceCommandTest
    {
        [Test]
        [TestCase("v1.6")]
        public void CommandText(string version)
        {
            AmpsCommandProvider provider = AmpsCommandFactory.CreateCommandProvider(version);

            Console.WriteLine("Testing Commands: {0}", provider.GetSupportedVersions());

            AmpsBox ampsBox  = new AmpsBox();
            ampsBox.Emulated = true;

            ampsBox.AbortTimeTable();
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

            for(int j = 0; j < 4; j++)
            {
                Signal signal = new AnalogOutputSignal(j.ToString(), 0, j);
                for(int i = 0; i < 10; i++)
                {
                    SignalEvent signalEvent = new AnalogStepEvent(signal, Convert.ToDouble(i) * clockTicks, Convert.ToDouble(i * offset));
                    table.Add(signalEvent);
                }
                offset += 10;
            }
            
            ampsBox.SaveParameters();
            ampsBox.SetHvOutput(1, 100);
            ampsBox.SetRfDriveLevel(1, 1500);
            ampsBox.SetRfFrequency(1, 1500);
            ampsBox.SetRfOutputVoltage(1, 1500);

            ampsBox.ClockType       = ClockType.Internal;            
            ampsBox.TriggerType = StartTriggerTypes.External;
            ampsBox.StartTimeTable(table, 1);

            ampsBox.TriggerType = StartTriggerTypes.Software;
            ampsBox.StartTimeTable(table, 0);
            ampsBox.AbortTimeTable();

            ampsBox.ClockType       = ClockType.External;
            ampsBox.ClockFrequency  = 10;
            ampsBox.StartTimeTable(table, 0);
            ampsBox.AbortTimeTable();
        }
    }
}
