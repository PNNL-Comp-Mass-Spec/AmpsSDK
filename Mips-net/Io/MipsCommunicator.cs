using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using FalkorSDK.IO.Ports;
using Mips_net.Commands;
using RJCP.IO.Ports;


namespace Mips_net.Io
{
internal sealed class MipsCommunicator : IMipsCommunicator
    {
        #region Members

        /// <summary>
        /// Synchronization object.
        /// </summary>
        private readonly object sync = new object();     
	    private readonly byte[] _lf = Encoding.ASCII.GetBytes("\n");
		#endregion

		#region Construction and Initialization

		public MipsCommunicator(SerialPortStream port)
        {
            this.port = port;
	        this.port.BaudRate = port.BaudRate;
			this.port.PortName = port.PortName;
            this.port.NewLine = "\n";
	        this.port.ErrorReceived += PortErrorReceived;
            this.port.RtsEnable = true; // must be true for MIPS / AMPS communication.
            this.port.WriteTimeout = 250;
            this.port.ReadTimeout = 250;
            IsEmulated = false;

            messageSources = ToDecodedMessage(ToMessage(Read)).Publish(); // Only create one connection.
        }

		#endregion

		internal string ReadLine()
		{
			return this.port.ReadLine();
		}

	    /// <summary>
	    /// Writes the command to the device.
	    /// </summary>
	    /// <param name="value"></param>
	    /// <param name="separator"></param>
	    public void Write(byte[] value, string separator)
		{
			if (!port.IsOpen)
			{
				return;
			}
			lock (sync)
			{
				if (string.IsNullOrEmpty(separator)) return;

				var bytes = Encoding.ASCII.GetBytes(separator);
				foreach (var b in bytes)
				{
					port.WriteByte(b);
				}

				foreach (var b in value)
				{
					port.WriteByte(b);
				}
			}
		}
	    public void WriteHeader(MipsCommand command)
	    {
			var commandBytes = MipsCommandMap.Default.GetBytes(command);
		    if (commandBytes == null)
		    {
			    throw new NotImplementedException();
		    }

		    foreach (var commandByte in commandBytes)
		    {
			 port.WriteByte(commandByte);
		    }

		}
	    public void WriteEnd(string appendToEnd=null)
	    {
		    if (!port.IsOpen)
		    {
			    return;
		    }
		    lock (sync)
		    {
			    if (!string.IsNullOrEmpty(appendToEnd))
			    {
				    var bytes = Encoding.ASCII.GetBytes(appendToEnd);
				    foreach (var b in bytes)
				    {
					    port.WriteByte(b);
				    }
			    }
			    foreach (var b in _lf)
			    {
				    port.WriteByte(b);
			    }
		    }
		}

		public void Close()
        {
            lock (sync)
            {
                if (port.IsOpen)
                {
                    port.Close();
                    connection.Dispose();
                }
            }
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
        private void PortErrorReceived(object sender, RJCP.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            switch (e.EventType)
            {
                case RJCP.IO.Ports.SerialError.Frame:
                    System.Diagnostics.Trace.WriteLine(e.EventType.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        #region Properties

        /// <summary>
        /// Gets port open status.
        /// </summary>
        public bool IsOpen => port.IsOpen;

        /// <summary>
        /// Gets the serial port
        /// </summary>
        private SerialPortStream port;

        /// <summary>
        /// Get or set read timeout for commincator.
        /// </summary>
        public int ReadTimeout { get; set; }

        /// <summary>
        /// Get or set the read and write timeout for communicator.
        /// </summary>
        public int ReadWriteTimeout { get; set; }

        /// <summary>
        /// Get or set whether we are emulating commincation or communicating.
        /// </summary>
        public bool IsEmulated { get; set; }


        private IDisposable connection;

        public void Open()
        {
            lock (sync)
            {
                if (port.IsOpen) return;
                port.Open();
                connection = messageSources.Connect();
            }
            ;
        }

        private IObservable<byte> Read
        {
            get
            {
                return
                    Observable.FromEventPattern<EventHandler<RJCP.IO.Ports.SerialDataReceivedEventArgs>, RJCP.IO.Ports.SerialDataReceivedEventArgs>(
						h => port.DataReceived += h, h => port.DataReceived -= h).SelectMany(_ =>
                    {
                        var buffer = new byte[1024];
                        var ret = new List<byte>();
                        int bytesRead;
                        do
                        {
                            bytesRead = port.Read(buffer, 0, buffer.Length);
                            ret.AddRange(buffer.Take(bytesRead));
                        } while (bytesRead >= buffer.Length);
                        return ret;
                    });
            }
        }

        private class FillingCollection
        {
            public byte LineEnding { get; }
            public List<byte> Message { get; set; }
            public bool Complete { get; set; }

            public bool IsError { get; set; }

            public FillingCollection()
            {
                LineEnding = Encoding.ASCII.GetBytes("\n")[0];
            }
        }

        private IObservable<IEnumerable<byte>> ToMessage(IObservable<byte> input)
        {
            return input.Scan(new FillingCollection {Message = new List<byte>()}, (buffer, newByte) =>
            {

                if (buffer.Complete)
                {
                    buffer.Message.Clear();
                    buffer.Complete = false;
                    buffer.IsError = false;
                }

                if (newByte == buffer.LineEnding)
                {
                    buffer.Complete = true;
                }
                else
                    switch (newByte)
                    {
                        case 0x06:

                            break;
                        case 0x15:
                            buffer.IsError = true;
                            break;
                        case 63:
                            break;
                        case 13:
                            break;
                        default:
                            buffer.Message.Add(newByte);
                            break;
                    }
                return buffer;
            }).Where(fc => fc.Complete).Select(fc => fc.Message);
        }

        private IObservable<string> ToDecodedMessage(IObservable<IEnumerable<byte>> input)
        {
            return input.Select(bytes =>
            {
                if (bytes.Any())
                {
                    return Encoding.ASCII.GetString(bytes.ToArray());
                }
                else
                {
                    return string.Empty;
                }

            });
        }

        private IConnectableObservable<string> messageSources;

        public IObservable<string> MessageSources => messageSources;

        public void Dispose()
        {
            port?.Dispose();
            connection?.Dispose();
        }
	    

		#endregion

	}
}