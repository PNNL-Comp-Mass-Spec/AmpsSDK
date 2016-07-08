// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsBox.cs" company="">
//   
// </copyright>
// <summary>
//   Communicates with a PNNL Amps Box
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AmpsBoxSdk.Modules;
using Infrastructure.Builders;

namespace AmpsBoxSdk.Devices
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
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

    using System.Reactive;

    using FalkorSDK.Channel;
    using FalkorSDK.Devices;

    using Infrastructure.Domain.Shared;

    using Timer = System.Timers.Timer;

    /// <summary>
    /// Communicates with a PNNL Amps Box
    /// Non shared parts creation policy so that multiple amps boxes can exist in the system at once.
    /// </summary>
    [DataContract]
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class AmpsBox : IEntity<AmpsBox>, IAmpsBox
    {
        #region Constants

        /// <summary>
        /// TODO The default_ box_ version.
        /// </summary>
        private const string ConstDefaultBoxVersion = "v3.2a";

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
        /// Firmware of the box.
        /// </summary>
        private string boxVersion;

        /// <summary>
        /// Last Table executed.
        /// </summary>
        private string lastTable;

        private int clockFrequency;

        #endregion

        #region Constructors and Destructors

       /// <summary>
       /// 
       /// </summary>
       [ImportingConstructor]
        public AmpsBox(IAmpsBoxCommunicator communicator, AmpsCommandProvider provider)
        {
           if (communicator == null)
           {
               throw new ArgumentNullException("communicator");
           }
            this.Communicator = communicator;
            this.boxVersion         = ConstDefaultBoxVersion;
            this.Emulated           = false;
            this.LatestWrite = string.Empty;
            this.LatestResponse = string.Empty;
            this.ClockFrequency = 16000000;
            this.PulseSequenceGeneratorModule = new PulseSequenceGeneratorModule(provider, communicator);
            this.StandardModule = new StandardModule(communicator, provider);
            this.DcBiasModule = new DcBiasModule(communicator, provider);
            this.DioModule = new DioModule(communicator, provider);
            this.RfDriverModule = new RfDriverModule(provider, communicator);
            this.EsiModule = new AmpsEsiModule(communicator, provider);
            this.TWaveModule = new MipsTWaveModule(communicator, provider);
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

        /// <summary>
        /// Gets or sets a value indicating whether is emulated or not.
        /// </summary>
        [DataMember]
        public bool Emulated { get; set; }

        public IAmpsBoxCommunicator Communicator { get; private set; }


        #endregion

        #region Public Methods and Operators

       

        [DataMember]
        public string LatestResponse { get; private set; }
        [DataMember]
        public string LatestWrite { get; private set; }
        [DataMember]
        public string LastTable { get; private set; }


        public IClockGenerationModule ClockGenerationModule { get; private set; }

        public IPulseSequenceGeneratorModule PulseSequenceGeneratorModule { get; private set; }

        public IDcBiasModule DcBiasModule { get; private set; }

        public IStandardModule StandardModule { get; private set; }

        public IDioModule DioModule { get; private set; }
        public IEsiModule EsiModule { get; private set; }


        public IFaimsModule FaimsModule { get; private set; }
        public IFilamentModule FilamentModule { get; private set; }
        public IMacroModule MacroModule { get; private set; }

        public IRfDriverModule RfDriverModule { get; private set; }

        public ITwaveModule TWaveModule { get; private set; }

        public IWiFiModule WiFiModule { get; private set; }

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
            ampsBoxData += string.Format("\t\tExt. Clock Freq: {0}\n", this.ClockFrequency);
            ampsBoxData += string.Format("\t\tLast Table:      {0}\n", this.lastTable);

            return ampsBoxData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetDcGuardState()
        {
            throw new NotImplementedException();
        }

       



        /// <summary>
        /// The get heater temperature.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public double GetHeaterTemperature()
        {
           // AmpsCommandType.GetHeaterTemperature;
               
            var splitResponse = this.Communicator.Response.Split(new[] { ']' });
            double temperature;
            double.TryParse(splitResponse[1], out temperature);
            return temperature;
        }

       /// <summary>
       /// Loads the AMPS Signal Table into the device and sets up the clock, trigger, and sets the AMPS box into table mode. Does not start the table.
       /// </summary>
       /// <param name="table"></param>
       /// <returns></returns>
        public IEnumerable<string> LoadAndSetupTimeTable(AmpsSignalTable table, ClockType clockType, StartTriggerTypes triggerType)
        {
            if (!table.Points.Any())
            {
                throw new ArgumentException("Table should have elements in it!");
            }


            var firstOrDefault = table[table.Points.FirstOrDefault().Name];
            if (firstOrDefault != null)
            {
                this.lastTable = firstOrDefault.Name;
            }

            var list = new List<string>();

            list.Add("\tSending Time Table Data.");
            this.PulseSequenceGeneratorModule.LoadTimeTable(table);


            list.Add("\tSetting Clock Type.");
            this.PulseSequenceGeneratorModule.SetClock(clockType);

            this.PulseSequenceGeneratorModule.SetTrigger(triggerType);

            list.Add("\tSetting mode.");
            this.PulseSequenceGeneratorModule.SetMode();

            return list.AsReadOnly();
        }

        /// <summary>
        /// Loads the AMPS Signal Table into the device and sets up the clock, trigger, and sets the AMPS box into table mode. Does not start the table.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IEnumerable<string> LoadAndSetupTimeTable(AmpsSignalTable table)
        {
            if (!table.Points.Any())
            {
                throw new ArgumentException("Table should have elements in it!");
            }


            var firstOrDefault = table[table.Points.FirstOrDefault().Name];
            if (firstOrDefault != null)
            {
                this.lastTable = firstOrDefault.Name;
            }

            var list = new List<string>();

            list.Add("\tSending Time Table Data.");
            this.PulseSequenceGeneratorModule.LoadTimeTable(table);

            list.Add("\tSetting mode.");
            this.PulseSequenceGeneratorModule.SetMode();

            return list.AsReadOnly();
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
        public void SetDcGuardState(string state)
        {
            throw new NotImplementedException();
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
        public void SetHeaterSetpoint(int temperature)
        {
           
                    //this.Communicator.Write(Formatter.BuildCommunicatorCommand(
                    //                                AmpsCommandType.SetGuardOffset,
                    //                                temperature));
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
        public void ToggleHeater(State state)
        {
           //this.Communicator.Write(Formatter.BuildCommunicatorCommand(
           //                                            AmpsCommandType.ToggleHeater,
           //                                            state));
        }

        #endregion

        public bool SameIdentityAs(AmpsBox other)
        {
            return other != null && this.Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (AmpsBox)obj;
            return this.SameIdentityAs(other);
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder().Append(this.Name).ToHashCode();
        }
    }
}