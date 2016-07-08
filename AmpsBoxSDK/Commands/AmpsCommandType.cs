// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsCommandType.cs" company="">
//   
// </copyright>
// <summary>
//   All available commands.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Commands
{
    /// <summary>
    /// All available commands.
    /// </summary>
    public enum AmpsCommandType
    {
        /// <summary>
        /// TODO The set output drive level.
        /// </summary>
        SetOutputDriveLevel, 

        /// <summary>
        /// TODO The time Table formatter.
        /// </summary>
        TimeTableFormatter, 

        /// <summary>
        /// TODO The time Table clock sync command
        /// </summary>
        TimeTableClockSync, 

		CommandSetTrigger,

		/// <summary>
		/// TODO The time Table abort.
		/// </summary>
		TimeTableAbort,

        TimeTableStop,

        /// <summary>
        /// TODO The get version.
        /// </summary>
        GetVersion, 

        /// <summary>
        /// TODO The save.
        /// </summary>
        Save, 

        /// <summary>
        /// The time table start.
        /// </summary>
        TimeTableStart, 

        /// <summary>
        /// The mode.
        /// </summary>
        Mode, 

        /// <summary>
        /// TODO The set RadioFrequency frequency.
        /// </summary>
        SetFrequency, 

        /// <summary>
        /// TODO The set RadioFrequency voltage.
        /// </summary>
        SetDriveLevel, 

        /// <summary>
        /// TODO The get RadioFrequency frequency.
        /// </summary>
        GetFrequency, 

        /// <summary>
        /// TODO The get RadioFrequency voltage.
        /// </summary>
        GetRfVoltage, 

        /// <summary>
        /// TODO The get drive level.
        /// </summary>
        GetDriveLevel, 

        /// <summary>
        /// TODO The set high voltage.
        /// </summary>
        SetDcBias, 

        /// <summary>
        /// TODO The get high voltage.
        /// </summary>
        GetDcBias, 

        /// <summary>
        /// TODO The get high voltage channels.
        /// </summary>
        GetChannels, 

        /// <summary>
        /// TODO The toggle heater.
        /// </summary>
        ToggleHeater, 

        /// <summary>
        /// TODO The set heater setpoint.
        /// </summary>
        SetHeaterSetpoint, 

        /// <summary>
        /// TODO The get heater temperature.
        /// </summary>
        GetHeaterTemperature, 

        /// <summary>
        /// TODO The read positive hv.
        /// </summary>
        ReadPositiveHV, 

        /// <summary>
        /// TODO The read negative hv.
        /// </summary>
        ReadNegativeHV, 

        /// <summary>
        /// TODO The set loop gain.
        /// </summary>
        SetLoopGain, 

        /// <summary>
        /// TODO The set loop status.
        /// </summary>
        SetLoopStatus, 

        /// <summary>
        /// TODO The set digital io.
        /// </summary>
        SetDigitalIo, 

        /// <summary>
        /// TODO The get digital io.
        /// </summary>
        GetDigitalIo, 

        /// <summary>
        /// TODO The set digital io direction.
        /// </summary>
        SetDigitalIoDirection, 

        /// <summary>
        /// TODO The get digital io direction.
        /// </summary>
        GetDigitalIoDirection, 

        /// <summary>
        /// TODO The get error.
        /// </summary>
        GetError, 

        /// <summary>
        /// The set guard offset.
        /// </summary>
        SetGuardOffset, 

        /// <summary>
        /// The get guard offset.
        /// </summary>
        GetGuardOffset,

        SetEsiVoltage,
        GetEsiVoltageSetpoint,
        GetEsiOutputVoltage,
        GetEsiChannelCurrent,
        GetEsiMaxVoltage,


        Reset,

        Test,

        SetName,

        GetName,

        TrigOut,

        GAENA,

        SAENA,

        SetDCPowerState,
        GetDcPowerState,

        GetTravellingWaveFrequency,
        SetTravellingWaveFrequency,
        GetTWaveVoltage,
        SetTWaveVoltage,
        GetGuardOneVoltage,
        SetGuardOneVoltage,
        GetGuardTwoVoltage,
        SetGuardTwoVoltage,
        GetOutputSequence,
        SetOutputSequence,
        GetTWaveOutputDirection,
        SetTWaveOutputDirection,
        ListAllCommands,
        GetCompressorOrder,
        SetCompressorOrder,
        GetCompressionTime,
        SetCompressionTime,
        GetNormalTime,
        SetNormalTime,
        GetNonCompressTime,
        SetNonCompressTime,
        ForceMultipassTrigger,
        GetSwitchState,
        SetSwitchState,
        SetTwaveCommonClock,
        SetTWaveCompressorMode,
        SetCompressorMode,
        GetCompressorMode,

    }
}