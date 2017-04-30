// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsBox.cs" company="">
//   
// </copyright>
// <summary>
//   Communicates with a PNNL Amps Box
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Io;
using AmpsBoxSdk.Modules;

namespace AmpsBoxSdk.Devices
{
    using System;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using Data;
    /// <summary>
    /// Communicates with a PNNL Amps Box
    /// Non shared parts creation policy so that multiple amps boxes can exist in the system at once.
    /// </summary>
    [DataContract]
    internal sealed class AmpsBox : IAmpsBox
    {
        private readonly AmpsBoxCommunicator communicator;

        private Lazy<AmpsBoxDeviceData> deviceData;


        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public AmpsBox(AmpsBoxCommunicator communicator)
        {
            this.communicator = communicator ?? throw new ArgumentNullException(nameof(communicator));
            this.communicator.Open();
            
            ClockFrequency = 16000000;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the clock frequency of the AMPS Box.
        /// </summary>   
        [DataMember]
        public int ClockFrequency { get; set; }

        [DataMember]
        public string Name => GetName().Result;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns a string representation of the current software configuration. 
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public async Task<AmpsBoxDeviceData> GetAmpsConfigurationAsync()
        {
            var dcBiasChannels = await this.GetNumberDcBiasChannels();
            var rfChannels = await this.GetNumberRfChannels();
            var digitalChannels = await this.GetNumberDigitalChannels();
            this.deviceData = new Lazy<AmpsBoxDeviceData>(() => new AmpsBoxDeviceData((uint)dcBiasChannels, (uint)rfChannels, (uint)digitalChannels));
			return this.deviceData.Value;
		}

        public async Task<Unit> SetDcBiasVoltage(int channel, int volts)
        {
            var ampsmessage = Message.Create(AmpsCommand.SDCB, channel.ToString(), volts.ToString());
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<int> GetDcBiasSetpoint(int channel)
        {
            var ampsmessage = Message.Create(AmpsCommand.GDCB, channel);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                int.TryParse(bytes, out int setPoint);
                return setPoint;
            }).FirstAsync();
        }

