using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using AmpsBoxSdk.IO;
using AmpsBoxSdk;

namespace AmpsBoxLib
{
    /// <summary>
    /// Communicates with a PNNL Amps Box
    /// </summary>
    public class AmpsBox
    {
        /// <summary>
        /// Fired when an error message is sent from the device.
        /// </summary>
        public event EventHandler<AmpsResponseEventArgs> ErrorOccured;

        /// <summary>
        /// 16 MHz internal clock of the AMPS box by default
        /// </summary>
        public const int DEFAULT_INTERNAL_CLOCK      = 16000000;
        /// <summary>
        /// Value required by the AMPS box to convert the clock to ticks
        /// </summary>
        private const int CONST_CLOCK_DIVIDER               = 1024;

        private const string RESPONSE_ACK                   = "ack";
        private const string RESPONSE_NAK                   = "nak";
        private const string RESPONSE_ERROR                 = "err";

        /// <summary>
        /// Time table command.  Notice that this is a format string.
        /// </summary>
        private const string COMMAND_TIME_TABLE_FORMATTER    = "TABLE,{0};{1}";        
        /// <summary>
        /// Trigger start of a time table from software
        /// </summary>
        private const string COMMAND_TIME_TABLE_START_SW     = "TRG,SW";
        /// <summary>
        /// Trigger start of a time table externally
        /// </summary>
        private const string COMMAND_TIME_TABLE_START_EXT    = "TRG,EXT";
        /// <summary>
        /// Sets the AMPS to use Internal Clock
        /// </summary>
        private const string COMMAND_CLOCK_SYNC_INTERNAL     = "CLK,INT";
        /// <summary>
        /// Sets the AMPS to use External Clock
        /// </summary>
        private const string COMMAND_CLOCK_SYNC_EXTERNAL     = "CLK,EXT";
        /// <summary>
        /// Aborts any time table command.
        /// </summary>
        private const string COMMAND_TIME_TABLE_ABORT        = "ABORT";
        /// <summary>
        /// Sets the mode of the time table (0 = run table forever, an integer would be to repeat N times)
        /// </summary>
        private const string COMMAND_TIME_TABLE_MODE         = "MODE";
        /// <summary>
        /// Version
        /// </summary>
        private const string COMMAND_VERSION                = "GVR";
        /// <summary>
        /// Saves the parameters to FLASH RAM on the AMPS Box
        /// </summary>
        private const string COMMAND_SAVE                   = "SAV";

        // RF Comands 
        /// <summary>
        /// 
        /// </summary>
        private const string COMMAND_RF_SET_FREQUENCY       = "SFA";
        private const string COMMAND_RF_SET_VOLTAGE         = "SOV";
        private const string COMMAND_RF_SET_DRIVELEVEL      = "SOD";

        private const string COMMAND_RF_GET_FREQUENCY       = "RFA";
        private const string COMMAND_RF_GET_VOLTAGE         = "ROV";
        private const string COMMAND_RF_GET_DRIVELEVEL      = "ROD";
        private const string COMMAND_RF_GET_CHANNELS        = "NRF";

        // HV Comands 
        private const string COMMAND_HV_SET_VOLTAGE         = "SCH";
        private const string COMMAND_HV_GET_VOLTAGE         = "RCH";
        private const string COMMAND_HV_GET_CHANNELS        = "NCH";        

        private const int    CONST_SLEEP_TIME               = 1000;
        private const string COMMAND_PARAMETER_SEPARATOR    = ",";
        private const string EOL                            = ";";

        /// <summary>
        /// Synchronization object.
        /// </summary>
        private object m_sync;
        /// <summary>
        /// Last table executed.
        /// </summary>
        private string m_lastTable = "";

        public AmpsBox()
        {
            m_sync          = new object();
            Port            = new SerialPort();
            Port.BaudRate   = 9600;
            Port.Parity     = Parity.None;
            Port.StopBits   = StopBits.One;
            Port.DataBits   = 8;
            Port.PortName   = "COM4";
            Port.Handshake  = Handshake.XOnXOff;
            ClockType       = ClockType.Internal;
            TriggerType     = StartTriggerTypes.Software;
            Emulated        = false;

            ClockFrequency = DEFAULT_INTERNAL_CLOCK;
        }

