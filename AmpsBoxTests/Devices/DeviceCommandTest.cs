using System;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;

namespace AmpsBoxTests.Devices
{
    using System.Collections;
    using System.IO.Ports;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    using NUnit.Framework;


    [TestFixture]
    class DeviceCommandTest
    {
        private AmpsBox box;


        [Test]
        public async void CommandSocket()
        {

            //Socket socket = new Socket(SocketType.Unknown, ProtocolType.IPv4);
            //socket.Connect(new DnsEndPoint("MIPS"));

            //password 1234
            //name : MIPSNET.LOCAL //lowercase
        }

        [TestFixtureSetUp]
        public void Init()
        {
        //    var serialPort = new SerialPort("COM18", 19200) { Handshake = Handshake.XOnXOff, Parity = Parity.Even};

       //   var reader = new AmpsBoxCommunicator();
            //reader.Port.PortName = "COM3";
            //reader.Port.BaudRate = 19200;
            //reader.Port.RtsEnable = true;
            //reader.Port.ReadTimeout = 1000;
            //reader.Port.Parity = Parity.Even;

         //box = new AmpsBox(reader);
         //   reader.Open();
          var version =  box.StandardModule.GetVersion();
        }

        [Test]
        public void DcBiasTest()
        {
           
        }

        [Test]
        public void DigitalIoTest()
        {
          
        }

        [Test]
        [TestCase(ErrorCodes.Nominal)]
        public void GetVersionTest(ErrorCodes errorCode)
        {
            //var version = box.StandardModule.GetVersion();
            //Console.WriteLine(version);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        [Test]
        [TestCase(ErrorCodes.Nominal)]
        public void GetNameTest(ErrorCodes errorCode)
        {
            //var name = box.StandardModule.GetName();
            //Console.WriteLine(name);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        [Test]
        [TestCase("MIPS", ErrorCodes.Nominal)]
        public void SetNameTest(string name, ErrorCodes errorCode)
        {
            //box.StandardModule.SetName(name);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }



        [Test(Description = "Test which verifys that the command: Get Travelling Wave Pulse Voltage works.")]
        [TestCase(1, ErrorCodes.Nominal)]
        [TestCase(2, ErrorCodes.Nominal)]
        [TestCase(0, ErrorCodes.ArgumentOutOfRange)]
        [TestCase(3, ErrorCodes.ArgumentOutOfRange)]
        public void GetTravellingWavePulseVoltageNominalTest(int boardNumber, ErrorCodes errorCode)
        {
            //box.TWaveModule.GetTravellingWavePulseVoltage(boardNumber);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());

        }

        [Test(Description = "Test which verifys that the command: Set Travelling Wave Pulse Voltage works.")]
        [TestCase(1, 10.0, ErrorCodes.Nominal)]
        [TestCase(2, 10.0, ErrorCodes.Nominal)]
        [TestCase(1, 5.0, ErrorCodes.Nominal)]
        [TestCase(2, 5.0, ErrorCodes.Nominal)]
        public void SetTravellingWavePulseVoltageTest(int boardNumber, double voltage, ErrorCodes errorCode)
        {
            //box.TWaveModule.SetTravellingWavePulseVoltage(boardNumber, voltage);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        [Test]
        [TestCase(1, ErrorCodes.Nominal)]
        [TestCase(2, ErrorCodes.Nominal)]
        public void GetTravellingWaveFrequencyNominalTest(int boardNumber, ErrorCodes errorCode)
        {
            //box.TWaveModule.GetTravellingWaveFrequency(boardNumber);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        [Test]
        [TestCase(1, 5000, ErrorCodes.Nominal)]
        [TestCase(2, 5000, ErrorCodes.Nominal)]
        [TestCase(1, 20000, ErrorCodes.Nominal)]
        [TestCase(2, 20000, ErrorCodes.Nominal)]
        public void SetTravellingWaveFrequencyNominalTest(int boardNumber, int frequency, ErrorCodes errorCode)
        {
            //box.TWaveModule.SetTravellingWaveFrequency(boardNumber, frequency);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        [Test]
        [TestCase(1, 100, ErrorCodes.Nominal)]
        [TestCase(2, 150, ErrorCodes.Nominal)]
        [TestCase(1, 200, ErrorCodes.Nominal)]
        [TestCase(2, 250, ErrorCodes.Nominal)]
        public void SetTravellingWaveGuard1Voltage(int boardNumber, double voltage, ErrorCodes errorCode)
        {
            //box.TWaveModule.SetGuardOneOutputVoltage(boardNumber, voltage);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        [Test]
        [TestCase(1, 100, ErrorCodes.Nominal)]
        [TestCase(2, 150, ErrorCodes.Nominal)]
        [TestCase(1, 200, ErrorCodes.Nominal)]
        [TestCase(2, 250, ErrorCodes.Nominal)]
        public void SetTravellingWaveGuard2Voltage(int boardNumber, double voltage, ErrorCodes errorCode)
        {
            //box.TWaveModule.SetGuardTwoOutputVoltage(boardNumber, voltage);
            //Console.WriteLine(box.Communicator.Response);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        [Test]
        [TestCase(1, ErrorCodes.Nominal)]
        [TestCase(2, ErrorCodes.Nominal)]
        public void GetTravellingWaveGuard1Voltage(int boardNumber, ErrorCodes errorCode)
        {
           //var voltage = box.TWaveModule.GetGuardOneOutputVoltage(boardNumber);
           // Console.WriteLine(voltage);
           // Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        [Test]
        [TestCase(1, ErrorCodes.Nominal)]
        [TestCase(2, ErrorCodes.Nominal)]
        public void GetTravellingWaveGuard2Voltage(int boardNumber, ErrorCodes errorCode)
        {
            //var voltage = box.TWaveModule.GetGuardTwoOutputVoltage(boardNumber);
            //Console.WriteLine(voltage);
            //Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }

        [Test]
        [TestCase(1, ErrorCodes.Nominal)]
        [TestCase(2, ErrorCodes.Nominal)]
        public void SetTravellingWaveSequence(int boardNumber, ErrorCodes errorCode)
        {
            var bitArray = new BitArray(new bool[] { true, true, false, false, true, true, false, false});
            box.TWaveModule.SetTWaveOutputSequence(boardNumber, bitArray);
            Assert.AreEqual(errorCode, box.StandardModule.GetError());
        }
    }
}
