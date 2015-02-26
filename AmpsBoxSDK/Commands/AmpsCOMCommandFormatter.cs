using AmpsBoxSdk.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Commands
{
    public class AmpsCOMCommandFormatter : IAmpsBoxFormatter
    {
        #region Members
        /// <summary>
        /// Command provider for the object.
        /// </summary>
        AmpsCommandProvider commandProvider;
        #endregion

        #region Construction and Initialization
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AmpsCOMCommandFormatter(string boxVersion)
        {
            this.commandProvider = AmpsCommandFactory.CreateCommandProvider(boxVersion);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Create a properly formatted RS232 command for the command type and command data provided.
        /// </summary>
        /// <param name="commandType">Enum command type to be created.</param>
        /// <param name="commandData">Command specific data (null, string, int, or Tuple) depending on command type param.</param>
        /// <returns></returns>
        public string BuildCommunicatorCommand(AmpsCommandType commandType, object commandData)
        {
            AmpsCommand command             = this.commandProvider.GetCommand(commandType);
            string      communicatorCommand = "";

            switch (commandType)
            {
                /// <summary>
                /// Command for the set output drive level.
                /// </summary>
                case AmpsCommandType.SetOutputDriveLevel:
                    Tuple<int, int> data                = commandData as Tuple<int,int>;
                    communicatorCommand                 = string.Format("{1}{0}{3}{0}{2:000}",
                                                                        this.commandProvider.CommandSeparator,
                                                                        command.Value,
                                                                        data.Item1,
                                                                        data.Item2);
                    break;
                /// <summary>
                /// Command for the time Table clock sycn internal.
                /// </summary>
                case AmpsCommandType.TimeTableClockSycnInternal:
                    communicatorCommand                     = command.Value;
                    break;
                    
                /// <summary>
                /// Command for the time Table clock sync external.
                /// </summary>
                case AmpsCommandType.TimeTableClockSyncExternal:
                    communicatorCommand                     = command.Value;
                    break;

                /// <summary>
                /// Command for setting the start trigger type.
                /// </summary>
		        case AmpsCommandType.CommandSetTrigger:
                    StartTriggerTypes startTriggerType      = (StartTriggerTypes)commandData;
                    communicatorCommand                     = string.Format("{0},{1}", 
                                                                            command.Value, 
                                                                            startTriggerType);
                    break;

		        /// <summary>
		        /// TODO The time Table abort.
		        /// </summary>
                case AmpsCommandType.TimeTableAbort:
                    communicatorCommand                     = command.Value;
                    break;

                case AmpsCommandType.TimeTableStop:
                    communicatorCommand                     = string.Format("{0}", command.Value);
                    break;

                /// <summary>
                /// TODO The get version.
                /// </summary>
                case AmpsCommandType.GetVersion:
                    communicatorCommand                     = command.Value;
                    break;

                /// <summary>
                /// TODO The save.
                /// </summary>
                case AmpsCommandType.Save:
                    communicatorCommand                     = command.Value;
                    break;

                /// <summary>
                /// The time table start.
                /// </summary>
                case AmpsCommandType.TimeTableStart:
                    communicatorCommand                     = string.Format("{0}", command.Value);
                    break;

                /// <summary>
                /// The mode.
                /// </summary>
                case AmpsCommandType.Mode:
                    communicatorCommand                     = string.Format("{0}", command.Value);
                    break;

                /// <summary>
                /// TODO The set RadioFrequency frequency.
                /// </summary>
                case AmpsCommandType.SetRfFrequency:
                    Tuple<int, int> rfSetParam = commandData as Tuple<int, int>;
                    communicatorCommand = string.Format(    "{1}{0}{3}{0}{2:000}",
                                                            this.commandProvider.CommandSeparator,
                                                            command.Value,
                                                            rfSetParam.Item1,
                                                            rfSetParam.Item2);
                    break;

                /// <summary>
                /// TODO The set RadioFrequency voltage.
                /// </summary>
                case AmpsCommandType.SetRfVoltage:
                    Tuple<int, int> rfVoltageParam = commandData as Tuple<int, int>;
                    communicatorCommand = string.Format(    "{1}{0}{3}{0}{2:000}",
                                                            this.commandProvider.CommandSeparator,
                                                            command.Value,
                                                            rfVoltageParam.Item1,
                                                            rfVoltageParam.Item2);
                    break;

                /// <summary>
                /// TODO The get RadioFrequency frequency.
                /// </summary>
                case AmpsCommandType.GetRfFrequency:
                    int channel             = (int)commandData;
                    communicatorCommand     = string.Format("{1}{0}{2}", 
                                                            commandProvider.CommandSeparator, 
                                                            command.Value, 
                                                            channel);
                    break;

                /// <summary>
                /// TODO The get RadioFrequency voltage.
                /// </summary>
                case AmpsCommandType.GetRfVoltage:
                    int rfVoltageChannel    = (int)commandData;
                    communicatorCommand     = string.Format("{1}{0}{2}",
                                                        this.commandProvider.CommandSeparator,
                                                        this.commandProvider.GetCommand(AmpsCommandType.GetRfVoltage).Value,
                                                        rfVoltageChannel);
                    break;

                /// <summary>
                /// TODO The get Radio Frequence channels.
                /// </summary>
                case AmpsCommandType.GetRfChannels:
                    communicatorCommand     = command.Value;
                    break;

                /// <summary>
                /// TODO Is this used? The set drive level.
                /// </summary>
                //case AmpsCommandType.SetDriveLevel:
                //    break;

                /// <summary>
                /// TODO The get drive level.
                /// </summary>
                case AmpsCommandType.GetDriveLevel:
                    int driveLevelChannel   = (int)commandData;
                    communicatorCommand     = string.Format(
                                                "{1}{0}{2}",
                                                this.commandProvider.CommandSeparator,
                                                this.commandProvider.GetCommand(AmpsCommandType.GetDriveLevel).Value,
                                                driveLevelChannel);
                    break;

                /// <summary>
                /// TODO The set high voltage.
                /// </summary>
                case AmpsCommandType.SetDcBias:
                    Tuple<int, int> dcBiasChannelVoltage    = commandData as Tuple<int,int>;
                    communicatorCommand = string.Format(    "{1}{0}{3}{0}{2:000}",
                                                            this.commandProvider.CommandSeparator,
                                                            command.Value,
                                                            dcBiasChannelVoltage.Item2,
                                                            dcBiasChannelVoltage.Item1);
                    break;

                /// <summary>
                /// TODO The get high voltage.
                /// </summary>
                case AmpsCommandType.GetDcBias:
                    int dcBiasChannel   = (int)commandData;
                    communicatorCommand = string.Format(
                            "{1}{0}{2}",
                            this.commandProvider.CommandSeparator,
                            this.commandProvider.GetCommand(AmpsCommandType.GetDcBias).Value,
                            dcBiasChannel);
                    break;

                /// <summary>
                /// TODO The get high voltage channels.
                /// </summary>
                case AmpsCommandType.GetHighVoltageChannels:
                    communicatorCommand = command.Value;
                    break;
                    
                /// <summary>
                /// Turns the "real time" mode on for the AMPS Box to listen for serial commands only.
                /// </summary>
                case AmpsCommandType.SetRTOn:
                    break;

                /// <summary>
                /// Turns the "real time" mode off for the AMPS Box to do other things while idle.
                /// </summary>
                case AmpsCommandType.SetRTOff:
                    communicatorCommand     = command.Value;
                    break;

                /// <summary>
                /// The toggle heater command creation.
                /// </summary>
                case AmpsCommandType.ToggleHeater:
                    State   heaterState = (State)commandData;
                    communicatorCommand = string.Format("{1}{0}{2}",
                                                        this.commandProvider.CommandSeparator, 
                                                        command.Value, 
                                                        heaterState);
                    break;

                /// <summary>
                /// TODO The set heater setpoint.
                /// </summary>
                case AmpsCommandType.SetHeaterSetpoint:
                    int temperature     = (int)commandData;
                    communicatorCommand = string.Format(
                        "{1}{0}{2}",
                        this.commandProvider.CommandSeparator,
                        command.Value, 
                        temperature);
                    break;

                /// <summary>
                /// TODO The get heater temperature.
                /// </summary>
                case AmpsCommandType.GetHeaterTemperature:
                    communicatorCommand = string.Format("{1}{0}", 
                                                        this.commandProvider.CommandSeparator, 
                                                        command.Value);
                    break;

                /// <summary>
                /// TODO The set positive hv.
                /// </summary>
                case AmpsCommandType.SetPositiveHV:
                    int posHV           = (int)commandData;
                    communicatorCommand = string.Format("{1}{0}{2:000}", 
                                                        this.commandProvider.CommandSeparator, 
                                                        command.Value, 
                                                        posHV);
                    break; 

                /// <summary>
                /// TODO The set negative hv.
                /// </summary>
                case AmpsCommandType.SetNegativeHV:
                    int negHV           = (int)commandData;
                    communicatorCommand = string.Format("{1}{0}{2:000}", 
                                                        this.commandProvider.CommandSeparator,
                                                        command.Value, 
                                                        negHV);
                    break; 

                /// <summary>
                /// TODO Is this used? The read positive hv.
                /// </summary>
                //case AmpsCommandType.ReadPositiveHV: break; 

                ///// <summary>
                ///// TODO Is this used? The read negative hv.
                ///// </summary>
                //case AmpsCommandType.ReadNegativeHV: break; 

                ///// <summary>
                ///// TODO Is this used? The set loop gain.
                ///// </summary>
                //case AmpsCommandType.SetLoopGain: break; 

                ///// <summary>
                ///// TODO Is this used? The set loop status.
                ///// </summary>
                //case AmpsCommandType.SetLoopStatus: break; 

                /// <summary>
                /// TODO The set digital io.
                /// </summary>
                case AmpsCommandType.SetDigitalIo:
                    Tuple<string, string>   digIOChannelData    = commandData as Tuple<string,string>;
                    communicatorCommand     = string.Format(    "{1}{0}{2}{0}{3}",
                                                                this.commandProvider.CommandSeparator,
                                                                command.Value,
                                                                digIOChannelData.Item1,
                                                                digIOChannelData.Item2);
                    break; 

                /// <summary>
                /// TODO Is this used? The get digital io.
                /// </summary>
                //case AmpsCommandType.GetDigitalIo: break; 

                /// <summary>
                /// TODO The set digital io direction.
                /// </summary>
                case AmpsCommandType.SetDigitalIoDirection:
                    Tuple<string, string> digIODirection    = commandData as Tuple<string, string>;
                    communicatorCommand                     = string.Format(    "{0}{1}{0}{2}{0}{3}",
                                                                                this.commandProvider.CommandSeparator,
                                                                                command.Value,
                                                                                digIODirection.Item1,
                                                                                digIODirection.Item2);
                    break; 

                /// <summary>
                /// TODO Is this used? The get digital io direction.
                /// </summary>
                //case AmpsCommandType.GetDigitalIoDirection: 
                //    break; 

                /// <summary>
                /// TODO Is this used?  The get number dc bias channels.
                /// </summary>
               // case AmpsCommandType.GetNumberDcBiasChannels: break; 

                /// <summary>
                /// TODO The get error.
                /// </summary>
                case AmpsCommandType.GetError:
                    communicatorCommand     = command.Value;
                    break; 

                /// <summary>
                /// The set guard offset.
                /// </summary>
                case AmpsCommandType.SetGuardOffset:
                    string state        = commandData as string;
                    communicatorCommand = string.Format("{0},{1}", command.Value, state);
                    break; 

                /// <summary>
                /// The get guard offset.
                /// </summary>
                case AmpsCommandType.GetGuardOffset:
                    communicatorCommand = string.Format("{0}", command.Value);
                    break;

                /// <summary>
                /// Reset the AMPS Box.
                /// </summary>
                case AmpsCommandType.Reset:
                    communicatorCommand     = command.Value;
                    break;

                /// <summary>
                /// Test the AMPS Box.
                /// </summary>
                case AmpsCommandType.Test:
                    communicatorCommand = command.Value;
                    break;

                /// <summary>
                /// Someone passed something we can't handle.
                /// </summary>
                default:
                    throw new NotSupportedException("Unsupported AmpsCommandType passed to AmpsCOMCommandFormatter.BuildCommunicatorCommand");
            }
            return communicatorCommand;
        }
        #endregion

        #region Properties

        #endregion        
    }
}
