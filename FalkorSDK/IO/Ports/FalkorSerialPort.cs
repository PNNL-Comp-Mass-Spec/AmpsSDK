// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FalkorSerialPort.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The falkor serial serialPortProperties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Ports
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO.Ports;
    using System.Reactive.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using FalkorSDK.Devices;

    using Newtonsoft.Json;

    using ReactiveUI;

    /// <summary>
    /// TODO The falkor serial serialPortProperties.
    /// </summary>
    [DataContract]
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FalkorSerialPort : ReactiveObject
    {
        #region Constants

        /// <summary>
        /// read timeout in milliseconds
        /// </summary>
        private const int CONST_READ_TIMEOUT = 10000;

        /// <summary>
        /// TODO The cons t_ writ e_ timeout.
        /// </summary>
        private const int CONST_WRITE_TIMEOUT = 10000;

        #endregion

        #region Fields

        /// <summary>
        /// The port.
        /// </summary>
        private SerialPort port;

        /// <summary>
        /// The baud rate.
        /// </summary>
        private int baudRate;

        /// <summary>
        /// The data bits.
        /// </summary>
        private int dataBits;

        /// <summary>
        /// The hand shake.
        /// </summary>
        private Handshake handShake;

        /// <summary>
        /// The parity.
        /// </summary>
        private Parity parity;

        /// <summary>
        /// The port name.
        /// </summary>
        private string portName;

        /// <summary>
        /// The stopbits.
        /// </summary>
        private StopBits stopbits;

        /// <summary>
        /// The read timeout.
        /// </summary>
        private int readTimeout;

        /// <summary>
        /// The write timeout.
        /// </summary>
        private int writeTimeout;

        /// <summary>
        /// The is open.
        /// </summary>
        private bool isOpen;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FalkorSerialPort"/> class.
        /// </summary>
        /// <param name="port">
        /// TODO The serialPortProperties.
        /// </param>
        public FalkorSerialPort(SerialPort port)
        {
            this.WhenAnyValue(x => x.Port).Where(x => x != null).Subscribe(this.OnNextSerialPort);
            this.Port = port;
            this.WhenAnyValue(x => x.PortName).Where(x => x != null).Subscribe(this.OnNextPortName);
            this.WhenAnyValue(x => x.ReadTimeout).Subscribe(this.OnNextReadTimeout);
            this.WhenAnyValue(x => x.StopBits).Subscribe(this.OnNextStopBits);
            this.WhenAnyValue(x => x.DataBits).Subscribe(this.OnNextDataBits);
            this.WhenAnyValue(x => x.WriteTimeout).Subscribe(this.OnNextWriteTimeout);
            this.WhenAnyValue(x => x.BaudRate).Subscribe(this.OnNextBaudRate);
            this.SerialPortProperties = new SerialPortProperties(this.Port);
        }

        /// <summary>
        /// The on next baud rate.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        private void OnNextBaudRate(int i)
        {
            this.Port.BaudRate = i;
            this.SerialPortProperties.BaudRate = i;
        }

        /// <summary>
        /// The on next write timeout.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        private void OnNextWriteTimeout(int i)
        {
            this.Port.WriteTimeout = i;
            this.SerialPortProperties.WriteTimeout = i;
        }

        /// <summary>
        /// The on next data bits.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        private void OnNextDataBits(int i)
        {
            this.Port.DataBits = i;
            this.SerialPortProperties.DataBits = i;
        }

        /// <summary>
        /// The on next stop bits.
        /// </summary>
        /// <param name="stopBits">
        /// The stop bits.
        /// </param>
        private void OnNextStopBits(StopBits stopBits)
        {
            this.Port.StopBits = stopBits;
            this.SerialPortProperties.StopBits = stopBits;
        }

        /// <summary>
        /// The on next read timeout.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        private void OnNextReadTimeout(int i)
        {
            this.Port.ReadTimeout = i;
            this.SerialPortProperties.ReadTimeout = i;
        }

        /// <summary>
        /// The on next port name.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        private void OnNextPortName(string s)
        {
            this.Port.PortName = s;
            this.SerialPortProperties.PortName = s;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FalkorSerialPort"/> class.
        /// </summary>
        [ImportingConstructor]
        public FalkorSerialPort()
        {
            this.WhenAnyValue(x => x.Port).Where(x => x != null).Subscribe(this.OnNextSerialPort);
            this.Port = new SerialPort
                              {
                                  BaudRate = SerialBaudRateFactory.Create(SerialBaudRateSetting.Baud19200), 
                                  Parity = Parity.Even, 
                                  StopBits = StopBits.One, 
                                  DataBits = 8, 
                                  Handshake = Handshake.XOnXOff, 
                                  ReadTimeout = CONST_READ_TIMEOUT, 
                                  WriteTimeout = CONST_WRITE_TIMEOUT,
                                  NewLine = "\n"
                              };

            this.WhenAnyValue(x => x.Port).Where(x => x != null).Subscribe(this.OnNextSerialPort);
            this.WhenAnyValue(x => x.PortName).Where(x => x != null).Subscribe(this.OnNextPortName);
            this.WhenAnyValue(x => x.ReadTimeout).Subscribe(this.OnNextReadTimeout);
            this.WhenAnyValue(x => x.StopBits).Subscribe(this.OnNextStopBits);
            this.WhenAnyValue(x => x.DataBits).Subscribe(this.OnNextDataBits);
            this.WhenAnyValue(x => x.WriteTimeout).Subscribe(this.OnNextWriteTimeout);
            this.WhenAnyValue(x => x.BaudRate).Subscribe(this.OnNextBaudRate);


        }

        /// <summary>
        /// The on next serial port.
        /// </summary>
        /// <param name="serialPort">
        /// The serial port.
        /// </param>
        private void OnNextSerialPort(SerialPort serialPort)
        {
            this.BaudRate = serialPort.BaudRate;
            this.DataBits = serialPort.DataBits;
            this.Handshake = serialPort.Handshake;
            this.Parity = serialPort.Parity;
            this.ReadTimeout = serialPort.ReadTimeout;
            this.WriteTimeout = serialPort.WriteTimeout;
            this.StopBits = serialPort.StopBits;
            this.SerialPortProperties = new SerialPortProperties(serialPort);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the baud rate.
        /// </summary>
        [DataMember]
        public int BaudRate
        {
            get
            {
                return this.baudRate;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.baudRate, value);
            }
        }

        /// <summary>
        /// Gets or sets the data bits.
        /// </summary>
        [DataMember]
        public int DataBits
        {
            get
            {
                return this.dataBits;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.dataBits, value);
            }
        }

        /// <summary>
        /// Gets or sets the handshake.
        /// </summary>
        [DataMember]
        public Handshake Handshake
        {
            get
            {
                return this.handShake;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.handShake, value);
            }
        }

        /// <summary>
        /// Gets the serial port properties.
        /// </summary>
        public SerialPortProperties SerialPortProperties { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is open.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return this.isOpen;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.isOpen, value);
            }
        }

        /// <summary>
        /// Gets or sets the parity.
        /// </summary>
        [DataMember]
        public Parity Parity
        {
            get
            {
                return this.parity;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.parity, value);
            }
        }

        /// <summary>
        /// Gets or sets the serialPortProperties.
        /// </summary>
        public SerialPort Port
        {
            get
            {
                return this.port;
            }

            private set
            {
                this.RaiseAndSetIfChanged(ref this.port, value);
            }
        }

        /// <summary>
        /// Gets or sets the serialPortProperties name.
        /// </summary>
        [DataMember]
        public string PortName
        {
            get
            {
                return this.portName;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.portName, value);
            }
        }

        /// <summary>
        /// Gets or sets the read timeout.
        /// </summary>
        [DataMember]
        public int ReadTimeout
        {
            get
            {
                return this.readTimeout;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.readTimeout, value);
            }
        }

        /// <summary>
        /// Gets or sets the stop bits.
        /// </summary>
        [DataMember]
        public StopBits StopBits
        {
            get
            {
                return this.stopbits;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.stopbits, value);
            }
        }

        /// <summary>
        /// Gets or sets the write timeout.
        /// </summary>
        [DataMember]
        public int WriteTimeout
        {
            get
            {
                return this.writeTimeout;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.writeTimeout, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// TODO The open.
        /// </summary>
        public void Open()
        {
            this.Port.Open();
            this.IsOpen = this.Port.IsOpen;
        }

        /// <summary>
        /// TODO The close.
        /// </summary>
        public void Close()
        {
            this.Port.Close();
            this.IsOpen = this.Port.IsOpen;
        }

        /// <summary>
        /// TODO The set serial serialPortProperties.
        /// </summary>
        /// <param name="serialPortProperties">
        /// TODO The serialPortProperties.
        /// </param>
        public void SetSerialPortProperties(SerialPortProperties serialPortProperties)
        {
            this.Port = new SerialPort
            {
                BaudRate = serialPortProperties.BaudRate, 
                Parity = serialPortProperties.Parity, 
                StopBits = serialPortProperties.StopBits, 
                DataBits = serialPortProperties.DataBits, 
                Handshake = serialPortProperties.Handshake, 
                ReadTimeout = serialPortProperties.ReadTimeout, 
                WriteTimeout = serialPortProperties.WriteTimeout
            };
        }

        #endregion
    }
}