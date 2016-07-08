// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GammaCommandProvider.cs" company="">
//   
// </copyright>
// <summary>
//   Major departure from Beta and Alpha commands.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;

namespace AmpsBoxSdk.Commands
{
    /// <summary>
    /// Major departure from Beta and Alpha commands.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class GammaCommandProvider : AmpsCommandProvider
    {
        #region Constants

        /// <summary>
        /// Sets the AMPS to use External Clock
        /// </summary>
        private const string CommandClockSync = "STBLCLK,{0}";


	    private const string CommandSetTrigger = "STBLTRG,{1}";

		/// <summary>
		/// TODO The command_ h v_ ge t_ channels.
		/// </summary>
		private const string CommandGetChannels = "GCHAN,{0}";


        /// <summary>
        /// TODO The command h v_ ge t_ voltage.
        /// </summary>
        private const string CommandHvGetVoltage = "GDCB,{0}";

        /// <summary>
        /// The command get dc guard offset status.
        /// </summary>
        private const string CommandGetDcGuardOffsetStatus = "GDCG";

        /// <summary>
        /// TODO The command h v_ se t_ voltage.
        /// </summary>
        private const string CommandSetDcBias = "SDCB,{0},{1}";

        private const string GetTableFrequency = "GTBLFRQ";


        /// <summary>
        /// TODO The command r f_ get_ drivelevel.
        /// </summary>
        private const string CommandRfGetDrivelevel = "GRFDRV,{0}";

        /// <summary>
        /// TODO The command r f_ get_ frequency.
        /// </summary>
        private const string CommandRfGetFrequency = "GRFFRQ,{0}";

        /// <summary>
        /// TODO The command r f_ set_ drivelevel.
        /// </summary>
        private const string CommandRfSetDrivelevel = "SRFDRV,{0},{1}";

        private const string SetRfLevel = "SRFVLT";

        private const string ReturnRfPositiveVpp = "GRFPPVP,{0}";

        private const string ReturnRfNegativeOutputVpp = "GRFPPVN,{0}";

        private const string GetOutputVppSetpoint = "GRFVLT,{0}";

        private const string GetChannelPowerWatts = "GRFPWR,{0}";

        /// <summary>
        /// 
        /// </summary>
        private const string CommandRfSetFrequency = "SRFFRQ,{0}";

        /// <summary>
        /// Saves the parameters to FLASH ROM on the AMPS Box
        /// </summary>
        private const string CommandSave = "SAVE";

        /// <summary>
        /// Aborts any time Table command.
        /// </summary>
        private const string CommandTimeTableAbort = "TBLABRT";

        /// <summary>
        /// Time Table command.  Notice that this is a format string.
        /// </summary>
        private const string CommandTimeTableFormatter = "STBLDAT,{0}";

        /// <summary>
        /// The command set mode.
        /// </summary>
        private const string CommandSetMode = "SMOD,TBL";

        /// <summary>
        /// The command start table.
        /// </summary>
        private const string CommandStartTable = "TBLSTRT";


        /// <summary>
        /// Version
        /// </summary>
        private const string CommandVersion = "GVER";

        /// <summary>
        /// TODO The get dc bias voltage read back actual.
        /// </summary>
        private const string GetDcBiasVoltageReadbackActual = "GDCBV,{0}";

        /// <summary>
        /// TODO The get digital io direction.
        /// </summary>
        private const string GetDigitalIoDirection = "GDIODR,{0}";


        /// <summary>
        /// TODO The get digital io state.
        /// </summary>
        private const string GetDigitalIoState = "GDIO,{0}";


        /// <summary>
        /// TODO The get heater read back.
        /// </summary>
        private const string GetHeaterReadback = "GHTRTC";

        /// <summary>
        /// TODO The set dc bias offset voltage.
        /// </summary>
        private const string SetDcBiasOffsetVoltage = "SDCBOF,{0}";

        private const string SetEsiVoltage = "SHV,{0},{1}";

        private const string GetEsiVoltageSetPoint = "GHV,{0}";

        private const string GetEsiVoltageOutput = "GHVV,{0}";

        private const string GetEsiChannelCurrent = "GHVI,{0}";

        private const string GetEsiChannelMaximumVoltage = "GHVMAX,{0}";

        /// <summary>
        /// TODO The set digital io.
        /// </summary>
        private const string SetDigitalIo = "SDIO,{0}";

        /// <summary>
        /// TODO The set digital io direction.
        /// </summary>
        private const string SetDigitalIoDirection = "SDIODR,{0}";


        /// <summary>
        /// TODO The set heater set point.
        /// </summary>
        private const string SetHeaterSetPoint = "SHTRTMP";

        /// <summary>
        /// TODO The toggle dc guard offset voltages.
        /// </summary>
        private const string ToggleDcGuardOffsetVoltages = "SDCG";

        /// <summary>
        /// TODO The toggle heater.
        /// </summary>
        private const string ToggleHeater = "SHTR";

        /// <summary>
        /// TODO The get error.
        /// </summary>
        private const string GetError = "GERR";

        private const string TableStop = "TBLSTOP";

        private const string Reset = "RESET,OUT";

        private const string Test = "TEST,OUT";

        private const string GetName = "GNAME";

        private const string SetName = "SNAME,{0}";

        private const string GetDcMinimum = "GDCMIN,{0}";

        private const string GetDcMaximum = "GDCMAX,{0}";

        private const string SetDcPower = "SDCPWR,{0}";

        private const string GetDcPower = "GDCPWR,{0}";

        private const string GetAllCommands = "GCMDS";

        private const string GetTWaveFreq = "GTWF";

        private const string SetTWaveFreq = "STWF,{0},{1}";

        private const string GetTWavePulseVoltage = "GTWPV,{0}";

        private const string SetTWavePulseVoltage = "STWPV,{0},{1}";

        private const string GetGuardOneVoltage = "GTWG1V,{0}";

        private const string SetGuardOneVoltage = "STWG1V,{0},{1}";

        private const string GetGuardTwoVoltage = "GTWG2V,{0}";

        private const string SetGuardTwoVoltage = "STWG2V,{0},{1}";

        private const string GetOutputSequence = "GTWSEQ,{0}";

        private const string SetOutputSequence = "STWSEQ,{0},{1}";

        private const string GetTWaveOutputDirection = "GTWDIR,{0}";

        private const string SetTWaveOutputDirection = "STWDIR,{0},{1}";



        #endregion

        // DIO Commands
        #region Public Methods and Operators

        /// <summary>
        /// TODO The get supported versions.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string GetSupportedVersions()
        {
            return "3.2a";
        }

        #endregion

        #region Methods

        /// <summary>
        /// TODO The add command.
        /// </summary>
        /// <param name="type">
        /// TODO The type.
        /// </param>
        /// <param name="command">
        /// TODO The command.
        /// </param>
        protected void AddCommand(AmpsCommandType type, string command)
        {
            this.m_commands.Add(type, new AmpsCommand(command));
        }

        protected void AddCommand(MipsCommandType type, string command)
        {
          //  this.mipsCommands.Add(type, new AmpsCommand());
        }

        /// <summary>
        /// TODO The generate commands.
        /// </summary>
        protected override void GenerateCommands()
        {
            this.AddCommand(AmpsCommandType.TimeTableFormatter, CommandTimeTableFormatter);
            this.AddCommand(AmpsCommandType.TimeTableStart, CommandStartTable);
            this.AddCommand(AmpsCommandType.Mode, CommandSetMode);

            this.AddCommand(AmpsCommandType.SetGuardOffset, ToggleDcGuardOffsetVoltages);
            this.AddCommand(AmpsCommandType.GetGuardOffset, CommandGetDcGuardOffsetStatus);

            this.AddCommand(AmpsCommandType.TimeTableAbort, CommandTimeTableAbort);

            this.AddCommand(
                AmpsCommandType.TimeTableClockSync, 
                CommandClockSync);

            this.AddCommand(AmpsCommandType.Save, CommandSave);
            this.AddCommand(AmpsCommandType.SetOutputDriveLevel, CommandRfSetDrivelevel);
            this.AddCommand(AmpsCommandType.SetFrequency, CommandRfSetFrequency);
            this.AddCommand(AmpsCommandType.SetDriveLevel, SetRfLevel);

            this.AddCommand(AmpsCommandType.SetDcBias, CommandSetDcBias);

            this.AddCommand(AmpsCommandType.GetVersion, CommandVersion);
            this.AddCommand(AmpsCommandType.GetFrequency, CommandRfGetFrequency);
            this.AddCommand(AmpsCommandType.GetDriveLevel, CommandRfGetDrivelevel);
            this.AddCommand(AmpsCommandType.GetDcBias, CommandHvGetVoltage);
            this.AddCommand(AmpsCommandType.GetChannels, CommandGetChannels);

            this.AddCommand(AmpsCommandType.ToggleHeater, ToggleHeater);
            this.AddCommand(AmpsCommandType.SetHeaterSetpoint, SetHeaterSetPoint);
            this.AddCommand(AmpsCommandType.GetHeaterTemperature, GetHeaterReadback);

            this.AddCommand(AmpsCommandType.SetDigitalIo, SetDigitalIo);
            this.AddCommand(AmpsCommandType.GetDigitalIo, GetDigitalIoState);
            this.AddCommand(AmpsCommandType.SetDigitalIoDirection, SetDigitalIoDirection);
            this.AddCommand(AmpsCommandType.GetDigitalIoDirection, GetDigitalIoDirection);
			this.AddCommand(AmpsCommandType.CommandSetTrigger, CommandSetTrigger);
            this.AddCommand(AmpsCommandType.GetError, GetError);

            this.AddCommand(AmpsCommandType.TimeTableStop, TableStop);

            this.AddCommand(AmpsCommandType.Reset, Reset);
            this.AddCommand(AmpsCommandType.Test, Test);

            this.AddCommand(AmpsCommandType.SetName, SetName);

            this.AddCommand(AmpsCommandType.GetName, GetName);

            this.AddCommand(AmpsCommandType.GetTravellingWaveFrequency, GetTWaveFreq);
            this.AddCommand(AmpsCommandType.SetTravellingWaveFrequency, SetTWaveFreq);
            this.AddCommand(AmpsCommandType.GetTWaveVoltage, GetTWavePulseVoltage);
            this.AddCommand(AmpsCommandType.SetTWaveVoltage, SetTWavePulseVoltage);
            this.AddCommand(AmpsCommandType.GetGuardOneVoltage, GetGuardOneVoltage);
            this.AddCommand(AmpsCommandType.SetGuardOneVoltage, SetGuardOneVoltage);
            this.AddCommand(AmpsCommandType.GetGuardTwoVoltage, GetGuardTwoVoltage);
            this.AddCommand(AmpsCommandType.SetGuardTwoVoltage, SetGuardTwoVoltage);
            this.AddCommand(AmpsCommandType.GetOutputSequence, GetOutputSequence);
            this.AddCommand(AmpsCommandType.SetOutputSequence, SetOutputSequence);
            this.AddCommand(AmpsCommandType.GetTWaveOutputDirection, GetTWaveOutputDirection);
            this.AddCommand(AmpsCommandType.SetTWaveOutputDirection, SetTWaveOutputDirection);


        }

        #endregion
    }
}