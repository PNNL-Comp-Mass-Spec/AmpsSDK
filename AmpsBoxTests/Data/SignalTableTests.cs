namespace AmpsBoxTests.Data
{
    using System;
    using System.IO.Ports;
    using System.Linq;
    using System.Text;

    using AmpsBoxSdk.Commands;
    using AmpsBoxSdk.Data;
    using AmpsBoxSdk.Devices;

    using FalkorSDK.Channel;
    using FalkorSDK.Data;
    using FalkorSDK.Data.Events;
    using FalkorSDK.Data.Signals;
    using FalkorSDK.IO.Ports;

    using NUnit.Framework;

    [TestFixture]
    public class SignalTableTests
    {
        [Test]
        public void SignalTableTest()
        {
            SignalTable table = new SignalTable(new Waveform(new ChannelAddress("1")).AddSignalElement(new AnalogStepElement(new ChannelAddress("1"), new Voltage(0.5))));
            table.UpdateExecutionData(new SignalTableExecutionData("test", "", 1, 150, TimeUnits.Microseconds));

          var formater = new AmpsBoxSignalTableCommandFormatter();
          var data =  formater.FormatTable(table, new AmpsClockConverter(16000));

        }

        [Test]
        public void NestedSignalTableTest()
        {
            SignalTable table = new SignalTable(new Waveform(new ChannelAddress("1")).AddSignalElement(new AnalogStepElement(new ChannelAddress("1"), new Voltage(5), 1500)));
            table.UpdateExecutionData(new SignalTableExecutionData("test", "", 1, 0.0, TimeUnits.Microseconds));
            table.Children.Add(
                new SignalTable(
                    new Waveform(new ChannelAddress("2")).AddSignalElement(
                        new AnalogStepElement(new ChannelAddress("2"), new Voltage(5), 1000))).UpdateExecutionData(new SignalTableExecutionData("test1", "desc", 2, 500, TimeUnits.Microseconds)));

            table.Children.First().Children.Add(
               new SignalTable(
                   new Waveform(new ChannelAddress("3")).AddSignalElement(
                       new AnalogStepElement(new ChannelAddress("3"), new Voltage(5), 1000))).UpdateExecutionData(new SignalTableExecutionData("test3", "desc1", 3, 600, TimeUnits.Microseconds)));


            var formater = new AmpsBoxSignalTableCommandFormatter();
            var data = formater.FormatTable(table, new AmpsClockConverter(16000000));
            Console.WriteLine(data);

            //var falkorSerialPort = new FalkorSerialPort(new SerialPort("COM18") { BaudRate = 19200, Handshake = Handshake.XOnXOff, DataBits = 8, Parity = Parity.Even, Encoding = Encoding.ASCII, StopBits = StopBits.One });
            //AmpsBox box = new AmpsBox(new AmpsBoxCOMReader(falkorSerialPort), new AmpsCOMCommandFormatter());
            //box.Communicator.Open();
            //var version = box.GetVersionAsync();
            //version.Wait();
            //var v = version.Result;

            //var response = box.LoadTimeTableAsync(table);

            //response.Wait();
            //var r = response.Result;
            //var error = box.GetError().Result;
            //Console.WriteLine(error);
        }
    }
}