        /// <summary>
        /// Sets whether is emulated or not.
        /// </summary>
        public bool Emulated { get; set; }

        /// <summary>
        /// Gets or sets the clock type for executing a time table.
        /// </summary>
        public ClockType ClockType { get; set; }
        /// <summary>
        /// Gets or sets the trigger type for starting a time table
        /// </summary>
        public StartTriggerTypes TriggerType { get; set; }
        /// <summary>
        /// Gets or sets the clock frequency of the AMPS Box.
        /// </summary>        
        public int ClockFrequency { get; set; }
        /// <summary>
        /// Gets or sets the serial port.
        /// </summary>
        public SerialPort Port
        {
            get;
            private set;
        }
        /// <summary>
        /// Opens the port
        /// </summary>
        public void Open()
        {
            lock (m_sync)
            {
                if (!Port.IsOpen)
                    Port.Open();
            }
        }
        /// <summary>
        /// Closes the port
        /// </summary>
        public void Close()
        {
            lock (m_sync)
            {
                if (Port.IsOpen)
                    Port.Close();
            }
        }
        /// <summary>
        /// Gets the version of the AMPS box.
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            string data = "";
            lock (m_sync)
            {
                data = WriteRead(COMMAND_VERSION);
            }
            return data;
        }
        private void Write(string command)
        {
            Port.WriteLine(command);
        }
        private string WriteRead(string command, int sleep=CONST_SLEEP_TIME)
        {
            if (Emulated)
            {
                Console.WriteLine("\t\tSIMULATED: >> {0}", command + EOL);
                return RESPONSE_ACK;
            }

            Port.Write(command + EOL);
            if (sleep > 0)
            {
                Thread.Sleep(sleep);
            }
            string data     = Port.ReadExisting();
            data            = data.Replace("\n","");
            data            = data.Replace("\0","");

            string [] values = data.Split(new string [] {";"}, StringSplitOptions.RemoveEmptyEntries);            
            if (values.Length > 0)
            {
                data = values[values.Length - 1];
            }
            return data;
        }
        /// <summary>
        /// Saves parameters on the AMPS Box.
        /// </summary>
        public void SaveParameters()
        {
            Write(COMMAND_SAVE);
        }
        /// <summary>
        /// Validate a response
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool WasResponseOk(string message)
        {
            string response = message.ToLower();
            response        = response.Trim();

            bool isValid    = false;
            switch (response)
            {
                case "err":
                    isValid = false;                    
                    if (ErrorOccured != null)
                    {
                        ErrorOccured(this, new AmpsResponseEventArgs("The device sent an error message."));
                    }
                    break;
                case "ack":
                    isValid = true;
                    break;
                case "nak":
                    isValid = false;                    
                    break;
                default:
                    isValid = true;
                    break;
            }

            return isValid;
        }
        /// <summary>
        /// Sets the RF frequency for the channel provided.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="frequency"></param>
        public bool SetRfFrequency(int channel, int frequency)
        {
            string response = WriteRead(string.Format("{1}{0}{3}{0}{2:000}", COMMAND_PARAMETER_SEPARATOR,
                                                 COMMAND_RF_SET_FREQUENCY,
                                                 frequency,
                                                 channel));

            return WasResponseOk(response);         
        }

