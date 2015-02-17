// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsBox.cs" company="">
//   
// </copyright>
// <summary>
//   Communicates with a PNNL Amps Box
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Devices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Ports;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    using AmpsBoxSdk.Commands;
    using AmpsBoxSdk.Data;

    using FalkorSDK.Data.Signals;
    using FalkorSDK.IO.Ports;
    using FalkorSDK.IO.Signals;

    using ReactiveUI;

    using Timer = System.Timers.Timer;

    /// <summary>
    /// Communicates with a PNNL Amps Box
    /// Non shared parts creation policy so that multiple amps boxes can exist in the system at once.
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [DataContract]
    public sealed class AmpsBox : ReactiveObject
    {
        #region Constants

        /// <summary>
        /// TODO The default_ box_ version.
        /// </summary>
        private const string ConstDefaultBoxVersion = "v2.0b";

        /// <summary>
        /// read timeout in milliseconds
        /// </summary>
        private const int ConstReadTimeout = 5000;

        /// <summary>
        /// Default sleep time between writes / reads
        /// </summary>
        private const int ConstSleepTime = 1000;

        /// <summary>
        /// TODO The cons t_ writ e_ timeout.
        /// </summary>
        private const int ConstWriteTimeout = 5000;

        /// <summary>
        /// Emulated channel count for RF and HV testing
        /// </summary>
        private const int EmulatedChannelCount = 8;

        /// <summary>
        /// TODO The emulate d_ output.
        /// </summary>
        private const int EmulatedOutput = 100;

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the command provider for the given box in case versions are different.
        /// </summary>
        private readonly AmpsCommandProvider commandProvider;

        /// <summary>
        /// Synchronization object.
        /// </summary>
        private readonly object sync;

        /// <summary>
        /// Firmware of the box.
        /// </summary>
        private string boxVersion;

        /// <summary>
        /// Serial Port 
        /// </summary>
        private FalkorSerialPort falkorPort;

        /// <summary>
        /// Last Table executed.
        /// </summary>
        private string lastTable;

        private int clockFrequency;

        private ClockType clockType;

        private StartTriggerTypes triggerType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsBox"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="falkorSerialPort">
        /// The falkor Serial Port.
        /// </param>
        [ImportingConstructor]
        public AmpsBox()
        {
            this.boxVersion = ConstDefaultBoxVersion;
            this.sync = new object();
            this.ReadTimeout = ConstReadTimeout;
            this.ClockType = ClockType.Internal;
            this.TriggerType = StartTriggerTypes.SW;
            this.Emulated = false;
            this.commandProvider = AmpsCommandFactory.CreateCommandProvider(this.boxVersion);
            this.ClockFrequency = this.commandProvider.InternalClock;
            this.ReadWriteTimeout = ConstSleepTime;
            this.WhenAnyValue(x => x.Port.Port).Subscribe(this.OnNext);
          
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the clock frequency of the AMPS Box.
        /// </summary>   
        [DataMember]
        public int ClockFrequency
        {
            get
            {
                return this.clockFrequency;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.clockFrequency, value);
            }
        }

        /// <summary>
        /// Gets or sets the clock type for executing a time Table.
        /// </summary>
        [DataMember]
        public ClockType ClockType
        {
            get
            {
                return this.clockType;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.clockType, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is emulated or not.
        /// </summary>
        [DataMember]
        public bool Emulated { get; set; }

        /// <summary>
        /// Gets or sets the ID of this device.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets the serial port
        /// </summary>
        public FalkorSerialPort Port
        {
            get
            {
                return this.falkorPort;
            }

            set
            {
                lock (this.sync)
                {
                    this.falkorPort = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of time to wait for a number of characters.
        /// </summary>		
        [DataMember]
        public int ReadTimeout { get; set; }

        /// <summary>
        /// Gets or sets the read write timeout between IO calls on the serial port
        /// </summary>
        [DataMember]
        public int ReadWriteTimeout { get; set; }

        /// <summary>
        /// Gets or sets the trigger type for starting a time Table
        /// </summary>
        [DataMember]
        public StartTriggerTypes TriggerType
        {
            get
            {
                return this.triggerType;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.triggerType, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Aborts the current time Table.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task AbortTimeTableAsync()
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.TimeTableAbort);

            try
            {
                var response = await this.WriteAsync(command.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        [DataMember]
        public string LatestResponse { get; private set; }
        [DataMember]
        public string LatestWrite { get; private set; }
        [DataMember]
        public string LastTable { get; private set; }

        /// <summary>
        /// Closes the port
        /// </summary>
        public void Close()
        {
            if (this.Emulated)
            {
                return;
            }

            lock (this.sync)
            {
                if (this.falkorPort.IsOpen)
                {
                    this.falkorPort.Close();
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the current software configuration. 
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetConfig()
        {
            string ampsBoxData = string.Empty;
            ampsBoxData += string.Format("\tDevice Settings\n");
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(this.falkorPort.Port))
            {
                ampsBoxData += string.Format(
                    "\t\t{0}:        {1}\n",
                    propertyDescriptor.DisplayName,
                    propertyDescriptor.GetValue(this.falkorPort.Port));
            }

            ampsBoxData += "\n";
            ampsBoxData += string.Format("\tTable Settings\n");
            ampsBoxData += string.Format("\t\tTrigger:         {0}\n", this.TriggerType);
            ampsBoxData += string.Format("\t\tClock:           {0}\n", this.ClockType);
            ampsBoxData += string.Format("\t\tExt. Clock Freq: {0}\n", this.ClockFrequency);
            ampsBoxData += string.Format("\t\tLast Table:      {0}\n", this.lastTable);

            return ampsBoxData;
        }

        /// <summary>
        /// TODO The get dc guard state async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<string> GetDcGuardStateAsync()
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.GetGuardOffset);
            var stringToReturn = await this.WriteAsync(string.Format("{0}", command.Value));
            return stringToReturn;
        }

        /// <summary>
        /// TODO The get drive level.
        /// </summary>
        /// <param name="channel">
        /// TODO The channel.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public async Task<int> GetDriveLevel(int channel)
        {
            var response =
                await
                    this.WriteAsync(
                        string.Format(
                            "{1}{0}{2}",
                            this.commandProvider.CommandSeparator,
                            this.commandProvider.GetCommand(AmpsCommandType.GetDriveLevel).Value,
                            channel));

            if (this.Emulated)
            {
                return EmulatedOutput; // This is a magic number but also dummy.
            }
            int driveLevel = 0;
            int.TryParse(response, out driveLevel);
            return driveLevel;
        }

        /// <summary>
        /// Converts string? response into an int and then parses that to an enum. 
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ErrorCodes> GetError()
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.GetError);
            string response = await this.WriteAsync(command.Value);
            int responseCode;
            int.TryParse(response, out responseCode);
            var code = (ErrorCodes)Enum.ToObject(typeof(ErrorCodes), responseCode);
            return code;
        }

        /// <summary>
        /// The get heater temperature.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<double> GetHeaterTemperature()
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.GetHeaterTemperature);
            var response =
                await
                this.WriteAsync(string.Format("{1}{0}", this.commandProvider.CommandSeparator, command.Value));
            var splitResponse = response.Split(new[] { ']' });
            double temperature;
            double.TryParse(splitResponse[1], out temperature);
            return temperature;
        }

        /// <summary>
        /// Retrieves from the device how many HV DC Power Supplies are available.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public async Task<int> GetHvChannelCount()
        {
            if (this.Emulated)
            {
                return EmulatedChannelCount;
            }

            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.GetHighVoltageChannels);
            string response = string.Empty;

                response = await this.WriteAsync(command.Value);

                // var channelResponse = response.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
                int channels;
                int.TryParse(response, out channels);
                return channels;
        }

        /// <summary>
        /// Retrieves the HV output (currently read)
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public async Task<int> GetHvOutput(int channel)
        {
            string response =
                await
                    this.WriteAsync(
                        string.Format(
                            "{1}{0}{2}",
                            this.commandProvider.CommandSeparator,
                            this.commandProvider.GetCommand(AmpsCommandType.GetDcBias).Value,
                            channel));

            if (this.Emulated)
            {
                return EmulatedOutput; // This is a magic number but also dummy.
            }

            // var data = response.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
            int output = 0;

            var s = response;
            int.TryParse(s, out output);

            return output;
        }

        /// <summary>
        /// TODO The get leftover data.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetLeftoverData()
        {
            return this.falkorPort.Port.ReadExisting();
        }

        /// <summary>
        /// TODO The get output voltage.
        /// </summary>
        /// <param name="channel">
        /// TODO The channel.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public async Task<int> GetOutputVoltage(int channel)
        {
            var response =
                await
                    this.WriteAsync(
                        string.Format(
                            "{1}{0}{2}",
                            this.commandProvider.CommandSeparator,
                            this.commandProvider.GetCommand(AmpsCommandType.GetRfVoltage).Value,
                            channel));

            if (this.Emulated)
            {
                return EmulatedOutput; // This is a magic number but also dummy.
            }

            int resultValue;
            int.TryParse(response, out resultValue);
            return resultValue;
        }

        /// <summary>
        /// Retrieves from the device how many HV RF Power Supplies are available.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public async Task<int> GetRfChannelCount()
        {
            var command = this.commandProvider.GetCommand(AmpsCommandType.GetRfChannels);
            string response = string.Empty;
            try
            {
                response = await this.WriteAsync(command.Value);

                if (this.Emulated)
                {
                    return EmulatedChannelCount; // This is a magic number but also dummy.
                }

                // var data = response.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
                int result;
                int.TryParse(response, out result);
                return result;
            }
            catch (Exception ex)
            {
            }

            return 0;
        }

        /// <summary>
        /// TODO The get radio frequency frequency.
        /// </summary>
        /// <param name="channel">
        /// TODO The channel.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public async Task<int> GetRfFrequency(int channel)
        {
            var command = this.commandProvider.GetCommand(AmpsCommandType.GetRfFrequency);
                var response =
                    await
                    this.WriteAsync(
                        string.Format("{1}{0}{2}", this.commandProvider.CommandSeparator, command.Value, channel));

                if (this.Emulated)
                {
                    return EmulatedOutput;
                }

                var data = response.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
                int frequency = 0;
                foreach (var s in data)
                {
                    int.TryParse(s, out frequency);
                }

                return frequency;
            
        }

        /// <summary>
        /// Gets the version of the AMPS box.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public async Task<string> GetVersion()
        {
            string data = null;

            var command = this.commandProvider.GetCommand(AmpsCommandType.GetVersion);
            try
            {
                data = await this.WriteAsync(command.Value);
                this.boxVersion = data;
            }
            catch (Exception ex)
            {
                data += ex.Message;
            }

            return data;
        }

        /// <summary>
        /// TODO The load time table async.
        /// </summary>
        /// <param name="table">
        /// TODO The table.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IList<string>> LoadTimeTableAsync(SignalTable table)
        {
            IList<string> list = new List<string>();

            list.Add("\tSending Time Table Data.");
            await this.LoadTimeTable(table);


            list.Add("\tSetting Clock Type.");
            await this.SetClock(this.ClockType);

            await this.SetTrigger(this.TriggerType);

            list.Add("\tSetting mode.");
            await this.SetMode();

            this.lastTable = table.Name;

            return list;
        }

        /// <summary>
        /// Opens the port
        /// </summary>
        public void Open()
        {
            if (this.Emulated)
            {
                return;
            }

            lock (this.sync)
            {
                if (!this.falkorPort.IsOpen)
                {
                    this.falkorPort.Port.ReadTimeout = ConstReadTimeout;
                    this.falkorPort.Port.WriteTimeout = ConstWriteTimeout;
                    this.falkorPort.Open();
                }
            }
        }

        /// <summary>
        /// TODO The toggle digital output.
        /// </summary>
        /// <param name="channel">
        /// TODO The channel.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task PulseDigitalOutput(string channel)
        {
            var command = this.commandProvider.GetCommand(AmpsCommandType.SetDigitalIo);
            await
                    this.WriteAsync(
                        string.Format(
                            "{1}{0}{2}{0}{3}",
                            this.commandProvider.CommandSeparator,
                            command.Value,
                            channel,
                            "P"));
        }

        /// <summary>
        /// Saves parameters on the AMPS Box.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SaveParameters()
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.Save);
            await this.WriteAsync(command.Value);
        }

        /// <summary>
        /// The set clock internal.
        /// </summary>
        public void SetClockInternal()
        {
            this.ClockType = ClockType.Internal;
            this.ClockFrequency = this.commandProvider.InternalClock;
        }

        public async Task Reset()
        {
            var command = this.commandProvider.GetCommand(AmpsCommandType.Reset);
            await this.WriteAsync(command.Value);
        }

        public async Task Test()
        {
            var command = this.commandProvider.GetCommand(AmpsCommandType.Test);
            await this.WriteAsync(command.Value);
        }

        /// <summary>
        /// Sets the output DC/HV voltage.
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="voltage">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SetDcBias(int channel, double voltage)
        {
            await this.SetDcBias(channel, Convert.ToInt32(voltage));
        }

        /// <summary>
        /// Sets the output DC/HV voltage
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="voltage">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SetDcBias(int channel, int voltage)
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.SetDcBias);
            try
            {
                await
                        this.WriteAsync(
                            string.Format(
                                "{1}{0}{3}{0}{2:000}",
                                this.commandProvider.CommandSeparator,
                                command.Value,
                                voltage,
                                channel));
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// TODO The set dc guard state async.
        /// </summary>
        /// <param name="state">
        /// TODO The state.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SetDcGuardStateAsync(string state)
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.SetGuardOffset);
            await this.WriteAsync(string.Format("{0},{1}", command.Value, state));
        }

        /// <summary>
        /// TODO The set heater setpoint.
        /// </summary>
        /// <param name="temperature">
        /// TODO The temperature.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SetHeaterSetpoint(int temperature)
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.SetHeaterSetpoint);
            await
                    this.WriteAsync(
                        string.Format("{1}{0}{2}", this.commandProvider.CommandSeparator, command.Value, temperature));
        }

        /// <summary>
        /// TODO The set negative hv.
        /// </summary>
        /// <param name="voltage">
        /// TODO The voltage.
        /// </param>
        public async void SetNegativeHV(int voltage)
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.SetNegativeHV);

            await
                    this.WriteAsync(
                        string.Format("{1}{0}{2:000}", this.commandProvider.CommandSeparator, command.Value, voltage));
        }

        /// <summary>
        /// TODO The set positive hv.
        /// </summary>
        /// <param name="voltage">
        /// TODO The voltage.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SetPositiveHV(int voltage)
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.SetPositiveHV);

            await
                    this.WriteAsync(
                        string.Format("{1}{0}{2:000}", this.commandProvider.CommandSeparator, command.Value, voltage));
        }

        /// <summary>
        /// TODO The set radio frequency drive level.
        /// </summary>
        /// <param name="channel">
        /// TODO The channel.
        /// </param>
        /// <param name="voltage">
        /// TODO The voltage.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SetRadioFrequencyDriveLevel(int channel, int voltage)
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.SetOutputDriveLevel);
            try
            {
                await
                        this.WriteAsync(
                            string.Format(
                                "{1}{0}{3}{0}{2:000}",
                                this.commandProvider.CommandSeparator,
                                command.Value,
                                voltage,
                                channel));
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Sets the RF frequency for the channel provided.
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="frequency">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SetRadioFrequencyFrequency(int channel, int frequency)
        {
            var command = this.commandProvider.GetCommand(AmpsCommandType.SetRfFrequency);
            try
            {
                await
                        this.WriteAsync(
                            string.Format(
                                "{1}{0}{3}{0}{2:000}",
                                this.commandProvider.CommandSeparator,
                                command.Value,
                                frequency,
                                channel));
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// TODO The set radio frequency output voltage.
        /// </summary>
        /// <param name="channel">
        /// TODO The channel.
        /// </param>
        /// <param name="voltage">
        /// TODO The voltage.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SetRadioFrequencyOutputVoltage(int channel, int voltage)
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.SetRfVoltage);

            await
                    this.WriteAsync(
                        string.Format(
                            "{1}{0}{3}{0}{2:000}",
                            this.commandProvider.CommandSeparator,
                            command.Value,
                            voltage,
                            channel));
        }

        /// <summary>
        /// Turns the real time mode off within the AMPS Box.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SetRealTimeOff()
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.SetRTOff);
            await this.WriteAsync(command.Value);
        }

        /// <summary>
        /// Loads then starts the time Table.
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public async Task<IList<string>> StartTimeTableAsync(SignalTable table)
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.TimeTableStart);

            IList<string> list = new List<string>();

            list.Add("\tStarting time table.");
            await this.WriteAsync(string.Format("{0}", command.Value));

            this.lastTable = table.Name;

            return list;
        }

        /// <summary>
        /// TODO The toggle digital direction.
        /// </summary>
        /// <param name="channel">
        /// The channel.
        /// </param>
        /// <param name="direction">
        /// TODO The direction.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ToggleDigitalDirection(string channel, string direction)
        {
            var command = this.commandProvider.GetCommand(AmpsCommandType.SetDigitalIoDirection);
            try
            {
                await
                        this.WriteAsync(
                            string.Format(
                                "{0}{1}{0}{2}{0}{3}",
                                this.commandProvider.CommandSeparator,
                                command.Value,
                                channel,
                                direction));
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// TODO The toggle digital output.
        /// </summary>
        /// <param name="channel">
        /// TODO The channel.
        /// </param>
        /// <param name="state">
        /// TODO The state.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ToggleDigitalOutput(string channel, bool state)
        {
            var command = this.commandProvider.GetCommand(AmpsCommandType.SetDigitalIo);
            await
                    this.WriteAsync(
                        string.Format(
                            "{1}{0}{2}{0}{3}",
                            this.commandProvider.CommandSeparator,
                            command.Value,
                            channel,
                            Convert.ToInt32(state)));
        }

        /// <summary>
        /// TODO The toggle heater.
        /// </summary>
        /// <param name="state">
        /// TODO The state.
        /// </param>
        [Obsolete("This method returns before the external function call has finished, use TogleHeaterAsync instead.")]
        public void ToggleHeater(State state)
        {
            var command = this.commandProvider.GetCommand(AmpsCommandType.ToggleHeater);
#pragma warning disable 4014
            this.WriteAsync(string.Format("{1}{0}{2}", this.commandProvider.CommandSeparator, command.Value, state));
#pragma warning restore 4014
        }

        /// <summary>
        /// TODO The toggle heater async.
        /// </summary>
        /// <param name="state">
        /// TODO The state.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ToggleHeaterAsync(State state)
        {
            var command = this.commandProvider.GetCommand(AmpsCommandType.ToggleHeater);
            await
                    this.WriteAsync(
                        string.Format("{1}{0}{2}", this.commandProvider.CommandSeparator, command.Value, state));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the Table onto the device
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task LoadTimeTable(SignalTable table)
        {
            AmpsClockConverter converter = new AmpsClockConverter(this.ClockFrequency);
            ISignalTableFormatter<SignalTable, double> formatter = new AmpsBoxSignalTableCommandFormatter();

            string command = formatter.FormatTable(table, converter);
            this.LastTable = command;

            await this.WriteAsync(command);
        }

        /// <summary>
        /// TODO The on next.
        /// </summary>
        /// <param name="serialPort">
        /// TODO The serial port.
        /// </param>
        private void OnNext(SerialPort serialPort)
        {
            serialPort.ReadTimeout = ConstReadTimeout;
            serialPort.WriteTimeout = ConstWriteTimeout;
            serialPort.DataReceived += SerialPort_DataReceived;
            //   serialPort.ErrorReceived += this.PortErrorReceived;
            //   serialPort.PinChanged += this.PortPinChanged;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Eof)
            {
                throw new Exception("End of file exception!");
            }
        }


        /// <summary>
        /// TODO The m_port_ error received.
        /// </summary>
        /// <param name="sender">
        /// TODO The sender.
        /// </param>
        /// <param name="e">
        /// TODO The e.
        /// </param>
        /// <exception cref="IOException">
        /// </exception>
        private void PortErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (e.EventType == SerialError.Frame)
            {
                throw new IOException("IO Frame Error");
            }

            if (e.EventType == SerialError.Overrun)
            {
                throw new IOException("IO Overrun Error");
            }

            if (e.EventType == SerialError.RXOver)
            {
                throw new IOException("IO RXOver Error");
            }

            if (e.EventType == SerialError.RXParity)
            {
                throw new IOException("IO RXParity Error");
            }

            if (e.EventType == SerialError.TXFull)
            {
                throw new IOException("IO TXFull Error");
            }
        }


        /// <summary>
        /// TODO The m_port_ pin changed.
        /// </summary>
        /// <param name="sender">
        /// TODO The sender.
        /// </param>
        /// <param name="e">
        /// TODO The e.
        /// </param>
        private void PortPinChanged(object sender, SerialPinChangedEventArgs e)
        {
        }

        private async Task<string> Read(SerialPort port)
        {
            byte[] buffer = new byte[1024];
            int actualLength = 0;
            string response = String.Empty;
            string stringToReturn = String.Empty;
            try
            {
                const int Offset = 0;
                Stopwatch watch = new Stopwatch();
                watch.Start();
              
                while (!stringToReturn.Contains("\r\n") && watch.ElapsedMilliseconds < 1000)
                {
                    actualLength = await port.BaseStream.ReadAsync(buffer, Offset, buffer.Length);

                    byte[] received = new byte[actualLength];

                    Buffer.BlockCopy(buffer, Offset, received, Offset, actualLength);

                    stringToReturn += System.Text.Encoding.ASCII.GetString(received);

                    var ampsResponse = await this.ValidateResponse(stringToReturn);
                    if (ampsResponse == Responses.NAK)
                    {
                        var error = await this.GetError();
                        return error.ToString();
                    }
                    if (stringToReturn.Contains(this.commandProvider.EndOfLine)
                        && (ampsResponse == Responses.ACK || ampsResponse == Responses.NAK))

                    {
                        watch.Stop();
                        return stringToReturn;
                    }
                }
                watch.Stop();
            }

            catch (InvalidOperationException ex)
            {
            }

            catch (NotSupportedException ex)

            {
            }

            catch (ArgumentNullException ex)

            {
            }

            catch (ArgumentOutOfRangeException ex)

            {
            }

            catch (ArgumentException ex)

            {
            }
            return string.Empty;
        }

        /// <summary>
        /// Tells the AMPS box which clock to use: external or internal
        /// </summary>
        /// <param name="clockType">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task SetClock(ClockType clockType)
        {
            AmpsCommand command;
            switch (clockType)
            {
                case ClockType.External:
                    command = this.commandProvider.GetCommand(AmpsCommandType.TimeTableClockSyncExternal);
                    await this.WriteAsync(command.Value);
                    break;

                case ClockType.Internal:
                    command = this.commandProvider.GetCommand(AmpsCommandType.TimeTableClockSycnInternal);
                    await this.WriteAsync(command.Value);
                    break;
            }
        }

        private async Task SetTrigger(StartTriggerTypes startTriggerType)
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.CommandSetTrigger);
            var commandString = string.Format("{0},{1}", command.Value, startTriggerType);
            await this.WriteAsync(commandString);
        }

        /// <summary>
        /// Tells the AMPS Box how to repeat (if at all) 
        /// </summary>
        /// <param name="iterations">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SetMode()
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.Mode);
            await this.WriteAsync(string.Format("{0}", command.Value));
        }

        public async Task StopTable()
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.TimeTableStop);
            await this.WriteAsync(string.Format("{0}", command.Value));
        }

        /// <summary>
        /// Validates the command string is not NAK
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<Responses> ValidateResponse(string message)
        {
            var messageRegex = Regex.Replace(message, @"\p{Cc}", a => string.Format("{0:X2}", (byte)a.Value[0]));
            ErrorCodes error;

            foreach (var s in messageRegex)
            {
                if (char.GetNumericValue(s) != -1)
                {
                    // 0x15 NAK ASCII code character
                    // 0x06 ACK ASCII code character
                    switch ((int)char.GetNumericValue(s))
                    {
                        case 0x15:
                            return Responses.NAK;
                        case 0x06:
                            return Responses.ACK;
                    }
                }
            }
            return new Responses();
        }

        /// <summary>
        /// Writes command to serial port.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="readBack"></param>
        public async Task<string> WriteAsync(string command)
        {
            LatestWrite = command;
            try
            {
                var buffer = System.Text.Encoding.ASCII.GetBytes(command + this.commandProvider.EndOfLine);

                await this.falkorPort.Port.BaseStream.WriteAsync(buffer, 0, buffer.Count());

                string response = await Read(this.falkorPort.Port);

                LatestResponse = await this.ParseResponseAsync(response);

                return LatestResponse;
            }
            catch (IOException ioException)
            {
                throw new IOException(ioException.Message, ioException.InnerException);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message, ex.InnerException);
            }
            catch (TimeoutException timeoutException)
            {
                throw new TimeoutException(timeoutException.Message, timeoutException.InnerException);
            }
            catch (ArgumentNullException exception)
            {
                throw new ArgumentNullException(exception.Message, exception.InnerException);
            }
        }

        private async Task<string> ParseResponseAsync(string response, bool shouldValidateResponse = true)
        {
            if (string.IsNullOrEmpty(response))
            {
                return String.Empty;
            }

            string localStringData = response;
            string dataToValidate = localStringData;
            localStringData = localStringData.Replace("\n", string.Empty);
            localStringData = localStringData.Replace("\0", string.Empty);
            var newData = Regex.Replace(localStringData, @"\p{Cc}", a => string.Format("[{0:X2}]", (byte)a.Value[0]));
            response = "\tAMPS>> " + newData;

            newData = Regex.Replace(localStringData, @"\p{Cc}", string.Empty);
            var values = newData.Split(new[] { "\0", "," }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length > 0)
            {
                localStringData = values[values.Length - 1];
            }
            return localStringData;
        }

        #endregion
    }
}