// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsBox.cs" company="">
//   
// </copyright>
// <summary>
//   Communicates with a PNNL Amps Box
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Reactive;
using System.Reactive.Linq;
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

        private IAmpsBoxCommunicator communicator;

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

        public IObservable<Unit> SetDcBiasVoltage(string channel, int volts)
        {
            Command command = new AmpsCommand("SDCB", "SDCB");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", volts);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<int> GetDcBiasSetpoint(string channel)
        {
            Command command = new AmpsCommand("GDCB", "GDCB");
            command = command.AddParameter(",", channel);

            int dcBiasSetpoint = 0;
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int.TryParse(s, out dcBiasSetpoint);
                return dcBiasSetpoint;
            });
        }

        public IObservable<int> GetDcBiasReadback(string channel)
        {
            Command command = new AmpsCommand("GDCBV", "GDCBV");
            command = command.AddParameter(",", channel);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int dcBiasReadback = 0;
                int.TryParse(s, out dcBiasReadback);
                return dcBiasReadback;
            });

        }

        public IObservable<int> GetDcBiasCurrentReadback(string channel)
        {
            Command command = new AmpsCommand("GDCBI", "GDCBI");
            command = command.AddParameter(",", channel);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int dcBiasCurrentReadback = 0;
                int.TryParse(s, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });

        }

        public IObservable<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber, int offsetVolts)
        {
            Command command = new AmpsCommand("SDCBOF", "SDCBOF");
            command = command.AddParameter(",", brdNumber).AddParameter(",", offsetVolts);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }

        public IObservable<int> GetBoardDcBiasOffsetVoltage(int brdNumber)
        {
            Command command = new AmpsCommand("GDCBOF", "GDCBOF");
            command = command.AddParameter(",", brdNumber);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int dcBiasCurrentReadback = 0;
                int.TryParse(s, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });


        }

        public IObservable<int> GetNumberDcBiasChannels()
        {
            Command command = new AmpsCommand("GCHAN", "GCHAN");
            command = command.AddParameter(",", "DCB");
            this.communicator.Write(command);

            return this.communicator.MessageSources.Select(bytes =>
            {
                var response = Encoding.ASCII.GetString(bytes.ToArray());
                int dcBiasCurrentReadback = 0;
                int.TryParse(response, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });

        }

        public IObservable<Unit> SetDigitalState(string channel, bool state)
        {
            Command command = new AmpsCommand("SDIO", "SDIO");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", state);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<Unit> PulseDigitalSignal(string channel)
        {
            Command command = new AmpsCommand("SDIO", "SDIO");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", "P");

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<bool> GetDigitalState(string channel)
        {
            Command command = new AmpsCommand("GDIO", "GDIO");
            command.AddParameter(",", channel);
            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                bool digitalState;
                bool.TryParse(s, out digitalState);
                return digitalState;
            });
        }

        public IObservable<Unit> SetDigitalDirection(string channel, DigitalDirection digitalDirection)
        {
            Command command = new AmpsCommand("SDIODR", "SDIODR");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", digitalDirection.ToString());

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<DigitalDirection> GetDigitalDirection(string channel)
        {
            Command command = new AmpsCommand("GDIODR", "GDIODR");
            command = command.AddParameter(",", channel);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                var direction = (DigitalDirection)Enum.Parse(typeof(DigitalDirection), s);
                return direction;
            });
        }

        public IObservable<int> GetNumberDigitalChannels()
        {
            Command command = new AmpsCommand("GCHAN", "GCHAN");
            command.AddParameter(",", "DIO");
            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int numberOfChannels = 0;
                int.TryParse(s, out numberOfChannels);
                return numberOfChannels;
            });
        }

        public IObservable<Unit> SetPositiveHighVoltage(int volts)
        {
            Command command = new AmpsCommand("SPHV", "SPHV");
            command = command.AddParameter(",", volts);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<Unit> SetNegativeHighVoltage(int volts)
        {
            Command command = new AmpsCommand("SNHV", "SNHV");
            command = command.AddParameter(",", volts);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<Tuple<double, double>> GetPositiveEsi()
        {
            Command command = new AmpsCommand("GPHVV", "GPHVV");

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes =>
            {
                var voltageAndCurrent = Encoding.ASCII.GetString(bytes.ToArray());
               var splitString = voltageAndCurrent.Split(new[] {','});
                double voltage = 0;
                double current = 0;
                if (splitString.Length >= 2)
                {
                    double.TryParse(splitString[0], out voltage);
                    double.TryParse(splitString[1], out current);
                }
                
                return new Tuple<double, double>(voltage, current);
            });
        }

        public IObservable<Tuple<double, double>> GetNegativeEsi()
        {
            Command command = new AmpsCommand("GNHVV", "GNHVV");

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes =>
            {
                var voltageAndCurrent = Encoding.ASCII.GetString(bytes.ToArray());
                var splitString = voltageAndCurrent.Split(new[] { ',' });
                double voltage = 0;
                double current = 0;
                if (splitString.Length >= 2)
                {
                    double.TryParse(splitString[0], out voltage);
                    double.TryParse(splitString[1], out current);
                }

                return new Tuple<double, double>(voltage, current);
            });
        }

        public IObservable<Unit> TurnOnHeater()
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> TurnOffHeater()
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> SetTemperatureSetpoint(int temperature)
        {
            throw new NotImplementedException();
        }

        public IObservable<int> ReadTemperature()
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> SetPidGain(int gain)
        {
            throw new NotImplementedException();
        }

        public IObservable<string> GetVersion()
        {
            Command command = new AmpsCommand("GVER", "GVER");
            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            var stream = messagePacket.Select(bytes => Encoding.ASCII.GetString(bytes.ToArray()));
            return stream;
        }

        public IObservable<ErrorCodes> GetError()
        {
            Command command = new AmpsCommand("GERR", "GERR");

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                return (ErrorCodes)Enum.Parse(typeof(ErrorCodes), s);
            });
        }

        public IObservable<string> GetName()
        {
            Command command = new AmpsCommand("GNAME", "GNAME");
            this.communicator.Write(command);

            return this.communicator.MessageSources.Select(bytes => Encoding.ASCII.GetString(bytes.ToArray()));
        }

        public IObservable<Unit> SetName(string name)
        {
            Command command = new AmpsCommand("SNAME", "SNAME");
            command.AddParameter(",", name);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(x => Unit.Default);
        }

        public IObservable<Unit> Reset()
        {
            throw new NotImplementedException("This is a dangerous function!");
            Command command = new AmpsCommand("RESET", "RESET");
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(x => Unit.Default);
        }

        public IObservable<Unit> Save()
        {
            Command command = new AmpsCommand("SAVE", "SAVE");
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(x => Unit.Default);
        }

        public IObservable<string> GetCommands()
        {
            Command command = new AmpsCommand("GCMDS", "GCMDS");

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(x => Encoding.ASCII.GetString(x.ToArray()));
        }

        public IObservable<Unit> SetSerialBaudRate(int baudRate)
        {
            Command command = new AmpsCommand("SBAUD", "SBAUD");
            command.AddParameter(",", baudRate);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(x => Unit.Default);
        }

        public IObservable<Unit> SetFrequency(string address, int frequency)
        {
            // TODO: figure out if all values of frequency are already in kHz
            // if(frequency < 500 || frequency > 5000)
            Command command = new AmpsCommand("SRFFRQ", "SRFFRQ");
            command = command.AddParameter(",", address);
            command = command.AddParameter(",", frequency);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<int> GetFrequencySetting(string address)
        {
            Command command = new AmpsCommand("GRFFRQ", "GRFFRQ");
            command = command.AddParameter(",", address);

            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int frequency = 0;
                int.TryParse(s, out frequency);
                return frequency;
            });
        }

        public IObservable<Unit> SetRfDriveSetting(string address, int drive)
        {
            if (drive < 0 || drive > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(drive), "Range must be between 0 and 255");
            }
            Command command = new AmpsCommand("SRFDRV", "SRFDRV");
            command = command.AddParameter(",", address);
            command = command.AddParameter(",", address);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<int> GetRfDriveSetting(string address)
        {
            Command command = new AmpsCommand("GRFDRV", "GRFDRV");
            command = command.AddParameter(",", address);

            int dcBiasSetpoint = 0;
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int.TryParse(s, out dcBiasSetpoint);
                return dcBiasSetpoint;
            });
        }

        public IObservable<int> GetRfChannelNumber()
        {
            Command command = new AmpsCommand("GCHAN", "GCHAN");
            command = command.AddParameter(",", "RF");

            int dcBiasSetpoint = 0;
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int.TryParse(s, out dcBiasSetpoint);
                return dcBiasSetpoint;
            });
        }

        public IObservable<Unit> AbortTimeTable()
        {
            Command command = new AmpsCommand("TBLABRT", "TBLABRT");
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }

        /// <summary>
        /// Starts execution of time table.
        /// </summary>
        /// <returns></returns>
        public IObservable<Unit> StartTimeTable()
        {
            Command command = new AmpsCommand("TBLSTRT", "TBLSTRT");
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }

        public string LastTable { get; private set; }

        /// <summary>
        /// Sets the table mode for the amps / mips box.
        /// </summary>
        public IObservable<Unit> SetMode(Modes mode)
        {
            Command command = new AmpsCommand("SMOD", "SMOD");
            command.AddParameter(",", mode.ToString());
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }
        /// <summary>
        /// Stop the time table of the device.
        /// </summary>
        /// <returns></returns>
        public IObservable<Unit> StopTable()
        {
            Command command = new AmpsCommand("TBLSTOP", "TBLSTOP");
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }

        /// <summary>
        /// Loads the Table onto the device
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public IObservable<Unit> LoadTimeTable(AmpsSignalTable table)
        {
            string formattedTable = table.RetrieveTableAsEncodedString();
            this.LastTable = formattedTable;
            Command command = new AmpsCommand("STBLDAT", formattedTable);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }

        /// <summary>
        /// Tells the AMPS box which clock to use: external or internal
        /// </summary>
        /// <param name="clockType">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public IObservable<Unit> SetClock(ClockType clockType)
        {
            Command command = new AmpsCommand("STBLCLK", "STBLCLK");
            command = command.AddParameter(",", clockType.ToString());
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }

        /// <summary>
        /// Set the device trigger.
        /// </summary>
        /// <param name="startTriggerType"></param>
        /// <returns></returns>
        public IObservable<Unit> SetTrigger(StartTriggerTypes startTriggerType)
        {
            Command command = new AmpsCommand("STBLCLK", "STBLCLK");
            command = command.AddParameter(",", startTriggerType.ToString());
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }

        #endregion
    }
}