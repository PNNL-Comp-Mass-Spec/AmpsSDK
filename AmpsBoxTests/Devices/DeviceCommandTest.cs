using System;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;
using Falkor.Plugin.Amps.Device;
using Xunit;

namespace AmpsBoxTests.Devices
{
    using System.Collections;
    using System.IO.Ports;
   public class DeviceCommandTest
    {
        private AmpsBox box;

        public DeviceCommandTest()
        {
            var serialPort = new SerialPort("COM3", 19200) { Handshake = Handshake.XOnXOff, Parity = Parity.Even };

            var reader = new AmpsBoxCommunicator(serialPort);

            box = new AmpsBox(reader);
           reader.Open();
            var version = box.StandardModule.GetVersion();
            version.Subscribe(s => Console.WriteLine(s));
        }

        [Fact]
        public void DcBiasTest()
        {
           
        }

        [Fact]
        public void DigitalIoTest()
        {
          
        }

        [Theory]
        [InlineData(ErrorCodes.Nominal)]
        public void GetVersionTest(ErrorCodes errorCode)
        {
            //var version = box.StandardModule.GetVersion();
            //Console.WriteLine(version);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        [Theory]
        [InlineData(ErrorCodes.Nominal)]
        public void GetNameTest(ErrorCodes errorCode)
        {
            //var name = box.StandardModule.GetName();
            //Console.WriteLine(name);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        [Theory]
        [InlineData("AMPS-Groot", ErrorCodes.Nominal)]
        public void SetNameTest(string name, ErrorCodes errorCode)
        {
            //box.StandardModule.SetName(name);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        public void GetCommandsTest()
        {
            
        }
        
    }
}