        public bool SetHvOutput(int channel, int voltage)
        {
            string response =  WriteRead(string.Format("{1}{0}{3}{0}{2:000}", COMMAND_PARAMETER_SEPARATOR,
                                                 COMMAND_HV_SET_VOLTAGE,
                                                 voltage,
                                                 channel));

            return WasResponseOk(response);   
        }
        /// <summary>
        /// Retrieves from the device how many HV DC Power Supplies are available.
        /// </summary>
        /// <returns></returns>
        public int GetHvChannelCount()
        {
            string response = WriteRead(COMMAND_HV_GET_CHANNELS);

            bool isValid = WasResponseOk(response);
            if (!isValid)
            {
                throw new AmpsCommandNotRecognized("The command was not recognized by the device.");
            }
            return Convert.ToInt32(response);
        }
        /// <summary>
        /// Retrieves from the device how many HV DC Power Supplies are available.
        /// </summary>
        /// <returns></returns>
        public int GetRfChannelCount()
        {
            string response = WriteRead(COMMAND_RF_GET_CHANNELS);

            bool isValid = WasResponseOk(response);
            if (!isValid)
            {
                throw new AmpsCommandNotRecognized("The command was not recognized by the device.");
            }
            return Convert.ToInt32(response);
        }
        /// <summary>
        /// Retrieves the HV output (currently read)
        /// </summary>
        /// <param name="channel"></param>
        public int GetHvOutput(int channel)
        {
            string response = WriteRead(string.Format("{1}{0}{2}", COMMAND_PARAMETER_SEPARATOR,
                                                 COMMAND_HV_GET_VOLTAGE,
                                                 channel));
            
            bool isValid = WasResponseOk(response);
            if (!isValid)
            {
                throw new AmpsCommandNotRecognized("The command was not recognized by the device.");
            }
            else
            {
                string[] data = response.Split(',');
                if (data.Length < 3)
                {
                    throw new AmpsCommandNotRecognized("The device did not send back enough data (set point, actual, current).");
                }
                return Convert.ToInt32(data[1]);
            }
            throw new AmpsCommandNotRecognized("The device sent back an invalid value.");
        }

        public int GetDriveLevel(int channel)
        {
            string response = WriteRead(string.Format("{1}{0}{2}", COMMAND_PARAMETER_SEPARATOR,
                                                 COMMAND_RF_GET_DRIVELEVEL,
                                                 channel));

            bool isValid = WasResponseOk(response);
            if (!isValid)
            {
                throw new AmpsCommandNotRecognized("The command was not recognized by the device.");
            }
            return Convert.ToInt32(response);
        }

        public int GetOutputVoltage(int channel)
        {
            string response = WriteRead(string.Format("{1}{0}{2}", COMMAND_PARAMETER_SEPARATOR,
                                                 COMMAND_RF_GET_VOLTAGE,
                                                 channel));

            bool isValid = WasResponseOk(response);
            if (!isValid)
            {
                throw new AmpsCommandNotRecognized("The command was not recognized by the device.");
            }
            return Convert.ToInt32(response);
        }

        public int GetRfFrequency(int channel)
        {
            string response = WriteRead(string.Format("{1}{0}{2}", COMMAND_PARAMETER_SEPARATOR,
                                                 COMMAND_RF_GET_FREQUENCY,
                                                 channel));

            bool isValid = WasResponseOk(response);
            if (!isValid)
            {
                throw new AmpsCommandNotRecognized("The command was not recognized by the device.");
            }
            return Convert.ToInt32(response);
        }

        public void SetRfDriveLevel(int channel, int voltage)
        {
            string response = WriteRead(string.Format("{1}{0}{3}{0}{2:000}", COMMAND_PARAMETER_SEPARATOR,
                                                 COMMAND_RF_SET_DRIVELEVEL,
                                                 voltage,
                                                 channel));

            bool isValid = WasResponseOk(response);
            if (!isValid)
            {
                throw new AmpsCommandNotRecognized("The command was not recognized by the device.");
            }            
        }

        public void SetRfOutputVoltage(int channel, int voltage)
        {
            string response = WriteRead(string.Format("{1}{0}{3}{0}{2:000}", COMMAND_PARAMETER_SEPARATOR,
                                                 COMMAND_RF_SET_VOLTAGE,
                                                 voltage,
                                                 channel));

            bool isValid = WasResponseOk(response);
            if (!isValid)
            {
                throw new AmpsCommandNotRecognized("The command was not recognized by the device.");
            }
        }


