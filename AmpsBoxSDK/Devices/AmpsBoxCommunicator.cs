using AmpsBoxSdk.Commands;
using FalkorSDK.IO.Ports;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Devices
{
    [DataContract]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AmpsBoxCommunicator : IAmpsBoxCommunicator
    {
        #region Constants
        /// <summary>
        /// Read timeout in milliseconds
        /// </summary>
        private const int ConstReadTimeout = 5000;
        /// <summary>
        /// TODO The cons t_ writ e_ timeout.
        /// </summary>
        private const int ConstWriteTimeout = 5000;

        #endregion

        #region Members
        /// <summary>
        /// Serial port used. 
        /// </summary>
        private SerialPort falkorPort;
        /// <summary>
        /// Synchronization object.
        /// </summary>
        private readonly object sync = new object();

        private event EventHandler DataUpdatedEvent;
        /// <summary>

        private string data;
        #endregion

        #region Construction and Initialization
        /// <summary>
        /// Default constuctor.
        /// </summary>
        public AmpsBoxCommunicator()
        {
            Port = new SerialPort();
            this.Port.NewLine = "\n";
            this.Port.ErrorReceived += PortErrorReceived;
            this.Port.RtsEnable = true; // must be true for MIPS / AMPS communication.

            this.DataUpdatedEvent += AmpsBoxCommunicator_DataUpdatedEvent;
            this.IsEmulated = false;
        }

        public AmpsBoxCommunicator(SerialPort port) : this()
        {
            this.Port.PortName = port.PortName;
            this.Port.BaudRate = port.BaudRate;
        }


        private void AmpsBoxCommunicator_DataUpdatedEvent(object sender, EventArgs e)
        {
            this.LatestResponse = this.Data;
        }

        [IgnoreDataMember]
        public string Data
        {
            get
            {
                return this.data;
            }

            set
            {
                this.data = value;
                if (this.DataUpdatedEvent != null)
                {
                    DataUpdatedEvent(this, EventArgs.Empty);
                }
            }
        }

        [IgnoreDataMember]
        public string Response
        {
            get
            {
                return this.Data;
            }
        }

        #endregion

       
        /// <summary>
        /// Writes the command to the device.
        /// </summary>
        /// <param name="command"></param>
        public void Write(string command)
        {
            if (!this.Port.IsOpen)
            {
                return;
            }
            LatestWrite = command;
            try
            {
                lock (this.sync)
                {
                    this.falkorPort.WriteLine(command);
                    this.Data = ParseResponse(this.falkorPort.ReadLine(), true);
                    this.falkorPort.DiscardInBuffer();
                    this.falkorPort.DiscardOutBuffer();
                }
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
                this.falkorPort.DiscardInBuffer();
                this.falkorPort.DiscardOutBuffer();
                throw new TimeoutException(timeoutException.Message, timeoutException.InnerException);
            }
            catch (ArgumentNullException exception)
            {
                throw new ArgumentNullException(exception.Message, exception.InnerException);
            }
        }
        /// <summary>
        /// Determine if the response is valid.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool ValidateResponse(string response)
        {
            var cleanedResponse = Regex.Replace(response, @"\d", String.Empty);
           cleanedResponse = Regex.Replace(cleanedResponse, @"\.", String.Empty);
            cleanedResponse = Regex.Replace(cleanedResponse, @"\?", String.Empty);
            cleanedResponse = Regex.Replace(cleanedResponse, @"\s", string.Empty);

            int asciiVal = 0;

            var messageRegex = Regex.Replace(cleanedResponse, @"\p{Cc}", a => string.Format("{0:X2}", (byte)a.Value[0]));

            if (int.TryParse(messageRegex, out asciiVal))
            {
                switch (asciiVal)
                {
                    case 0x15:
                        return false;
                    case 0x06:
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Open communication
        /// </summary>
        /// <returns>True on success.</returns>
        public void Open()
        {
            lock (this.sync)
            {
                if (!this.falkorPort.IsOpen)
                {
                    this.falkorPort.ReadTimeout = ConstReadTimeout;
                    this.falkorPort.WriteTimeout = ConstWriteTimeout;
                    this.falkorPort.Open();
                }
            }
        }
        /// <summary>
        /// Close communication
        /// </summary>
        /// <returns>True on success.</returns>
        public void Close()
        {
            lock (this.sync)
            {
                if (this.falkorPort.IsOpen)
                {
                    this.falkorPort.Close();
                }
            }
        }

        /// <summary>
        /// Returns a new instance of the Serial Port Properties object.
        /// </summary>
        [DataMember]
        public SerialPortProperties SerialPortProperties
        {
            get
            {
                return new SerialPortProperties(this.Port);
            }
        }

        public void SetSerialPortProperties(SerialPortProperties properties)
        {
            if (this.Port.IsOpen)
            {
                return;
            }
            this.Port.PortName = properties.PortName;
            this.Port.BaudRate = properties.BaudRate;
            this.Port.DataBits = properties.DataBits;
            this.Port.Handshake = properties.Handshake;
            this.Port.Parity = properties.Parity;
            this.Port.StopBits = properties.StopBits;
        }

        public string PortName
        {
            get
            {
                return this.SerialPortProperties.PortName;
            }
        }

        [IgnoreDataMember]
        public SerialPort Interface
        {
            get
            {
                return this.Port;
            }
        }

        /// <summary>
        ///// Parses a response from the Amps Box.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="shouldValidateResponse"></param>
        /// <returns></returns>
        public string ParseResponse(string response, bool shouldValidateResponse)
        {
            if (string.IsNullOrEmpty(response))
            {
                return string.Empty;
            }
            ValidateResponse(response);

            string localStringData = response;
            localStringData = Regex.Replace(localStringData, @"\s", string.Empty);
            var newData = Regex.Replace(
                localStringData,
                @"\p{Cc}",
                a => string.Format("[{0:X2}]", (byte)a.Value[0]));

            response = "\tAMPS>> " + newData;

            newData = Regex.Replace(localStringData, @"\p{Cc}", string.Empty);
           
            return newData;

        }

        void IAmpsBoxCommunicator.Close()
        {
            this.Port.Close();
        }

        /// <summary>
        /// TODO The m_port_ error received.
        /// </summary>
        /// <param name="sender">
        /// TODO The sender.
        /// </param>
        /// <param name="e">
        /// TODO The e.
        /// </param>
        /// <exception cref="IOException">
        /// </exception>
        private void PortErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (e.EventType == SerialError.Frame)
            {
                throw new IOException("IO Frame Error");
            }

            if (e.EventType == SerialError.Overrun)
            {
                throw new IOException("IO Overrun Error");
            }

            if (e.EventType == SerialError.RXOver)
            {
                throw new IOException("IO RXOver Error");
            }

            if (e.EventType == SerialError.RXParity)
            {
                throw new IOException("IO RXParity Error");
            }

            if (e.EventType == SerialError.TXFull)
            {
                throw new IOException("IO TXFull Error");
            }
        }

        #region Properties
        /// <summary>
        /// Gets port open status.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return Port.IsOpen;
            }
        }
        /// <summary>
        /// Gets the serial port
        /// </summary>
        public SerialPort Port
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
        public string LatestResponse { get; set; }
        /// <summary>
        /// Get the most recent command written to the Amps Box.
        /// </summary>
        [DataMember]
        public string LatestWrite { get; set; }
        /// <summary>
        /// Gets the most recent table taken from the Amps Box.
        /// </summary>
        [DataMember]
        public string LastTable { get; set; }
        #endregion
    }
}
