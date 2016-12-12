namespace AmpsBoxTests.Data
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;
    using System.Windows.Media;

    using AmpsBoxSdk.Commands;
    using AmpsBoxSdk.Data;
    using AmpsBoxSdk.Devices;

    using AMPSFalkorPlugin;
    using AMPSFalkorPlugin.Domain.Model;
    using AMPSFalkorPlugin.Services;

    using FalkorSDK.Channel;
    using FalkorSDK.Channel.Calibration;
    using FalkorSDK.Data.MassSpectrometry;
    using FalkorSDK.Data.Signals;
    using FalkorSDK.IO.Ports;
    using NUnit.Framework;

    [TestFixture]
    public class SignalTableTests
    {
        [Test]
        public void SignalTableTest()
        {
            var signalTable = new FalkorSDK.Data.Signals.AmpsSignalTable();
            var point = new FalkorSDK.Data.Signals.PsgPoint("1", 100, new FalkorSDK.Data.Signals.LoopData());
            var point2 = new FalkorSDK.Data.Signals.PsgPoint("2", 150, new FalkorSDK.Data.Signals.LoopData() );

            var point3 = new FalkorSDK.Data.Signals.PsgPoint("3", 175, new FalkorSDK.Data.Signals.LoopData() );

            point.CreateOutput(new ChannelAddress("4"), 0);
            point.CreateOutput(new ChannelAddress("A"), true );
            point2.CreateOutput(new ChannelAddress("3"), 0 );
            point3.CreateOutput(new ChannelAddress("4"), 40 );
            point3.CreateOutput(new ChannelAddress("A"), false);
            signalTable.AddTimePoint(point);
            signalTable.AddTimePoint(point2);
            signalTable.AddTimePoint(point3);
            var clockConverter = new AmpsClockConverter(100000);
            var table = signalTable.FormatTable();
            Console.WriteLine(table);
        }

        [Test]
        public void Test1()
        {
          Console.WriteLine((int)'A');
        }

        [Test]
        public void NestedSignalTableTest()
        {
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

        [Test]
        public void AmpsCommunicationSignalTableTest()
        {
        }



        private string data;
       
        [Test]
        public async void AmpsCommunicationTest()
        {
         //   var falkorSerialPort = new FalkorSerialPort(new SerialPort("COM18") { BaudRate = 19200, Handshake = Handshake.XOnXOff, DataBits = 8, Parity = Parity.Even, Encoding = Encoding.ASCII, StopBits = StopBits.One });
         //   AmpsBox box = new AmpsBox(new AmpsBoxCOMReader(new FalkorSerialPort()), new AmpsCOMCommandFormatter());
         //   //box.Communicator.Open();
         //   var version = box.GetVersionAsync().Result;
         //Console.WriteLine(version);
        }

  

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var port = (SerialPort)sender;
            data = port.ReadExisting();
            Console.WriteLine(data);
        }

        public class ArduinoControllerMain
        {

        }
    }


}