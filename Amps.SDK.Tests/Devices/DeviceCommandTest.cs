using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Data;
using AmpsBoxSdk.Devices;
using AmpsBoxSdk.Io;
using AmpsBoxSdk.Modules;
using RJCP.IO.Ports;
using Xunit;
using Xunit.Abstractions;

namespace AmpsBoxTests.Devices
{
   public class DeviceCommandTest : IDisposable
    {
        private IAmpsBox box;
        private ITestOutputHelper output;
        private SerialPortStream serialPort;
        public DeviceCommandTest(ITestOutputHelper output)
        {
            this.output = output;
            serialPort = new SerialPortStream("COM3", 19200 * 2, 8, Parity.Even, StopBits.One) {RtsEnable = false, Handshake = Handshake.XOn};

            box = AmpsBoxFactory.CreateAmpsBox(serialPort);
        }

        [Fact]
        public void AmpsSignalTableTest()
        {
            var signalTable = new AmpsSignalTable();
            signalTable = signalTable.AddTimePoint(0, new LoopData());
            signalTable[0].CreateOutput("1", 10);
           signalTable= signalTable.AddTimePoint(10, new LoopData());
            signalTable[10].CreateOutput("1", 5);

            signalTable.Points.LastOrDefault()?.ReferenceTimePoint(signalTable.Points.FirstOrDefault(), 10);
            var encodedString = signalTable.RetrieveTableAsEncodedString();
            output.WriteLine(encodedString);

            box.LoadTimeTable(signalTable).Wait();
            var error = box.GetError().Result;
            output.WriteLine(error.ToString());
            box.SetClock(ClockType.INT).Wait();
            error = box.GetError().Result;
            output.WriteLine(error.ToString());
            box.SetTrigger(StartTriggerTypes.SW).Wait();
            error = box.GetError().Result;
            output.WriteLine(error.ToString());
            box.SetMode(Modes.TBL).Wait();
            error = box.GetError().Result;
            output.WriteLine(error.ToString());
            box.StartTimeTable().Wait();
            error = box.GetError().Result;
            output.WriteLine(error.ToString());
        }

        [Fact]
        public void GetDcBiasTest()
        {
            var subscription = box.GetDcBiasSetpoint(1).Result;
            var readback = box.GetDcBiasReadback(1).Result;
            output.WriteLine(subscription.ToString());
            output.WriteLine(readback.ToString());
            var error = box.GetError().Result;
            output.WriteLine(error.ToString());
        }

        [Fact]
        public void SetDcBiasTest()
        {
            int setpointVoltage = 10;
            var subscription = box.SetDcBiasVoltage(1, setpointVoltage).Result;
            var readback = box.GetDcBiasSetpoint(1).Result;
            Assert.Equal(setpointVoltage, readback);
        }
         
        [Theory]
        [InlineData("A")]
        public void GetDigitalStateTest(string channel)
        {
           
            var result = box.GetDigitalState(channel).Result;
            output.WriteLine(result.ToString());
           var error = box.GetError().Result;
            output.WriteLine(error.ToString());
        }

        [Theory]
        [InlineData("A", true)]
        [InlineData("A", false)]
        public void SetDigitalStateTest(string channel, bool state)
        {
            var unit = box.SetDigitalState(channel, state).Result;
            var error = box.GetError().Result;
            output.WriteLine(error.ToString());
        }
        [Theory]
        [InlineData(ErrorCodes.Nominal)]
        public void GetVersionTest(ErrorCodes errorCode)
        {
            var version = box.GetVersion().Result;
            output.WriteLine(version);
            var error = box.GetError().Result;
            Assert.Equal(error, ErrorCodes.Nominal);
        }

        [Theory]
        [InlineData(ErrorCodes.Nominal)]
        public void GetNameTest(ErrorCodes errorCode)
        {
            output.WriteLine(DateTimeOffset.Now.LocalDateTime.ToString());
            var version = box.GetName().Result;
            output.WriteLine(version);
            Thread.Sleep(500);
        }

        [Theory]
        [InlineData("AMPS-Groot", ErrorCodes.Nominal)]
        public void SetNameTest(string name, ErrorCodes errorCode)
        {
           
            output.WriteLine(DateTimeOffset.Now.LocalDateTime.ToString());
           
           
        }

        [Fact]
        public void GetCommandsTest()
        {
            var timestamp = Stopwatch.GetTimestamp();
            var commands = box.GetCommands().Result;

            var timestamp2 = Stopwatch.GetTimestamp();
            output.WriteLine(timestamp.ToString());
            output.WriteLine(commands.Count().ToString());
            foreach (var command in commands)
            {
                output.WriteLine(command);
            }
           
            output.WriteLine(timestamp2.ToString());
            var difference = timestamp2 - timestamp;
            output.WriteLine(difference.ToString());
           var time = 1.0/Stopwatch.Frequency * difference;
            output.WriteLine(time.ToString());
        }

        [Fact]
        public void GetNumberDcBias()
        {
            var numberDcBias = box.GetNumberDcBiasChannels().Result;
            output.WriteLine(numberDcBias.ToString());
        }

        [Fact]
        public void GetPositiveEsiVoltageAndCurrent()
        {
            var esi = box.GetPositiveEsi().Result;
            output.WriteLine(esi.ToString());
        }

        [Fact]
        public void GetNumberOfDigitalChannelsTest()
        {
            var digitalChannels = box.GetNumberDigitalChannels().Result;
            output.WriteLine(digitalChannels.ToString());
        }

        [Fact]
        public void GetDeviceDataTest()
        {
            output.WriteLine("{0},{1},{2}", box.DeviceData.NumberHvChannels, box.DeviceData.NumberRfChannels, box.DeviceData.NumberDigitalChannels);
        }

        public void Dispose()
        {
            serialPort.Dispose();
        }
    }
}
