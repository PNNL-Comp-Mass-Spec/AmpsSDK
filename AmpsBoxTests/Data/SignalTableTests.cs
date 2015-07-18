namespace AmpsBoxTests.Data
{
    using System;
    using System.IO.Ports;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using AmpsBoxSdk.Commands;
    using AmpsBoxSdk.Data;
    using AmpsBoxSdk.Devices;

    using FalkorSDK.Channel;
    using FalkorSDK.Data;
    using FalkorSDK.Data.Elements;
    using FalkorSDK.Data.Signals;
    using FalkorSDK.IO.Ports;

    using NUnit.Framework;

    [TestFixture]
    public class SignalTableTests
    {
        [Test]
        public void SignalTableTest()
        {
          


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
            var falkorSerialPort = new FalkorSerialPort(new SerialPort("COM18") { BaudRate = 19200, Handshake = Handshake.XOnXOff, DataBits = 8, Parity = Parity.Even, Encoding = Encoding.ASCII, StopBits = StopBits.One });
            AmpsBox box = new AmpsBox(new AmpsBoxCOMReader(falkorSerialPort), new AmpsCOMCommandFormatter());
            box.Communicator.Open();
            var version = box.GetVersionAsync().Result;
         Console.WriteLine(version);
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