        public async Task<int> GetDcBiasReadback(int channel)
        {
            var ampsmessage = Message.Create(AmpsCommand.GDCBV, channel);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                int.TryParse(bytes, out int voltageReadback);
                return voltageReadback;
            }).FirstAsync();
        }

        public async Task<int> GetDcBiasCurrentReadback(int channel)
        {
            var ampsmessage = Message.Create(AmpsCommand.GDCBI, channel);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                int.TryParse(bytes, out int currentReadback);
                return currentReadback;
            }).FirstAsync();
        }

        public async Task<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber, int offsetVolts)
        {
            var ampsmessage = Message.Create(AmpsCommand.SDCBOF, brdNumber, offsetVolts);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<int> GetBoardDcBiasOffsetVoltage(int brdNumber)
        {
            var ampsmessage = Message.Create(AmpsCommand.SDCBOF, brdNumber);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                int.TryParse(bytes, out int offsetVoltage);
                return offsetVoltage;
            }).FirstAsync();
        }

        public async Task<int> GetNumberDcBiasChannels()
        {
            var ampsmessage = Message.Create(AmpsCommand.GCHAN, Module.DCB.ToString());
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                int.TryParse(bytes, out int channels);
                return channels;
            }).FirstAsync();

        }

        public async Task<Unit> SetDigitalState(string channel, bool state)
        {
            var messagePacket = communicator.MessageSources;
            var stateAsInt = Convert.ToInt32(state).ToString();
            var ampsmessage = Message.Create(AmpsCommand.SDIO, channel, stateAsInt);
            ampsmessage.WriteTo(communicator);

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<Unit> PulseDigitalSignal(string channel)
        {
            var ampsmessage = Message.Create(AmpsCommand.SDIO, channel, "P");
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<bool> GetDigitalState(string channel)
        {
            var ampsmessage = Message.Create(AmpsCommand.GDIO, channel);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                if (string.IsNullOrEmpty(bytes))
                {
                    return false;
                }
                var result = Convert.ToBoolean(Convert.ToInt16(bytes));
                return result;

            }).FirstAsync();
        }

        /// <summary>
        /// Changes I/O direction bit on specified channel.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="digitalDirection"></param>
        /// <returns></returns>
        public async Task<Unit> SetDigitalDirection(string channel, DigitalDirection digitalDirection)
        {
            switch (channel)
            {
                case "E":
                case "F":
                case "G":
                case "H":
                    var ampsmessage = Message.Create(AmpsCommand.SDIODR, channel, digitalDirection.ToString());
                    ampsmessage.WriteTo(communicator);
                    var messagePacket = communicator.MessageSources;

                    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
                default:
                    return Unit.Default;
            }
            
        }

        public async Task<DigitalDirection> GetDigitalDirection(string channel)
        {
            var ampsmessage = Message.Create(AmpsCommand.GDIODR, channel);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                Enum.TryParse(bytes, true, out DigitalDirection result);
                return result;
            }).FirstAsync();
        }

        public async Task<int> GetNumberDigitalChannels()
        {
            var ampsmessage = Message.Create(AmpsCommand.GCHAN, Module.DIO.ToString());
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                int.TryParse(bytes, out int channels);
                return channels;
            }).FirstAsync();
        }

        public async Task<Unit> SetPositiveHighVoltage(int volts)
        {
            var ampsmessage = Message.Create(AmpsCommand.SPHV, volts);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<Unit> SetNegativeHighVoltage(int volts)
        {
            var ampsmessage = Message.Create(AmpsCommand.SNHV, volts);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<(double, double)> GetPositiveEsi()
        {
            var ampsmessage = Message.Create(AmpsCommand.GPHVV);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                var values = bytes.Split(',');
                if (values.Length < 2)
                {
                    return (0, 0);
                }
                int.TryParse(values[0], out int voltage);
                int.TryParse(values[1], out int current);

                return (voltage, current);
            }).FirstAsync();
        }

        public async Task<(double, double)> GetNegativeEsi()
        {
            var ampsmessage = Message.Create(AmpsCommand.GNHVV);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                var values = bytes.Split(',');
                if (values.Length < 2)
                {
                    return (0, 0);
                }
                int.TryParse(values[0], out int voltage);
                int.TryParse(values[1], out int current);

                return (voltage, current);
            }).FirstAsync();
        }

        public async Task<Unit> TurnOnHeater()
        {
            var ampsmessage = Message.Create(AmpsCommand.SHTR, State.ON.ToString());
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<Unit> TurnOffHeater()
        {
            var ampsmessage = Message.Create(AmpsCommand.SHTR, State.OFF.ToString());
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<Unit> SetTemperatureSetpoint(int temperature)
        {
            var ampsmessage = Message.Create(AmpsCommand.SHTRTMP, temperature);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<int> ReadTemperature()
        {
            var ampsmessage = Message.Create(AmpsCommand.GHTRTC);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                int.TryParse(bytes, out int temperature);
                return temperature;
            }).FirstAsync();
        }

        public async Task<Unit> SetPidGain(int gain)
        {
            var ampsmessage = Message.Create(AmpsCommand.SHTRGAIN, gain);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<string> GetVersion()
        {
            var ampsmessage = Message.Create(AmpsCommand.GVER);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => bytes).FirstAsync();
        }

        public async Task<ErrorCodes> GetError()
        {
            var ampsmessage = Message.Create(AmpsCommand.GERR);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                Enum.TryParse(bytes, true, out ErrorCodes result);
                return result;
            }).FirstAsync();
        }

		public async Task<string> GetName()
		{
			var ampsmessage = Message.Create(AmpsCommand.GNAME);
			ampsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => bytes).FirstAsync();
		}

		public async Task<Unit> SetName(string name)
        {
            var ampsmessage = Message.Create(AmpsCommand.SNAME, name);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
    }

        public async Task<Unit> Reset()
        {
            var ampsmessage = Message.Create(AmpsCommand.RESET);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<Unit> Save()
        {
            var ampsmessage = Message.Create(AmpsCommand.SAVE);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<IEnumerable<string>> GetCommands()
        {
            var ampsmessage = Message.Create(AmpsCommand.GCMDS);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;
            var aggregator = await messagePacket.Select(s => s).Scan(new List<string>(),
                    (list, s) =>
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            list.Add(s);
                        }

                        return list;
                    })
                .Where(list => list.Count == 51).FirstAsync(); // hardcoded hack
            return aggregator;
        }

        public async Task<Unit> SetSerialBaudRate(int baudRate)
        {
            throw new NotImplementedException();
        }

        public async Task<Unit> SetFrequency(int address, int frequency)
        {
            var ampsmessage = Message.Create(AmpsCommand.SRFFRQ, address.ToString(), frequency.ToString());
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<int> GetFrequencySetting(int address)
        {
            var ampsmessage = Message.Create(AmpsCommand.GRFFRQ, address);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                int.TryParse(bytes, out int frequencySetting);
                return frequencySetting;
            }).FirstAsync();
        }

        public async Task<Unit> SetRfDriveSetting(int address, int drive)
        {
            var ampsmessage = Message.Create(AmpsCommand.SRFDRV, address.ToString(), drive.ToString());
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<int> GetRfDriveSetting(int address)
        {
            var ampsmessage = Message.Create(AmpsCommand.GRFFRQ, address);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                int.TryParse(bytes, out int rfDriveSetting);
                return rfDriveSetting;
            }).FirstAsync();
        }

        public async Task<int> GetNumberRfChannels()
        {
            var ampsmessage = Message.Create(AmpsCommand.GCHAN, Module.RF.ToString());
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                int.TryParse(bytes, out int channels);
                return channels;
            }).FirstAsync();
        }

        public async Task<Unit> AbortTimeTable()
        {
            var ampsmessage = Message.Create(AmpsCommand.TBLABRT);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        /// <summary>
        /// Starts execution of time table.
        /// </summary>
        /// <returns></returns>
        public async Task<Unit> StartTimeTable()
        {
            var ampsmessage = Message.Create(AmpsCommand.TBLSTRT);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public string LastTable { get; private set; }
        public async Task<string> ReportExecutionStatus()
        {
            var ampsmessage = Message.Create(AmpsCommand.TBLRPT);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes =>
            {
                return bytes;
            }).FirstAsync();
        }

        /// <summary>
        /// Sets the table mode for the amps / mips box.
        /// </summary>
        public async Task<Unit> SetMode(Modes mode)
        {
            var ampsmessage = Message.Create(AmpsCommand.SMOD, mode.ToString());
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }
        /// <summary>
        /// Stop the time table of the device.
        /// </summary>
        /// <returns></returns>
        public async Task<Unit> StopTable()
        {
            var ampsmessage = Message.Create(AmpsCommand.TBLSTOP);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        /// <summary>
        /// Loads the Table onto the device
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Unit> LoadTimeTable(AmpsSignalTable table)
        {
            var ampsmessage = Message.Create(AmpsCommand.STBLDAT, table);
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        /// <summary>
        /// Tells the AMPS box which clock to use: external or internal
        /// </summary>
        /// <param name="clockType">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Unit> SetClock(ClockType clockType)
        {
            var ampsmessage = Message.Create(AmpsCommand.STBLCLK, clockType.ToString());
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        /// <summary>
        /// Set the device trigger.
        /// </summary>
        /// <param name="startTrigger"></param>
        /// <returns></returns>
        public async Task<Unit> SetTrigger(StartTrigger startTrigger)
        {
            var ampsmessage = Message.Create(AmpsCommand.STBLTRG, startTrigger.ToString());
            ampsmessage.WriteTo(communicator);
            var messagePacket = communicator.MessageSources;

            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        #endregion
    }
}