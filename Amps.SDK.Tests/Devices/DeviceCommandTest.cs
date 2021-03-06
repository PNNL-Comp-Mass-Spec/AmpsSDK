﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using RJCP.IO.Ports;
using Xunit;
using Xunit.Abstractions;
using Amps;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Data;
using AmpsBoxSdk.Devices;

namespace Amps.SDK.Tests.Devices
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
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }
           
            box = AmpsBoxFactory.CreateAmpsBox(serialPort);
        }

        [Fact]
        public async void AmpsSignalTableTest()
        {
            

            box.ModeReady.Subscribe(unit =>
            {
                output.WriteLine("mode");
            });
            var signalTable = new AmpsSignalTable();
            signalTable = signalTable.AddTimePoint(0, new LoopData());
            signalTable[0].CreateOutput("1", 10);
            signalTable= signalTable.AddTimePoint(1000, new LoopData());
            signalTable[1000].CreateOutput("1", 5);

            signalTable.Points.LastOrDefault()?.ReferenceTimePoint(signalTable.Points.FirstOrDefault(), 100);
            var encodedString = signalTable.RetrieveTableAsEncodedString();
            output.WriteLine(encodedString);

            box.LoadTimeTable(signalTable).Wait();
            var ampsError = box.GetError().Result;
            output.WriteLine(ampsError.ToString());
            box.SetClock(ClockType.INT).Wait();
            ampsError = box.GetError().Result;
            output.WriteLine(ampsError.ToString());
            box.SetTrigger(StartTrigger.SW).Wait();
            ampsError = box.GetError().Result;
            output.WriteLine(ampsError.ToString());
            box.SetMode(Modes.TBL).Wait();
            var error = box.GetError().Result;
            output.WriteLine(error.ToString());
            box.StartTimeTable().Wait();
            error = box.GetError().Result;
            output.WriteLine(error.ToString());
            var status = box.ReportExecutionStatus().Result;
            output.WriteLine(status);

            box.StopTable().Wait();
            box.AbortTimeTable().Wait();
            box.TableCompleteOrAborted.FirstAsync().Wait();
            Thread.Sleep(1000);
        }

        [Fact]
        public void GetAmpsNameProperty()
        {
            var name = box.Name;
            output.WriteLine(name);
        }

		[Fact]
        public void GetDescriptorTest()
        {
            foreach (var portDescription in SerialPortStream.GetPortDescriptions())
            {
                output.WriteLine(portDescription.Description);
            }
        }

        [Fact]
        public void GetPositiveEsiStatusTest()
        {
           var status = box.GetPositiveEsi().Result;
            output.WriteLine("volts: {0}\ncurrent: {1}", status.Item1, status.Item2);
        }

        [Fact]
        public void GetNegativeEsiStatusTest()
        {
            var status = box.GetNegativeEsi().Result;
            output.WriteLine("volts: {0}\ncurrent: {1}", status.Item1, status.Item2);
        }

        [Fact]
        public void SetPositiveEsiTest()
        {
            var status = box.SetPositiveHighVoltage(50).Result;
        }

        [Fact]
        public void SetNegativeEsiTest()
        {
            var status = box.SetNegativeHighVoltage(50).Result;
        }

        [Fact]
        public void GetDcBiasCurrentReadback()
        {
            var status = box.GetDcBiasCurrentReadback(1).Result;
            output.WriteLine("current: {0}", status);
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
        public void GetDigitalDirectionTest()
        {
            var config = box.GetAmpsConfigurationAsync().Result;
            for (int i = 0; i < config.NumberDigitalChannels; i++)
            {
               var direction = box.GetDigitalDirection(config.GetDioChannel((uint) i)).Result;
                output.WriteLine(direction.ToString());
            }
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
            //var error = box.GetError().Result;
            //output.WriteLine(error.ToString());
        }
        [Theory]
        [InlineData(ErrorCodes.Nominal)]
        public void GetVersionTest(ErrorCodes errorCode)
        {
            var version = box.GetVersion().Result;
            output.WriteLine(version);
            var error = box.GetError().Result;
            output.WriteLine(error.ToString());
        }

        [Fact]
        public void GetConfigTest()
        {
            var config = box.GetAmpsConfigurationAsync().Result;
           output.WriteLine("HV: {0}\nDIO: {1}\nRF: {2}", config.NumberHvChannels, config.NumberDigitalChannels, config.NumberRfChannels);
        }

        [Theory]
        [InlineData(ErrorCodes.Nominal)]
        public void GetNameTest(ErrorCodes errorCode)
        {
            var version = box.GetName().Result;
            output.WriteLine(version);
        }

        [Theory]
        [InlineData("AMPS-Groot", ErrorCodes.Nominal)]
        public void SetNameTest(string name, ErrorCodes errorCode)
        {
            
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
        public void GetNumberOfDigitalChannelsTest()
        {
            var digitalChannels = box.GetNumberDigitalChannels().Result;
            output.WriteLine(digitalChannels.ToString());
        }

        public void Dispose()
        {
            serialPort.Dispose();
        }
    }
}
