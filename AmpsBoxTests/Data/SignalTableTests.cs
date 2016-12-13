using Xunit;

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

    public class SignalTableTests
    {
       [Fact]
        public void SignalTableTest()
        {
        }

        [Fact]
        public void Test1()
        {
          Console.WriteLine((int)'A');
        }

        [Fact]
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

        [Fact]
        public void AmpsCommunicationSignalTableTest()
        {
        }

        [Fact]
        public async void AmpsCommunicationTest()
        {
         //   var falkorSerialPort = new FalkorSerialPort(new SerialPort("COM18") { BaudRate = 19200, Handshake = Handshake.XOnXOff, DataBits = 8, Parity = Parity.Even, Encoding = Encoding.ASCII, StopBits = StopBits.One });
         //   AmpsBox box = new AmpsBox(new AmpsBoxCOMReader(new FalkorSerialPort()), new AmpsCOMCommandFormatter());
         //   //box.Communicator.Open();
         //   var version = box.GetVersionAsync().Result;
         //Console.WriteLine(version);
        }
    }


}