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
        /// TODO The time Table clock sycn internal.
        /// </summary>
        TimeTableClockSycnInternal, 

        /// <summary>
        /// TODO The time Table clock sync external.
        /// </summary>
        TimeTableClockSyncExternal,

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
        SetRfFrequency, 

        /// <summary>
        /// TODO The set RadioFrequency voltage.
        /// </summary>
        SetRfVoltage, 

        /// <summary>
        /// TODO The get RadioFrequency frequency.
        /// </summary>
        GetRfFrequency, 

        /// <summary>
        /// TODO The get RadioFrequency voltage.
        /// </summary>
        GetRfVoltage, 

        /// <summary>
        /// TODO The get Radio Frequence channels.
        /// </summary>
        GetRfChannels, 

        /// <summary>
        /// TODO The set drive level.
        /// </summary>
        SetDriveLevel, 

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
        GetHighVoltageChannels, 

        /// <summary>
        /// Turns the "real time" mode on for the AMPS Box to listen for serial commands only.
        /// </summary>
        SetRTOn, 

        /// <summary>
        /// Turns the "real time" mode off for the AMPS Box to do other things while idle.
        /// </summary>
        SetRTOff, 

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
        /// TODO The set positive hv.
        /// </summary>
        SetPositiveHV, 

        /// <summary>
        /// TODO The set negative hv.
        /// </summary>
        SetNegativeHV, 

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
        /// TODO The get number dc bias channels.
        /// </summary>
        GetNumberDcBiasChannels, 

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

        Reset,

        Test,

        SetName,

        GetName
    }
}