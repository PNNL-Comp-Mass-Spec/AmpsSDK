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
        public readonly IAmpsCommunicator communicator;

        private Lazy<AmpsBoxDeviceData> deviceData;

        private readonly Queue<AmpsMessage> messageQueue = new Queue<AmpsMessage>();

        private readonly Queue<string> responseQueue = new Queue<string>();


        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public AmpsBox(IAmpsCommunicator communicator)
        {
            this.communicator = communicator ?? throw new ArgumentNullException(nameof(communicator));
            this.communicator.Open();
            var source = this.communicator.MessageSources;

            source.Where(x => x != "tblcmplt" && !x.Contains("ABORTED") && x != "tblrdy" && !string.IsNullOrEmpty(x) && x != "TableNotReady").Select(s =>
            {
                this.responseQueue.Enqueue(s);
                return s;
            }).Subscribe();

            this.TableCompleteOrAborted = source
                .Where(x => x.Equals("tblcmplt", StringComparison.OrdinalIgnoreCase) || x.Contains("ABORTED")).Select(x => Unit.Default);
            this.TableCompleteOrAborted.Subscribe(unit =>
            {
                System.Diagnostics.Trace.WriteLine($"{Environment.NewLine}complete{DateTime.Now}");
            });

            this.ModeReady = source.Where(x => x.Equals("tblcmplt", StringComparison.OrdinalIgnoreCase)).Select(x => Unit.Default);
            this.ModeReady.Subscribe();
            ClockFrequency = 16000000;
        }

        #endregion

        private async Task ProcessQueue(bool response = false)
        {
            while (messageQueue.Count > 0)
            {
                var message = messageQueue.Dequeue();
                message.WriteTo(this.communicator);
                Thread.Sleep(50);
                while (response && responseQueue.Count == 0)
                {
                    Thread.Sleep(50);
                }
                break;

            }


        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the clock frequency of the AMPS Box.
        /// </summary>   
        [DataMember]
        public int ClockFrequency { get; set; }

        [DataMember]
        public string Name => GetName().Result;

        public IObservable<Unit> TableCompleteOrAborted { get; }

        public IObservable<Unit> ModeReady { get; }
        public IAmpsCommunicator Communicator => communicator;
        
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
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SDCB, channel.ToString(), volts.ToString());
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(false);

            return Unit.Default;
        }

        public async Task<int> GetDcBiasSetpoint(int channel)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GDCB, channel);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();
            int.TryParse(response, out int setPoint);
            return setPoint;
        }

        public async Task<int> GetDcBiasReadback(int channel)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GDCBV, channel);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);

            int voltageReadback = 0;
            var response = responseQueue.Dequeue();
            int.TryParse(response, out voltageReadback);

            return voltageReadback;
        }

        public async Task<int> GetDcBiasCurrentReadback(int channel)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GDCBI, channel);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);

            int currentReadback = 0;
            var response = responseQueue.Dequeue();
            int.TryParse(response, out currentReadback);
            return currentReadback;
        }

        public async Task<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber, int offsetVolts)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SDCBOF, brdNumber, offsetVolts);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(false);

            return Unit.Default;
        }

        public async Task<int> GetBoardDcBiasOffsetVoltage(int brdNumber)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SDCBOF, brdNumber);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            int.TryParse(response, out int offsetVoltage);
            return offsetVoltage;
        }

        public async Task<int> GetNumberDcBiasChannels()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GCHAN, Module.DCB.ToString());
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            int.TryParse(response, out int channels);
            return channels;

        }

        public async Task<Unit> SetDigitalState(string channel, bool state)
        {
            var stateAsInt = Convert.ToInt32(state).ToString();
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SDIO, channel, stateAsInt);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(false);


            return Unit.Default;
        }

        public async Task<Unit> PulseDigitalSignal(string channel)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SDIO, channel, "P");
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(false);

            return Unit.Default;
        }

        public async Task<bool> GetDigitalState(string channel)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GDIO, channel);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();
            var result = Convert.ToBoolean(Convert.ToInt16(response));
            return result;
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
                    var ampsmessage = AmpsMessage.Create(AmpsCommand.SDIODR, channel, digitalDirection.ToString());
                    messageQueue.Enqueue(ampsmessage);
                    await ProcessQueue(false);

                    return Unit.Default;
                default:
                    return Unit.Default;
            }

        }

        public async Task<DigitalDirection> GetDigitalDirection(string channel)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GDIODR, channel);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            Enum.TryParse(response, true, out DigitalDirection result);
            return result;
        }

        public async Task<int> GetNumberDigitalChannels()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GCHAN, Module.DIO.ToString());
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            int.TryParse(response, out int channels);
            return channels;
        }

        public async Task<Unit> SetPositiveHighVoltage(int volts)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SPHV, volts);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(false);

            return Unit.Default;
        }

        public async Task<Unit> SetNegativeHighVoltage(int volts)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SNHV, volts);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(false);


            return Unit.Default;
        }

        public async Task<(double, double)> GetPositiveEsi()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GPHVV);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();
            var values = response.Split(',');
            if (values.Length < 2)
            {
                return (0, 0);
            }
            int.TryParse(values[0], out int voltage);
            int.TryParse(values[1], out int current);

            return (voltage, current);
        }

        public async Task<(double, double)> GetNegativeEsi()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GNHVV);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            var values = response.Split(',');
            if (values.Length < 2)
            {
                return (0, 0);
            }
            int.TryParse(values[0], out int voltage);
            int.TryParse(values[1], out int current);

            return (voltage, current);
        }

        public async Task<Unit> TurnOnHeater()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SHTR, State.ON.ToString());
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(false);
            return Unit.Default;
        }

        public async Task<Unit> TurnOffHeater()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SHTR, State.OFF.ToString());
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(false);


            return Unit.Default;
        }

        public async Task<Unit> SetTemperatureSetpoint(int temperature)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SHTRTMP, temperature);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
        }
        public async Task<int> GetTemperatureSetpoint()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GHTRTMP);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            int.TryParse(response, out int temperature);
            return temperature;
        }

        public async Task<int> ReadTemperature()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GHTRTC);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            int.TryParse(response, out int temperature);
            return temperature;
        }

        public async Task<Unit> SetPidGain(int gain)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SHTRGAIN, gain);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
        }

        public async Task<string> GetVersion()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GVER);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            return response;
        }

        public async Task<ErrorCodes> GetError()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GERR);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            Enum.TryParse(response, true, out ErrorCodes result);
            return result;
        }

        public async Task<string> GetName()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GNAME);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            return response;
        }

        public async Task<Unit> SetName(string name)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SNAME, name);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
        }

        public async Task<Unit> Reset()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.RESET);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
        }

        public async Task<Unit> Save()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SAVE);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
        }

        public async Task<IEnumerable<string>> GetCommands()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GCMDS);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);

            List<string> responses = new List<string>();
            while (responseQueue.Count > 0)
            {
                var response = responseQueue.Dequeue();
                if (string.IsNullOrEmpty(response))
                {
                    continue;
                }
                responses.Add(response);
            }
            return responses;
        }

        public async Task<Unit> SetSerialBaudRate(int baudRate)
        {
            throw new NotImplementedException();
        }

        public async Task<Unit> SetFrequency(int address, int frequency)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SRFFRQ, address.ToString(), frequency.ToString());
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
        }

        public async Task<int> GetFrequencySetting(int address)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GRFFRQ, address);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            int.TryParse(response, out int frequencySetting);
            return frequencySetting;
        }

        public async Task<Unit> SetRfDriveSetting(int address, int drive)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SRFDRV, address.ToString(), drive.ToString());
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
        }

        public async Task<int> GetRfDriveSetting(int address)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GRFFRQ, address);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();
            int.TryParse(response, out int rfDriveSetting);
            return rfDriveSetting;
        }

        public async Task<int> GetNumberRfChannels()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.GCHAN, Module.RF.ToString());
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();


            int.TryParse(response, out int channels);
            return channels;
        }

        public async Task<Unit> AbortTimeTable()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.TBLABRT);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
        }

        /// <summary>
        /// Starts execution of time table.
        /// </summary>
        /// <returns></returns>
        public async Task<Unit> StartTimeTable()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.TBLSTRT);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
        }

        public string LastTable { get; private set; }
        public async Task<string> ReportExecutionStatus()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.TBLRPT);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue(true);
            var response = responseQueue.Dequeue();

            return response;
        }

        /// <summary>
        /// Sets the table mode for the amps / mips box.
        /// </summary>
        public async Task<Unit> SetMode(Modes mode)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.SMOD, mode.ToString());
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();
            return Unit.Default;
        }
        /// <summary>
        /// Stop the time table of the device.
        /// </summary>
        /// <returns></returns>
        public async Task<Unit> StopTable()
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.TBLSTOP);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();
            return Unit.Default;
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
            var ampsmessage = AmpsMessage.Create(AmpsCommand.STBLDAT, table);
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
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
            var ampsmessage = AmpsMessage.Create(AmpsCommand.STBLCLK, clockType.ToString());
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
        }

        /// <summary>
        /// Set the device trigger.
        /// </summary>
        /// <param name="startTrigger"></param>
        /// <returns></returns>
        public async Task<Unit> SetTrigger(StartTrigger startTrigger)
        {
            var ampsmessage = AmpsMessage.Create(AmpsCommand.STBLTRG, startTrigger.ToString());
            messageQueue.Enqueue(ampsmessage);
            await ProcessQueue();

            return Unit.Default;
        }



        #endregion
    }
}