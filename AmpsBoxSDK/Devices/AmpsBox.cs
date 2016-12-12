// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsBox.cs" company="">
//   
// </copyright>
// <summary>
//   Communicates with a PNNL Amps Box
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    public sealed class AmpsBox : IAmpsBox
    {
        #region Constants

        /// <summary>
        /// Emulated channel count for RF and HV testing
        /// </summary>
        private const int EmulatedChannelCount = 8;

        /// <summary>
        /// TODO The emulate d_ output.
        /// </summary>
        private const int EmulatedOutput = 100;

        #endregion
        private IAmpsBoxCommunicator communicator;

        #region Constructors and Destructors

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
            
            this.StandardModule = new StandardModule(communicator);
            this.PulseSequenceGeneratorModule = new PulseSequenceGeneratorModule(communicator, StandardModule);
            this.DcBiasModule = new DcBiasModule(communicator);
            this.DioModule = new DioModule(communicator);
            this.RfDriverModule = new RfDriverModule(communicator);
            this.EsiModule = new AmpsEsiModule(communicator);
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

       

        public IPulseSequenceGeneratorModule PulseSequenceGeneratorModule { get; }

        public IDcBiasModule DcBiasModule { get; }

        public IStandardModule StandardModule { get;  }

        public IDioModule DioModule { get;  }
        public IEsiModule EsiModule { get; }

        public IRfDriverModule RfDriverModule { get;  }

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

       /// <summary>
       /// Loads the AMPS Signal Table into the device and sets up the clock, trigger, and sets the AMPS box into table mode. Does not start the table.
       /// </summary>
       /// <param name="table"></param>
       /// <returns></returns>
        public void LoadAndSetupTimeTable(AmpsSignalTable table, ClockType clockType, StartTriggerTypes triggerType)
        {
            if (!table.Points.Any())
            {
                throw new ArgumentException("Table should have elements in it!");
            }


            var firstOrDefault = table[table.Points.FirstOrDefault()?.Name];


            this.PulseSequenceGeneratorModule.LoadTimeTable(table);

            this.PulseSequenceGeneratorModule.SetClock(clockType);

            this.PulseSequenceGeneratorModule.SetTrigger(triggerType);

            this.PulseSequenceGeneratorModule.SetMode();
        }

        /// <summary>
        /// Loads the AMPS Signal Table into the device and sets up the clock, trigger, and sets the AMPS box into table mode. Does not start the table.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public void LoadAndSetupTimeTable(AmpsSignalTable table)
        {
            if (!table.Points.Any())
            {
                throw new ArgumentException("Table should have elements in it!");
            }


            var firstOrDefault = table[table.Points.FirstOrDefault()?.Name];

            this.PulseSequenceGeneratorModule.LoadTimeTable(table);

            this.PulseSequenceGeneratorModule.SetMode();
        }
        #endregion
    }
}