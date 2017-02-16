// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsBox.cs" company="">
//   
// </copyright>
// <summary>
//   Communicates with a PNNL Amps Box
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Modules;

namespace AmpsBoxSdk.Devices
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using AmpsBoxSdk.Data;
    /// <summary>
    /// Communicates with a PNNL Amps Box
    /// Non shared parts creation policy so that multiple amps boxes can exist in the system at once.
    /// </summary>
    [DataContract]
    public sealed class AmpsBox : IAmpsBox, IStandardModule, IPulseSequenceGeneratorModule, IDcBiasModule, IDioModule, IRfDriverModule, IEsiModule, IHeaterModule
    {
        #region Constants

        private readonly IAmpsBoxCommunicator communicator;

        /// <summary>
        /// 
        /// </summary>
        public AmpsBox(IAmpsBoxCommunicator communicator)
        {
            if (communicator == null)
            {
                throw new ArgumentNullException(nameof(communicator));
            }
            this.communicator = communicator;
            this.ClockFrequency = 16000000;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the clock frequency of the AMPS Box.
        /// </summary>   
        [DataMember]
        public int ClockFrequency { get; set; }

        [DataMember]
        public string Name { get; set; }
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns a string representation of the current software configuration. 
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetConfig()
        {
            string ampsBoxData  = string.Empty;
            ampsBoxData         += string.Format("\tDevice Settings\n");

            ampsBoxData += "\n";
            ampsBoxData += string.Format("\tTable Settings\n");
            ampsBoxData += $"\t\tExt. Clock Freq: {this.ClockFrequency}\n";

            return ampsBoxData;
        }

        public async Task<Unit> SetDcBiasVoltage(string channel, int volts)
        {
            Command command = new Command("SDCB", "SDCB");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", volts);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<int> GetDcBiasSetpoint(string channel)
        {
            Command command = new Command("GDCB", "GDCB");
            command = command.AddParameter(",", channel);

            int dcBiasSetpoint = 0;
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(s =>
            {
                int.TryParse(s, out dcBiasSetpoint);
                return dcBiasSetpoint;
            }).FirstAsync();
        }

        public async Task<int> GetDcBiasReadback(string channel)
        {
            Command command = new Command("GDCBV", "GDCBV");
            command = command.AddParameter(",", channel);
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(s =>
            {
                int dcBiasReadback = 0;
                int.TryParse(s, out dcBiasReadback);
                return dcBiasReadback;
            }).FirstAsync();
        }

        public async Task<int> GetDcBiasCurrentReadback(string channel)
        {
            Command command = new Command("GDCBI", "GDCBI");
            command = command.AddParameter(",", channel);
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(s =>
            {
                int dcBiasCurrentReadback = 0;
                int.TryParse(s, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            }).FirstAsync();

        }

        public async Task<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber, int offsetVolts)
        {
            Command command = new Command("SDCBOF", "SDCBOF");
            command = command.AddParameter(",", brdNumber).AddParameter(",", offsetVolts);
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<int> GetBoardDcBiasOffsetVoltage(int brdNumber)
        {
            Command command = new Command("GDCBOF", "GDCBOF");
            command = command.AddParameter(",", brdNumber);
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(s =>
            {
                int dcBiasCurrentReadback = 0;
                int.TryParse(s, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            }).FirstAsync();


        }

        public async Task<int> GetNumberDcBiasChannels()
        {
            Command command = new Command("GCHAN", "GCHAN");
            command = command.AddParameter(",", "DCB");
            this.communicator.Write(command);

            return await this.communicator.MessageSources.Select(s =>
            {
                int dcBiasCurrentReadback = 0;
                int.TryParse(s, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            }).FirstAsync();

        }

        public async Task<Unit> SetDigitalState(string channel, bool state)
        {
            Command command = new Command("SDIO", "SDIO");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", Convert.ToInt32(state));

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(bytes => Unit.Default).FirstAsync().Timeout(TimeSpan.FromMilliseconds(500)).ToTask();
        }

        public async Task<Unit> PulseDigitalSignal(string channel)
        {
            Command command = new Command("SDIO", "SDIO");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", "P");

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<bool> GetDigitalState(string channel)
        {
            Command command = new Command("GDIO", "GDIO");
            command = command.AddParameter(",", channel);
            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(s =>
            {
                bool digitalState;
                bool.TryParse(s, out digitalState);
                return digitalState;
            }).FirstAsync();
        }

        public async Task<Unit> SetDigitalDirection(string channel, DigitalDirection digitalDirection)
        {
            Command command = new Command("SDIODR", "SDIODR");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", digitalDirection.ToString());

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<DigitalDirection> GetDigitalDirection(string channel)
        {
            Command command = new Command("GDIODR", "GDIODR");
            command = command.AddParameter(",", channel);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(s =>
            {
                DigitalDirection direction;
                var success = Enum.TryParse(s, true, out direction);
                if (success) return direction;
                else return DigitalDirection.OUT;
            }).FirstAsync();
        }

        public async Task<int> GetNumberDigitalChannels()
        {
            Command command = new Command("GCHAN", "GCHAN");
            command = command.AddParameter(",", "DIO");
            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(s =>
            {
                if (string.IsNullOrEmpty(s))
                {
                    return 0;
                }
                int numberOfChannels = 0;
                int.TryParse(s, out numberOfChannels);
                return numberOfChannels;
            }).FirstAsync();
        }

        public async Task<Unit> SetPositiveHighVoltage(int volts)
        {
            Command command = new Command("SPHV", "SPHV");
            command = command.AddParameter(",", volts);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<Unit> SetNegativeHighVoltage(int volts)
        {
            Command command = new Command("SNHV", "SNHV");
            command = command.AddParameter(",", volts);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<(double Voltage, double Current)> GetPositiveEsi()
        {
            Command command = new Command("GPHVV", "GPHVV");

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(s =>
            {
               var splitString = s.Split(new[] {','});
                double voltage = 0;
                double current = 0;
                if (splitString.Length >= 2)
                {
                    double.TryParse(splitString[0], out voltage);
                    double.TryParse(splitString[1], out current);
                }

                return (voltage, current);
            }).FirstAsync();
        }

        public async Task<(double Voltage, double Current)> GetNegativeEsi()
        {
            Command command = new Command("GNHVV", "GNHVV");

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(s =>
            {
                var splitString = s.Split(new[] { ',' });
                double voltage = 0;
                double current = 0;
                if (splitString.Length >= 2)
                {
                    double.TryParse(splitString[0], out voltage);
                    double.TryParse(splitString[1], out current);
                }

                return (voltage, current);
            }).FirstAsync();
        }

        public Task<Unit> TurnOnHeater()
        {
            throw new NotImplementedException();
        }

        public Task<Unit> TurnOffHeater()
        {
            throw new NotImplementedException();
        }

        public Task<Unit> SetTemperatureSetpoint(int temperature)
        {
            throw new NotImplementedException();
        }

        public Task<int> ReadTemperature()
        {
            throw new NotImplementedException();
        }

        public Task<Unit> SetPidGain(int gain)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetVersion()
        {
            Command command = new Command("GVER", "GVER");
            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.FirstAsync().ToTask();
        }

        public async Task<ErrorCodes> GetError()
        {
            Command command = new Command("GERR", "GERR");

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(s =>
            {
                int error = 0;
                int.TryParse(s, out error);
                return (ErrorCodes) error;
            }).FirstAsync();
        }

        public async Task<string> GetName()
        {
            Command command = new Command("GNAME", "GNAME");
            this.communicator.Write(command);

            return await this.communicator.MessageSources.FirstAsync().ToTask();
        }

        public async Task<Unit> SetName(string name)
        {
            Command command = new Command("SNAME", "SNAME");
            command.AddParameter(",", name);
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<Unit> Reset()
        {
            throw new NotImplementedException("This is a dangerous function!");
            Command command = new Command("RESET", "RESET");
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(x => Unit.Default).FirstAsync();
        }

        public async Task<Unit> Save()
        {
            Command command = new Command("SAVE", "SAVE");
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(x => Unit.Default).FirstAsync();
        }

        public async Task<IEnumerable<string>> GetCommands()
        {
            Command command = new Command("GCMDS", "GCMDS");

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
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
            Command command = new Command("SBAUD", "SBAUD");
            command = command.AddParameter(",", baudRate);
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(x => Unit.Default).FirstAsync();
        }

        public async Task<Unit> SetFrequency(string address, int frequency)
        {
            // TODO: figure out if all values of frequency are already in kHz
            // if(frequency < 500 || frequency > 5000)
            Command command = new Command("SRFFRQ", "SRFFRQ");
            command = command.AddParameter(",", address);
            command = command.AddParameter(",", frequency);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<int> GetFrequencySetting(string address)
        {
            Command command = new Command("GRFFRQ", "GRFFRQ");
            command = command.AddParameter(",", address);

            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(s =>
            {
                int frequency = 0;
                int.TryParse(s, out frequency);
                return frequency;
            }).FirstAsync();
        }

        public async Task<Unit> SetRfDriveSetting(string address, int drive)
        {
            if (drive < 0 || drive > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(drive), "Range must be between 0 and 255");
            }
            Command command = new Command("SRFDRV", "SRFDRV");
            command = command.AddParameter(",", address);
            command = command.AddParameter(",", address);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
        }

        public async Task<int> GetRfDriveSetting(string address)
        {
            Command command = new Command("GRFDRV", "GRFDRV");
            command = command.AddParameter(",", address);

            int dcBiasSetpoint = 0;
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(s =>
            {
                int.TryParse(s, out dcBiasSetpoint);
                return dcBiasSetpoint;
            }).FirstAsync();
        }

        public async Task<int> GetRfChannelNumber()
        {
            Command command = new Command("GCHAN", "GCHAN");
            command = command.AddParameter(",", "RF");

            int dcBiasSetpoint = 0;
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(s =>
            {
                int.TryParse(s, out dcBiasSetpoint);
                return dcBiasSetpoint;
            }).FirstAsync();
        }

        public async Task<Unit> AbortTimeTable()
        {
            Command command = new Command("TBLABRT", "TBLABRT");
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(bytes => Unit.Default).FirstAsync();
        }

        /// <summary>
        /// Starts execution of time table.
        /// </summary>
        /// <returns></returns>
        public async Task<Unit> StartTimeTable()
        {
            Command command = new Command("TBLSTRT", "TBLSTRT");
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(bytes => Unit.Default).FirstAsync();
        }

        public string LastTable { get; private set; }

        /// <summary>
        /// Sets the table mode for the amps / mips box.
        /// </summary>
        public async Task<Unit> SetMode(Modes mode)
        {
            Command command = new Command("SMOD", "SMOD");
            command = command.AddParameter(",", mode.ToString());
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(bytes => Unit.Default).FirstAsync();
        }
        /// <summary>
        /// Stop the time table of the device.
        /// </summary>
        /// <returns></returns>
        public async Task<Unit> StopTable()
        {
            Command command = new Command("TBLSTOP", "TBLSTOP");
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(bytes => Unit.Default).FirstAsync();
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
            string formattedTable = table.RetrieveTableAsEncodedString();
            this.LastTable = formattedTable;
            Command command = new Command("STBLDAT",$"STBLDAT;{formattedTable};");
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(bytes => Unit.Default).FirstAsync().ToTask();
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
            Command command = new Command("STBLCLK", "STBLCLK");
            command = command.AddParameter(",", clockType.ToString());
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(bytes => Unit.Default).FirstAsync();
        }

        /// <summary>
        /// Set the device trigger.
        /// </summary>
        /// <param name="startTriggerType"></param>
        /// <returns></returns>
        public async Task<Unit> SetTrigger(StartTriggerTypes startTriggerType)
        {
            Command command = new Command("STBLTRG", "STBLTRG");
            command = command.AddParameter(",", startTriggerType.ToString());
            this.communicator.Write(command);
            return await this.communicator.MessageSources.Select(bytes => Unit.Default).FirstAsync();
        }

        public IAmpsBoxCommunicator Communicator
        {
            get { return this.communicator; }
        }

        #endregion
    }
}