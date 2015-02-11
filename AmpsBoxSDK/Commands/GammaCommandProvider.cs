// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GammaCommandProvider.cs" company="">
//   
// </copyright>
// <summary>
//   Major departure from Beta and Alpha commands.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Commands
{
    /// <summary>
    /// Major departure from Beta and Alpha commands.
    /// </summary>
    public class GammaCommandProvider : AmpsCommandProvider
    {
        #region Constants

        /// <summary>
        /// Sets the AMPS to use External Clock
        /// </summary>
        private const string CommandClockSyncExternal = "STBLCLK,EXT";

        /// <summary>
        /// Sets the AMPS to use Internal Clock
        /// </summary>
        private const string CommandClockSyncInternal = "STBLCLK,INT";

	    private const string CommandSetTrigger = "STBLTRG";

		/// <summary>
		/// TODO The command_ h v_ ge t_ channels.
		/// </summary>
		private const string CommandHvGetChannels = "GCHAN,DCB";


        /// <summary>
        /// TODO The command h v_ ge t_ voltage.
        /// </summary>
        private const string CommandHvGetVoltage = "GDCB";

        /// <summary>
        /// The command get dc guard offset status.
        /// </summary>
        private const string CommandGetDcGuardOffsetStatus = "GDCG";

        /// <summary>
        /// TODO The command h v_ se t_ voltage.
        /// </summary>
        private const string CommandSetDcBias = "SDCB";


        /// <summary>
        /// TODO The command r f_ get_ channels.
        /// </summary>
        private const string CommandRfGetChannels = "GCHAN,RF";

        /// <summary>
        /// TODO The command r f_ get_ drivelevel.
        /// </summary>
        private const string CommandRfGetDrivelevel = "GRFDRV";

        /// <summary>
        /// TODO The command r f_ get_ frequency.
        /// </summary>
        private const string CommandRfGetFrequency = "GRFFRQ";

        /// <summary>
        /// TODO The command r f_ set_ drivelevel.
        /// </summary>
        private const string CommandRfSetDrivelevel = "SRFDRV";

        /// <summary>
        /// 
        /// </summary>
        private const string CommandRfSetFrequency = "SRFFRQ";

        /// <summary>
        /// TODO The command_ r f_ set_ voltage.
        /// </summary>
        private const string CommandRfSetVoltage = "SRFAUTO";

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
        private const string CommandTimeTableFormatter = "STBLDAT;{1};";

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
        private const string GetDcBiasVoltageReadbackActual = "GDCBV";

        /// <summary>
        /// TODO The get digital io direction.
        /// </summary>
        private const string GetDigitalIoDirection = "GDIODR";


        /// <summary>
        /// TODO The get digital io state.
        /// </summary>
        private const string GetDigitalIoState = "GDIO";


        /// <summary>
        /// TODO The get heater read back.
        /// </summary>
        private const string GetHeaterReadback = "GHTRTC";

        /// <summary>
        /// TODO The get number dc bias channels.
        /// </summary>
        private const string GetNumberDcBiasChannels = "GCHAN";


        /// <summary>
        /// TODO The read negative hv.
        /// </summary>
        private const string ReadNegativeHv = "GNHV";

        /// <summary>
        /// TODO The read positive hv.
        /// </summary>
        private const string ReadPositiveHv = "GPHV";

        /// <summary>
        /// TODO The set dc bias offset voltage.
        /// </summary>
        private const string SetDcBiasOffsetVoltage = "SDCBOF";

        /// <summary>
        /// TODO The set digital io.
        /// </summary>
        private const string SetDigitalIo = "SDIO";

        /// <summary>
        /// TODO The set digital io direction.
        /// </summary>
        private const string SetDigitalIoDirection = "SDIODR";


        /// <summary>
        /// TODO The set heater set point.
        /// </summary>
        private const string SetHeaterSetPoint = "SHTRTMP";

        /// <summary>
        /// TODO The set negative hv.
        /// </summary>
        private const string SetNegativeHv = "SNHV";


        /// <summary>
        /// TODO The set positive hv.
        /// </summary>
        private const string SetPositiveHv = "SPHV";


        /// <summary>
        /// TODO The supported version.
        /// </summary>
        private const string SupportedVersion = "v2.0b";

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
            return SupportedVersion;
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
            this.m_commands.Add(type, new AmpsCommand(type, command));
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
                AmpsCommandType.TimeTableClockSycnInternal, 
                CommandClockSyncInternal);
            this.AddCommand(
                AmpsCommandType.TimeTableClockSyncExternal, 
                CommandClockSyncExternal);

            this.AddCommand(AmpsCommandType.Save, CommandSave);
            this.AddCommand(AmpsCommandType.SetOutputDriveLevel, CommandRfSetDrivelevel);
            this.AddCommand(AmpsCommandType.SetRfFrequency, CommandRfSetFrequency);
            this.AddCommand(AmpsCommandType.SetRfVoltage, CommandRfSetVoltage);
            this.AddCommand(AmpsCommandType.SetDriveLevel, CommandRfSetDrivelevel);
            this.AddCommand(AmpsCommandType.SetDcBias, CommandSetDcBias);

            this.AddCommand(AmpsCommandType.GetVersion, CommandVersion);
            this.AddCommand(AmpsCommandType.GetRfFrequency, CommandRfGetFrequency);
            this.AddCommand(AmpsCommandType.GetRfChannels, CommandRfGetChannels);
            this.AddCommand(AmpsCommandType.GetDriveLevel, CommandRfGetDrivelevel);
            this.AddCommand(AmpsCommandType.GetDcBias, CommandHvGetVoltage);
            this.AddCommand(AmpsCommandType.GetHighVoltageChannels, CommandHvGetChannels);

            this.AddCommand(AmpsCommandType.ToggleHeater, ToggleHeater);
            this.AddCommand(AmpsCommandType.SetHeaterSetpoint, SetHeaterSetPoint);
            this.AddCommand(AmpsCommandType.GetHeaterTemperature, GetHeaterReadback);
            this.AddCommand(AmpsCommandType.SetPositiveHV, SetPositiveHv);
            this.AddCommand(AmpsCommandType.SetNegativeHV, SetNegativeHv);

            this.AddCommand(AmpsCommandType.SetDigitalIo, SetDigitalIo);
            this.AddCommand(AmpsCommandType.GetDigitalIo, GetDigitalIoState);
            this.AddCommand(AmpsCommandType.SetDigitalIoDirection, SetDigitalIoDirection);
            this.AddCommand(AmpsCommandType.GetDigitalIoDirection, GetDigitalIoDirection);
			this.AddCommand(AmpsCommandType.CommandSetTrigger, CommandSetTrigger);
            this.AddCommand(AmpsCommandType.GetError, GetError);

            this.AddCommand(AmpsCommandType.TimeTableStop, TableStop);

            this.AddCommand(AmpsCommandType.Reset, Reset);
            this.AddCommand(AmpsCommandType.Test, Test);
        }

        #endregion
    }
}