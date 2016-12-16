﻿using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AmpsBoxSdk.Commands;
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
        public void DcBiasTest()
        {
            var subscription = box.GetDcBiasSetpoint("1").Timestamp().Subscribe(timestamped =>
            {
                output.WriteLine(timestamped.Value.ToString());
                output.WriteLine(timestamped.Timestamp.LocalDateTime.ToString());
            });
            Thread.Sleep(500);
        }

        [Fact]
        public void DigitalIoTest()
        {
          
        }

        [Theory]
        [InlineData(ErrorCodes.Nominal)]
        public void GetVersionTest(ErrorCodes errorCode)
        {
            output.WriteLine(DateTimeOffset.Now.LocalDateTime.ToString());
            var subscription = box.GetVersion().Timestamp().Subscribe(timestamped =>
            {
                output.WriteLine(timestamped.Value);
                output.WriteLine(timestamped.Timestamp.LocalDateTime.ToString());
            });
            Thread.Sleep(500);
        }

        [Theory]
        [InlineData(ErrorCodes.Nominal)]
        public void GetNameTest(ErrorCodes errorCode)
        {
            output.WriteLine(DateTimeOffset.Now.LocalDateTime.ToString());
            var version = box.GetName().Timestamp().Subscribe(timestamped =>
            {
                output.WriteLine(timestamped.Value);
                output.WriteLine(timestamped.Timestamp.LocalDateTime.ToString());
            });
            Thread.Sleep(500);

        }

        [Theory]
        [InlineData("AMPS-Groot", ErrorCodes.Nominal)]
        public void SetNameTest(string name, ErrorCodes errorCode)
        {
           
            output.WriteLine(DateTimeOffset.Now.LocalDateTime.ToString());
           var finished = box.SetName(name).Timestamp().Wait();
            output.WriteLine(finished.Timestamp.LocalDateTime.ToString());
        }

        [Fact]
        public void GetCommandsTest()
        {
            output.WriteLine(DateTimeOffset.Now.LocalDateTime.ToString());
            var version = box.GetCommands();
            int count = 0;
            version.Subscribe(s =>
            {
               output.WriteLine(s);
                count++;
            });
            System.Threading.Thread.Sleep(1000);
            output.WriteLine(count.ToString());
        }

        [Fact]
        public void GetPositiveEsiVoltageAndCurrent()
        {
            var esi = box.GetPositiveEsi();
            int count = 0;
            esi.Subscribe(s =>
            {
                output.WriteLine(s.Item1.ToString());
                output.WriteLine(s.Item2.ToString());
            });
            System.Threading.Thread.Sleep(500);
            output.WriteLine(count.ToString());
        }

        public void Dispose()
        {
            this.reader?.Close();
        }
    }
}
