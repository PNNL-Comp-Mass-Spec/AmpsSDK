using AmpsBoxSdk.Commands;
using FalkorSDK.IO.Ports;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Devices
{
    public class AmpsBoxCOMReader : IAmpsBoxCommunicator
    {
        #region Constants
        /// <summary>
        /// Read timeout in milliseconds
        /// </summary>
        private const int   ConstReadTimeout        = 5000;
        /// <summary>
        /// TODO The cons t_ writ e_ timeout.
        /// </summary>
        private const int   ConstWriteTimeout       = 5000;
        /// <summary>
        /// TODO The default_ box_ version.
        /// </summary>
        private const string ConstDefaultBoxVersion = "v2.0b";
        #endregion

        #region Members
        /// <summary>
        /// Serial port used. 
        /// </summary>
        private FalkorSerialPort                falkorPort;
        /// <summary>
        /// Synchronization object.
        /// </summary>
        private readonly object                 sync;
        /// <summary>
        /// Gets or sets the command provider for the given box in case versions are different.
        /// </summary>
        private readonly AmpsCommandProvider    commandProvider;
        /// <summary>
        /// Firmaware version running.
        /// </summary>
        private string                          boxVersion;
        #endregion

        #region Construction and Initialization
        /// <summary>
        /// Default constuctor.
        /// </summary>
        [ImportingConstructor]
        public AmpsBoxCOMReader()
        {
            this.sync               = new object();
            this.commandProvider    = AmpsCommandFactory.CreateCommandProvider(this.boxVersion);
            this.boxVersion         = ConstDefaultBoxVersion;
            this.IsEmulated         = false;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read from the device asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReadAsync(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return String.Empty;
            }

            string localStringData  = response;
            string dataToValidate   = localStringData;
            localStringData         = localStringData.Replace("\n", string.Empty);
            localStringData         = localStringData.Replace("\0", string.Empty);
            var newData             = Regex.Replace(localStringData, @"\p{Cc}", a => string.Format("[{0:X2}]", (byte)a.Value[0]));
            response                = "\tAMPS>> " + newData;

            newData                 = Regex.Replace(localStringData, @"\p{Cc}", string.Empty);
            var values              = newData.Split(new[] { "\0", "," }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length > 0)
            {
                localStringData     = values[values.Length - 1];
            }
            return localStringData;
        }
        /// <summary>
        /// Write to the device asynchronously.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<string> WriteAsync(string command)
        {
            LatestWrite = command;
            try
            {
                var buffer = System.Text.Encoding.ASCII.GetBytes(command + this.commandProvider.EndOfLine);

                await this.falkorPort.Port.BaseStream.WriteAsync(buffer, 0, buffer.Count());

                string response = await Read();

                LatestResponse = await ReadAsync(response);

                return LatestResponse;
            }
            catch (IOException ioException)
            {
                throw new IOException(ioException.Message, ioException.InnerException);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message, ex.InnerException);
            }
            catch (TimeoutException timeoutException)
            {
                throw new TimeoutException(timeoutException.Message, timeoutException.InnerException);
            }
            catch (ArgumentNullException exception)
            {
                throw new ArgumentNullException(exception.Message, exception.InnerException);
            }
        }
        /// <summary>
        /// Read from the device.
        /// </summary>
        /// <returns></returns>
        public async Task<string> Read()
        {
            // Emulate/copy readline, 
            byte[] buffer           = new byte[1024];
            int actualLength        = 0;
            string response         = String.Empty;
            string stringToReturn   = String.Empty;
            try
            {
                const int Offset    = 0;
                Stopwatch watch     = new Stopwatch();
                watch.Start();
              
                while (!stringToReturn.Contains("\r\n") && watch.ElapsedMilliseconds < 1000)
                {
                    actualLength        = await falkorPort.Port.BaseStream.ReadAsync(buffer, Offset, buffer.Length);
                    byte[] received     = new byte[actualLength];
                    Buffer.BlockCopy(buffer, Offset, received, Offset, actualLength);
                    stringToReturn      += System.Text.Encoding.ASCII.GetString(received);
                    bool validResponse  = await this.IsValidCommunicationAsync(stringToReturn);
                    if (!validResponse)
                    {
                        var error   = await this.GetError();
                        return error.ToString();
                    }

                    if (stringToReturn.Contains(this.commandProvider.EndOfLine))
                    {
                        watch.Stop();
                        return stringToReturn;
                    }
                }
                watch.Stop();
            }

            catch (InvalidOperationException ex)
            {
            }

            catch (NotSupportedException ex)
            {
            }

            catch (ArgumentNullException ex)
            {
            }

            catch (ArgumentOutOfRangeException ex)
            {
            }

            catch (ArgumentException ex)
            {
            }
            return string.Empty;
        }
        /// <summary>
        /// Write to the device.
        /// </summary>
        /// <param name="command">ASCII Command to send to the Amps Box.</param>
        /// <returns></returns>
        public string Write(string command)
        {
            return "";
        }
        /// <summary>
        /// Determine if the response is valid.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<bool> IsValidCommunicationAsync(string response)
        {
            var messageRegex = Regex.Replace(response, @"\p{Cc}", a => string.Format("{0:X2}", (byte)a.Value[0]));

            foreach (var s in messageRegex)
            {
                if (char.GetNumericValue(s) != -1)
                {
                    // 0x15 NAK ASCII code character
                    // 0x06 ACK ASCII code character
                    switch ((int)char.GetNumericValue(s))
                    {
                        case 0x15:
                            return false;
                        case 0x06:
                            return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Converts string? response into an int and then parses that to an enum. 
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ErrorCodes> GetError()
        {
            AmpsCommand command     = this.commandProvider.GetCommand(AmpsCommandType.GetError);
            string response         = await this.WriteAsync(command.Value);
            int responseCode;
            int.TryParse(response, out responseCode);
            var code                = (ErrorCodes)Enum.ToObject(typeof(ErrorCodes), responseCode);
            return code;
        }
        /// <summary>
        /// Open communication
        /// </summary>
        /// <returns>True on success.</returns>
        public bool Open()
        {
            if (this.IsEmulated)
            {
                return true;
            }

            lock (this.sync)
            {
                if (!this.falkorPort.IsOpen)
                {
                    this.falkorPort.Port.ReadTimeout = ConstReadTimeout;
                    this.falkorPort.Port.WriteTimeout = ConstWriteTimeout;
                    this.falkorPort.Open();
                }
            }
            return this.falkorPort.IsOpen;
        }
        /// <summary>
        /// Close communication
        /// </summary>
        /// <returns>True on success.</returns>
        public bool Close()
        {
            if (this.IsEmulated)
            {
                return true;
            }

            lock(this.sync) 
            {
                if (this.falkorPort.IsOpen)
                {
                    this.falkorPort.Close();
                }
            }
            return this.falkorPort.IsOpen;
        }
        #endregion

        #region Properties
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
                lock (this.sync)
                {
                    this.falkorPort = value;
                }
            }
        }
        /// <summary>
        /// Get or set read timeout for commincator.
        /// </summary>
        [DataMember]
        public int ReadTimeout { get; set; }
        /// <summary>
        /// Get or set the read and write timeout for communicator.
        /// </summary>
        [DataMember]
        public int ReadWriteTimeout { get; set; }
        /// <summary>
        /// Get or set whether we are emulating commincation or communicating.
        /// </summary>
        [DataMember]
        public bool IsEmulated { get; set; }
        /// <summary>
        /// Gets the most recent response from the Amps Box.
        /// </summary>
        [DataMember]
        public string LatestResponse { get; private set; }
        /// <summary>
        /// Get the most recent command written to the Amps Box.
        /// </summary>
        [DataMember]
        public string LatestWrite { get; private set; }
        /// <summary>
        /// Gets the most recent table taken from the Amps Box.
        /// </summary>
        [DataMember]
        public string LastTable { get; private set; }
        #endregion
    }
}
