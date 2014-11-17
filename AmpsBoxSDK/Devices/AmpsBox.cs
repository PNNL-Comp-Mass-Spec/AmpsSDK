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
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Ports;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    using AmpsBoxSdk.Commands;
    using AmpsBoxSdk.IO;

    using FalkorSDK.Data.Signals;
    using FalkorSDK.IO.Ports;
    using FalkorSDK.IO.Signals;

    using Microsoft.Practices.Prism.Logging;

    using ReactiveUI;

    /// <summary>
    /// Communicates with a PNNL Amps Box
    /// Non shared parts creation policy so that multiple amps boxes can exist in the system at once.
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class AmpsBox
    {
        #region Constants

        /// <summary>
        /// TODO The default_ box_ version.
        /// </summary>
        private const string CONST_DEFAULT_BOX_VERSION = "v2.0b";

        /// <summary>
        /// read timeout in milliseconds
        /// </summary>
        private const int CONST_READ_TIMEOUT = 10000;

        /// <summary>
        /// Default sleep time between writes / reads
        /// </summary>
        private const int CONST_SLEEP_TIME = 1000;

        /// <summary>
        /// TODO The cons t_ writ e_ timeout.
        /// </summary>
        private const int CONST_WRITE_TIMEOUT = 10000;

        /// <summary>
        /// Emulated channel count for RF and HV testing
        /// </summary>
        private const int EMULATED_CHANNEL_COUNT = 8;

        /// <summary>
        /// TODO The emulate d_ output.
        /// </summary>
        private const int EMULATED_OUTPUT = 100;

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the command provider for the given box in case versions are different.
        /// </summary>
        private readonly AmpsCommandProvider commandProvider;

        /// <summary>
        /// TODO The data buffer queue.
        /// </summary>
        private readonly Queue<string> dataBufferQueue;

        /// <summary>
        /// TODO The logger.
        /// </summary>
        private readonly ILoggerFacade logger;

        /// <summary>
        /// Synchronization object.
        /// </summary>
        private readonly object sync;

        /// <summary>
        /// TODO The task factory.
        /// </summary>
        private readonly TaskFactory<string> taskFactory;

        /// <summary>
        /// Firmware of the box.
        /// </summary>
        private string boxVersion;

        /// <summary>
        /// TODO The data.
        /// </summary>
        private string data;

        /// <summary>
        /// TODO The _expected response length.
        /// </summary>
        private int expectedResponseLength;

        /// <summary>
        /// Serial Port 
        /// </summary>
        private FalkorSerialPort falkorPort;

        /// <summary>
        /// Last Table executed.
        /// </summary>
        private string lastTable;

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
        public AmpsBox(ILoggerFacade logger)
        {
            this.logger = logger;
            this.taskFactory = new TaskFactory<string>();
            this.boxVersion = CONST_DEFAULT_BOX_VERSION;
            this.sync = new object();
            this.ReadTimeout = CONST_READ_TIMEOUT;
            this.ClockType = ClockType.Internal;
            this.TriggerType = StartTriggerTypes.SW;
            this.Emulated = false;
            this.commandProvider = AmpsCommandFactory.CreateCommandProvider(this.boxVersion);
            this.ClockFrequency = this.commandProvider.InternalClock;
            this.ReadWriteTimeout = CONST_SLEEP_TIME;
            this.WhenAnyValue(x => x.Port.Port).Subscribe(this.OnNext);
            this.dataBufferQueue = new Queue<string>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the clock frequency of the AMPS Box.
        /// </summary>        
        public int ClockFrequency { get; set; }

        /// <summary>
        /// Gets or sets the clock type for executing a time Table.
        /// </summary>
        public ClockType ClockType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is emulated or not.
        /// </summary>
        public bool Emulated { get; set; }

        /// <summary>
        /// Gets or sets the ID of this device.
        /// </summary>
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
                this.falkorPort = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of time to wait for a number of characters.
        /// </summary>		
        public int ReadTimeout { get; set; }

        /// <summary>
        /// Gets or sets the read write timeout between IO calls on the serial port
        /// </summary>
        public int ReadWriteTimeout { get; set; }

        /// <summary>
        /// Gets or sets the trigger type for starting a time Table
        /// </summary>
        public StartTriggerTypes TriggerType { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Aborts the current time Table.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task AbortTimeTable()
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.TimeTableAbort);

            try
            {
                var response = await Task.Run(() => this.WriteRead(command.Value));
                this.logger.Log(response, Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                this.logger.Log(ex.StackTrace, Category.Exception, Priority.High);
            }
        }

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
            string data = string.Empty;
            data += string.Format("\tDevice Settings\n");
            data += string.Format("\t\tPort:        {0}\n", this.falkorPort.Port.PortName);
            data += string.Format("\t\tBaud:        {0}\n", this.falkorPort.Port.BaudRate);
            data += string.Format("\t\tHandshake:   {0}\n", this.falkorPort.Port.Handshake);
            data += string.Format("\t\tStopBits:    {0}\n", this.falkorPort.Port.StopBits);
            data += string.Format("\t\tParity:      {0}\n", this.falkorPort.Port.Parity);
            data += string.Format("\t\tIs Open:     {0}\n", this.falkorPort.IsOpen);
            data += "\n";
            data += string.Format("\tTable Settings\n");
            data += string.Format("\t\tTrigger:         {0}\n", this.TriggerType);
            data += string.Format("\t\tClock:           {0}\n", this.ClockType);
            data += string.Format("\t\tExt. Clock Freq: {0}\n", this.ClockFrequency);
            data += string.Format("\t\tLast Table:      {0}\n", this.lastTable);

            return data;
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
            var stringToReturn = await Task.Run(() => this.WriteRead(string.Format("{0}", command.Value)));
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
                Task.Run(
                    () =>
                    this.WriteRead(
                        string.Format(
                            "{1}{0}{2}", 
                            this.commandProvider.CommandSeparator, 
                            this.commandProvider.GetCommand(AmpsCommandType.GetDriveLevel).Value, 
                            channel)));

            if (this.Emulated)
            {
                return EMULATED_OUTPUT; // This is a magic number but also dummy.
            }

            return Convert.ToInt32(response);
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
            string response = await Task.Run(() => this.WriteRead(command.Value));
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
                Task.Run(
                    () => this.WriteRead(string.Format("{1}{0}", this.commandProvider.CommandSeparator, command.Value)));
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
                return EMULATED_CHANNEL_COUNT;
            }

            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.GetHighVoltageChannels);
            string response = string.Empty;
            try
            {
                response = await Task.Run(() => this.WriteRead(command.Value));

                // var channelResponse = response.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
                int channels;
                int.TryParse(response, out channels);
                return channels;
            }
            catch (Exception ex)
            {
                this.logger.Log(response, Category.Info, Priority.High);
                this.logger.Log(ex.Message, Category.Exception, Priority.High);
                this.logger.Log(ex.StackTrace, Category.Exception, Priority.High);
            }

            return 0;
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
            this.falkorPort.Port.DiscardInBuffer();
            this.falkorPort.Port.DiscardOutBuffer();

            string response =
                await
                Task.Run(
                    () =>
                    this.WriteRead(
                        string.Format(
                            "{1}{0}{2}", 
                            this.commandProvider.CommandSeparator, 
                            this.commandProvider.GetCommand(AmpsCommandType.GetDcBias).Value, 
                            channel)));

            if (this.Emulated)
            {
                return EMULATED_OUTPUT; // This is a magic number but also dummy.
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
                Task.Run(
                    () =>
                    this.WriteRead(
                        string.Format(
                            "{1}{0}{2}", 
                            this.commandProvider.CommandSeparator, 
                            this.commandProvider.GetCommand(AmpsCommandType.GetRfVoltage).Value, 
                            channel)));

            if (this.Emulated)
            {
                return EMULATED_OUTPUT; // This is a magic number but also dummy.
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
                response = await Task.Run(() => this.WriteRead(command.Value));

                if (this.Emulated)
                {
                    return EMULATED_CHANNEL_COUNT; // This is a magic number but also dummy.
                }

                // var data = response.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
                int result;
                int.TryParse(response, out result);
                return result;
            }
            catch (Exception ex)
            {
                this.logger.Log(ex.StackTrace, Category.Exception, Priority.High);
                this.logger.Log(response, Category.Exception, Priority.High);
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
            try
            {
                var response =
                    await
                    this.WriteRead(
                        string.Format("{1}{0}{2}", this.commandProvider.CommandSeparator, command.Value, channel));

                if (this.Emulated)
                {
                    return EMULATED_OUTPUT;
                }

                var data = response.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
                int frequency = 0;
                foreach (var s in data)
                {
                    int.TryParse(s, out frequency);
                }

                return frequency;
            }
            catch (Exception ex)
            {
                this.logger.Log(ex.StackTrace, Category.Exception, Priority.High);
            }

            return 0;
        }

        /// <summary>
        /// Gets the version of the AMPS box.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public async Task<string> GetVersion()
        {
            string data;

            var command = this.commandProvider.GetCommand(AmpsCommandType.GetVersion);
            try
            {
                data = await Task.Run(() => this.WriteRead(command.Value));
                this.boxVersion = data;
            }
            catch (Exception ex)
            {
                this.logger.Log(ex.Message, Category.Exception, Priority.High);
                data = "Failed to get version";
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
                    try
                    {
                        this.falkorPort.Open();
                    }
                    catch (Exception ex)
                    {
                        this.logger.Log(ex.Message, Category.Exception, Priority.High);
                    }
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
                Task.Run(
                    () =>
                    this.WriteRead(
                        string.Format(
                            "{1}{0}{2}{0}{3}", 
                            this.commandProvider.CommandSeparator, 
                            command.Value, 
                            channel, 
                            "P")));
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
            await Task.Run(() => this.WriteRead(command.Value));
        }

        /// <summary>
        /// The set clock internal.
        /// </summary>
        public void SetClockInternal()
        {
            this.ClockType = ClockType.Internal;
            this.ClockFrequency = this.commandProvider.InternalClock;
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
                    Task.Run(
                        () =>
                        this.WriteRead(
                            string.Format(
                                "{1}{0}{3}{0}{2:000}", 
                                this.commandProvider.CommandSeparator, 
                                command.Value, 
                                voltage, 
                                channel)));
            }
            catch (Exception ex)
            {
                this.logger.Log(ex.StackTrace, Category.Exception, Priority.High);
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
            await Task.Run(() => this.WriteRead(string.Format("{0},{1}", command.Value, state)));
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
                Task.Run(
                    () =>
                    this.WriteRead(
                        string.Format("{1}{0}{2}", this.commandProvider.CommandSeparator, command.Value, temperature)));
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
                Task.Run(
                    () =>
                    this.WriteRead(
                        string.Format("{1}{0}{2:000}", this.commandProvider.CommandSeparator, command.Value, voltage)));
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
                Task.Run(
                    () =>
                    this.WriteRead(
                        string.Format("{1}{0}{2:000}", this.commandProvider.CommandSeparator, command.Value, voltage)));
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
                    Task.Run(
                        () =>
                        this.WriteRead(
                            string.Format(
                                "{1}{0}{3}{0}{2:000}", 
                                this.commandProvider.CommandSeparator, 
                                command.Value, 
                                voltage, 
                                channel)));
            }
            catch (Exception ex)
            {
                this.logger.Log(ex.Message, Category.Exception, Priority.High);
                this.logger.Log(ex.StackTrace, Category.Exception, Priority.High);
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
                    Task.Run(
                        () =>
                        this.WriteRead(
                            string.Format(
                                "{1}{0}{3}{0}{2:000}", 
                                this.commandProvider.CommandSeparator, 
                                command.Value, 
                                frequency, 
                                channel)));
            }
            catch (Exception ex)
            {
                this.logger.Log(ex.Message, Category.Exception, Priority.High);
                this.logger.Log(ex.StackTrace, Category.Exception, Priority.High);
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
                Task.Run(
                    () =>
                    this.WriteRead(
                        string.Format(
                            "{1}{0}{3}{0}{2:000}", 
                            this.commandProvider.CommandSeparator, 
                            command.Value, 
                            voltage, 
                            channel)));
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
            await Task.Run(() => this.WriteRead(command.Value));
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
            await Task.Run(() => this.WriteRead(string.Format("{0}", command.Value)));

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
                    Task.Run(
                        () =>
                        this.WriteRead(
                            string.Format(
                                "{0}{1}{0}{2}{0}{3}", 
                                this.commandProvider.CommandSeparator, 
                                command.Value, 
                                channel, 
                                direction)));
            }
            catch (Exception ex)
            {
                this.logger.Log(ex.Message, Category.Exception, Priority.High);
                this.logger.Log(ex.StackTrace, Category.Exception, Priority.High);
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
                Task.Run(
                    () =>
                    this.WriteRead(
                        string.Format(
                            "{1}{0}{2}{0}{3}", 
                            this.commandProvider.CommandSeparator, 
                            command.Value, 
                            channel, 
                            Convert.ToInt32(state))));
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
            this.WriteRead(string.Format("{1}{0}{2}", this.commandProvider.CommandSeparator, command.Value, state));
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
                Task.Run(() => this.WriteRead(string.Format("{1}{0}{2}", this.commandProvider.CommandSeparator, command.Value, state)));
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

            await Task.Run(() => this.WriteRead(command));
        }

        /// <summary>
        /// TODO The on next.
        /// </summary>
        /// <param name="serialPort">
        /// TODO The serial port.
        /// </param>
        private void OnNext(SerialPort serialPort)
        {
            serialPort.DataReceived += this.PortOnDataReceived;
            serialPort.ErrorReceived += this.PortErrorReceived;
            serialPort.PinChanged += this.PortPinChanged;
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
        /// TODO The m port on data received.
        /// </summary>
        /// <param name="sender">
        /// TODO The sender.
        /// </param>
        /// <param name="serialDataReceivedEventArgs">
        /// TODO The serial data received event args.
        /// </param>
        private async void PortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            SerialPort sp = (SerialPort)sender;
            if (serialDataReceivedEventArgs.EventType == SerialData.Chars)
            {
                var tempData = sp.ReadExisting();
                try
                {
                    var response = await this.ValidateResponse(tempData);
                    this.dataBufferQueue.Enqueue(tempData);
                    if (tempData.Contains(this.commandProvider.EndOfLine) || response == Responses.ACK
                        || response == Responses.NAK)
                    {
                        string data = string.Empty;
                        while (this.dataBufferQueue.Any())
                        {
                            data += this.dataBufferQueue.Dequeue();
                        }

                        this.data = data;

                        if (this.data.Contains(this.commandProvider.TableResponse))
                        {
                            this.logger.Log("Table Complete", Category.Info, Priority.Low);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.logger.Log(ex.Message, Category.Exception, Priority.High);
                }
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
                    await Task.Run(() => this.WriteRead(command.Value));
                    break;

                case ClockType.Internal:
                    command = this.commandProvider.GetCommand(AmpsCommandType.TimeTableClockSycnInternal);
                    await Task.Run(() => this.WriteRead(command.Value));
                    break;
            }
        }

	    private async Task SetTrigger(StartTriggerTypes startTriggerType)
	    {
		    AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.CommandSetTrigger);
		    string commandString = command.Value + "," + startTriggerType.ToString();
		    try
		    {
				await Task.Run(() => this.WriteRead(commandString));
			}
		    catch (Exception ex)
		    {
			   this.logger.Log(ex.Message, Category.Exception, Priority.High);
		    }
		   
	    }

        /// <summary>
        /// Tells the AMPS Box how to repeat (if at all) 
        /// </summary>
        /// <param name="iterations">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task SetMode()
        {
            AmpsCommand command = this.commandProvider.GetCommand(AmpsCommandType.Mode);
            await Task.Run(() => this.WriteRead(string.Format("{0}", command.Value)));
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
                            error = await this.GetError();
                            this.logger.Log(error.ToString(), Category.Warn, Priority.High);
                            return Responses.NAK;
                        case 0x06:
                            return Responses.ACK;
                    }
                }
            }

            if (message.Length < 1)
            {
                throw new AmpsEmptyResponseErrorException("The AMPS Response was empty.");
            }

            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrEmpty(message))
            {
                throw new AmpsEmptyResponseErrorException("The AMPS Response was empty.");
            }

            throw new AmpsErrErrorException("The command expected an acknowledge response but did not receive it.");
        }

        /// <summary>
        /// Writes the command string to the serial port and waits for an expected response.
        /// </summary>
        /// <param name="command">
        /// </param>
        /// <param name="shouldValidateResponse">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private async Task<string> WriteRead(string command, bool shouldValidateResponse = true)
        {
            this.data = null;
            if (this.Emulated)
            {
                return "ACK";
            }

            string outData = command; // + this.commandProvider.EndOfLine;

            this.expectedResponseLength = 1 + this.commandProvider.NumberOfPaddingCharacters;

            this.falkorPort.Port.DiscardInBuffer();
            this.falkorPort.Port.DiscardOutBuffer();
            this.falkorPort.Port.WriteLine(outData);

            string response = "\tSW>> " + outData;
            this.logger.Log(response, Category.Info, Priority.Low);
            Stopwatch watch = new Stopwatch();
            while (this.data == null)
            {
                watch.Start();
                Thread.Sleep(50);
                if (watch.ElapsedMilliseconds > 10000)
                {
                    throw new Exception("Timeout Exception");
                }
            }

            this.data += this.falkorPort.Port.ReadExisting();
            string localStringData = this.data;
            string dataToValidate = localStringData;
            localStringData = localStringData.Replace("\n", string.Empty);
            localStringData = localStringData.Replace("\0", string.Empty);
            var newData = Regex.Replace(localStringData, @"\p{Cc}", a => string.Format("[{0:X2}]", (byte)a.Value[0]));
            response = "\tAMPS>> " + newData;
            this.logger.Log(response, Category.Info, Priority.Low);
            newData = Regex.Replace(localStringData, @"\p{Cc}", string.Empty);
            var values = newData.Split(new[] { "\0", "," }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length > 0)
            {
                localStringData = values[values.Length - 1];
            }

            try
            {
                if (shouldValidateResponse)
                {
                    await Task.Run(() => this.ValidateResponse(dataToValidate));
                }
            }
            catch
            {
                File.AppendAllText(
                    "AMPS-commands.txt", 
                    string.Format(
                        "{0}\tread-exception\t{1}\n", 
                        DateTime.Now, 
                        dataToValidate.Replace("\n", string.Empty)));
            }

            return localStringData;
        }

        #endregion
    }
}