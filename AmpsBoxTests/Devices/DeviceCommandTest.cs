using System;
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
using Xunit;
using Xunit.Abstractions;

namespace AmpsBoxTests.Devices
{
    using System.Collections;
    using System.IO.Ports;
   public class DeviceCommandTest : IDisposable
    {
        private AmpsBox box;
        private AmpsBoxCommunicator reader;
        private ITestOutputHelper output;

        public DeviceCommandTest(ITestOutputHelper output)
        {
            this.output = output;
            var serialPort = new SerialPort("COM3", 19200*2) { Handshake = Handshake.XOnXOff, Parity = Parity.Even };
            serialPort.RtsEnable = true;

            reader = new AmpsBoxCommunicator(serialPort);
            box = new AmpsBox(reader);
            reader.Open();
            
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
            box.SetClock(ClockType.INT).Wait();
            box.SetTrigger(StartTriggerTypes.SW).Wait();
            box.SetMode(Modes.TBL).Wait();
            var error = box.GetError().Result;
            output.WriteLine(error.ToString());
            box.StartTimeTable().Wait();
            box.AbortTimeTable().Wait();
            box.SetMode(Modes.LOC).Wait();

        }

        [Fact]
        public void DcBiasTest()
        {
          //  box.SetDcBiasVoltage("1", 10).Wait();
            var subscription = box.GetDcBiasSetpoint("1").Result;
            var readback = box.GetDcBiasReadback("1").Result;
            output.WriteLine(subscription.ToString());
            output.WriteLine(readback.ToString());
        }

        [Fact]
        public void DigitalIoTest()
        {
          
        }
        [Theory]
        [InlineData(ErrorCodes.Nominal)]
        public void GetVersionTest(ErrorCodes errorCode)
        {
            var version = box.GetVersion().Result;
            output.WriteLine(version);
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
            output.WriteLine(DateTimeOffset.Now.LocalDateTime.ToString());
            var commands = box.GetCommands().Result;
            output.WriteLine(commands.Count().ToString());
            foreach (var command in commands)
            {
                output.WriteLine(command);
            }
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

        public void Dispose()
        {
            this.reader?.Close();
        }
    }
}