        /// <summary>
        /// Given a table, formats the table into 
        /// </summary>
        /// <param name="table"></param>
        public void LoadTimeTable(AmpsSignalTimeTable table)
        {           
            AmpsClockConverter converter               = new AmpsClockConverter(ClockFrequency);
            ISignalTableFormatter<double> formatter    = new AmpsBoxSignalTableCommandFormatter();

            string command = formatter.FormatTable(table, converter);

            // Writes the command to the device.
            string data = "";
            lock (m_sync)
            {
                data = WriteRead(command);
            }
            bool wasOk = WasResponseOk(data);
            if (!wasOk)
            {
                // THROW ERROR
            }
        }
        /// <summary>
        /// Aborts the current time table.
        /// </summary>
        public void AbortTimeTable()
        {
            WriteRead(COMMAND_TIME_TABLE_ABORT);
        }
        /// <summary>
        /// Tells the AMPS box which clock to use: external or internal
        /// </summary>
        /// <param name="clockType"></param>
        /// <param name="ClockFrequency"></param>
        private void SetClock(ClockType clockType)
        {
            switch (clockType)
            {
                case ClockType.External:
                    WriteRead(COMMAND_CLOCK_SYNC_EXTERNAL);
                    break;
                case ClockType.Internal:
                    WriteRead(COMMAND_CLOCK_SYNC_INTERNAL);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Tells the AMPS Box how to repeat (if at all) 
        /// </summary>
        /// <param name="iterations"></param>
        private void SetMode(int iterations)
        {
            WriteRead(string.Format("{0},{1}", COMMAND_TIME_TABLE_MODE, iterations));
        }
        /// <summary>
        /// Tells the device what trigger type to expect
        /// </summary>
        /// <param name="triggerType"></param>
        private void SetTriggerType(StartTriggerTypes triggerType)
        {
            switch (triggerType)
            {
                case StartTriggerTypes.Software:
                    WriteRead(COMMAND_TIME_TABLE_START_SW);
                    break;
                case StartTriggerTypes.External:
                    WriteRead(COMMAND_TIME_TABLE_START_EXT);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Starts the generation of a time table.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="iterations"></param>
        public void StartTimeTable(AmpsSignalTimeTable table, int iterations)
        {   
            LoadTimeTable(table);

            SetMode(iterations);

            SetClock(ClockType);      
      
            SetTriggerType(TriggerType);

            m_lastTable = table.Name;
        }


        public string GetConfig()
        {
            string data = "";
            data += string.Format("\tDevice Settings\n");
            data += string.Format("\t\tPort:        {0}\n", Port.PortName);
            data += string.Format("\t\tBaud:        {0}\n", Port.BaudRate);
            data += string.Format("\t\tHandshake:   {0}\n", Port.Handshake);
            data += string.Format("\t\tStopBits:    {0}\n", Port.StopBits);
            data += string.Format("\t\tParity:      {0}\n", Port.Parity);
            data += string.Format("\t\tIs Open:     {0}\n", Port.IsOpen);
            data += "\n";
            data += string.Format("\tTable Settings\n");
            data += string.Format("\t\tTrigger:         {0}\n", TriggerType);
            data += string.Format("\t\tClock:           {0}\n", ClockType);
            data += string.Format("\t\tExt. Clock Freq: {0}\n", ClockFrequency);
            data += string.Format("\t\tLast Table:      {0}\n", m_lastTable);

            return data;
        }
    }

    /// <summary>
    /// This exception class is thrown when a command to the AMPS box was not understood.
    /// </summary>
    public class AmpsCommandNotRecognized : Exception
    {
        public AmpsCommandNotRecognized(string message) :
            base(message)
        {
        }
    }

    public class AmpsResponseEventArgs: EventArgs
    {
        public AmpsResponseEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }

    /// <summary>
    /// Trigger types for starting a time table
    /// </summary>
    public enum StartTriggerTypes
    {
        Software,
        External
    }

    /// <summary>
    /// Type of clock to use to help sync pulses for voltage timing
    /// </summary>
    public enum ClockType
    {
        /// <summary>
        /// Externally driven clock
        /// </summary>
        External,
        /// <summary>
        /// Internally driven clock
        /// </summary>
        Internal
    }
}
