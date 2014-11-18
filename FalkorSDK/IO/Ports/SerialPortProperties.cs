// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerialPortProperties.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The serial port properties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Ports
{
    using System.IO.Ports;

    /// <summary>
    /// TODO The serial port properties.
    /// </summary>
    public class SerialPortProperties
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialPortProperties"/> class.
        /// </summary>
        /// <param name="port">
        /// TODO The port.
        /// </param>
        public SerialPortProperties(SerialPort port)
        {
            this.Handshake = port.Handshake;
            this.Parity = port.Parity;
            this.StopBits = port.StopBits;
            this.DataBits = port.DataBits;
            this.BaudRate = port.BaudRate;
            this.PortName = port.PortName;
            this.ReadTimeout = port.ReadTimeout;
            this.WriteTimeout = port.WriteTimeout;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialPortProperties"/> class.
        /// </summary>
        public SerialPortProperties()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the baud rate.
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// Gets or sets the data bits.
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// Gets or sets the handshake.
        /// </summary>
        public Handshake Handshake { get; set; }

        /// <summary>
        /// Gets or sets the parity.
        /// </summary>
        public Parity Parity { get; set; }

        /// <summary>
        /// Gets or sets the port name.
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// Gets or sets the read timeout.
        /// </summary>
        public int ReadTimeout { get; set; }

        /// <summary>
        /// Gets or sets the stop bits.
        /// </summary>
        public StopBits StopBits { get; set; }

        /// <summary>
        /// Gets or sets the write timeout.
        /// </summary>
        public int WriteTimeout { get; set; }

        #endregion
    }
}