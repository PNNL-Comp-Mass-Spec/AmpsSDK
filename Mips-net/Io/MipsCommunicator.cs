using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Mips.Commands;

namespace Mips.Io
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
	    private SerialPort serialPort;

		#region Construction and Initialization

		public MipsCommunicator(SerialPort port)
		{
			
			this.serialPort= port ?? throw new ArgumentNullException(nameof(port));
			IsEmulated = false;
		}

		#endregion

		internal string ReadLine()
		{
			return this.serialPort.ReadLine();
		}

	    /// <summary>
	    /// Writes the command to the device.
	    /// </summary>
	    /// <param name="value"></param>
	    /// <param name="separator"></param>
	    public void Write(byte[] value, string separator)
		{
			if (!serialPort.IsOpen)
			{
				return;
			}
			lock (sync)
			{
				if (string.IsNullOrEmpty(separator)) return;

				var bytes = Encoding.ASCII.GetBytes(separator);
				foreach (var b in bytes)
				{
					serialPort.BaseStream.WriteByte(b);
				}
				System.Diagnostics.Debug.Write(Encoding.UTF8.GetString(bytes));
				foreach (var b in value)
				{
					serialPort.BaseStream.WriteByte(b);
				}
				System.Diagnostics.Debug.Write(Encoding.UTF8.GetString(value));
			}
		}
	    public void WriteHeader(MipsCommand command)
	    {
			var commandBytes = MipsCommandMap.Default.GetBytes(command);
		    if (commandBytes == null)
		    {
			    throw new NotImplementedException();
		    }
		    lock (sync)
		    {
			    foreach (var commandByte in commandBytes)
			    {
				    serialPort.BaseStream.WriteByte(commandByte);
			    }
			}
		   System.Diagnostics.Debug.Write(Encoding.UTF8.GetString(commandBytes));

		}
	    public void WriteEnd(string appendToEnd=null)
	    {
		    if (!serialPort.IsOpen)
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
					    serialPort.BaseStream.WriteByte(b);
				    }
			    }
			    foreach (var b in _lf)
			    {
				    serialPort.BaseStream.WriteByte(b);
			    }
			    System.Diagnostics.Debug.Write(Environment.NewLine);

			}
		}

		public void Close()
        {
            lock (sync)
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                    connection.Dispose();
                }
            }
        }
		public void Open()
		{
			lock (sync)
			{

				if (messageSources == null)
				{
					messageSources = ToDecodedMessage(ToMessage(Read)).Publish(); // Only create one connection.
				}
				if (connection == null)
				{
					connection = messageSources.Connect();
				}
				if (serialPort.IsOpen) return;

				serialPort.Open();
			}
		}


        #region Properties

        /// <summary>
        /// Gets serialPort open status.
        /// </summary>
        public bool IsOpen => serialPort.IsOpen;

        /// <summary>
        /// Gets the serial serialPort
        /// </summary>
       

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

		private IObservable<byte> Read
        {
            get
            {
                return
                    Observable.FromEventPattern<SerialDataReceivedEventHandler, SerialDataReceivedEventArgs>(
                        h => serialPort.DataReceived += h, h => serialPort.DataReceived -= h).SelectMany(_ =>
                    {
                        var buffer = new byte[1024];
                        var ret = new List<byte>();
                        int bytesRead;
                        do
                        {
                            bytesRead = serialPort.Read(buffer, 0, buffer.Length);
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

        private IObservable<(bool, List<byte>)> ToMessage(IObservable<byte> input)
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
							System.Diagnostics.Trace.WriteLine("MIPS: ERROR");
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
            }).Where(fc => fc.Complete).Select(fc => (fc.IsError, fc.Message));
        }

        private IObservable<(bool, string)> ToDecodedMessage(IObservable<(bool, List<byte>)> input)
        {
            return input.Select(bytes =>
            {

				if (bytes.Item2.Count != 0)
                {
                    return (bytes.Item1, Encoding.ASCII.GetString(bytes.Item2.ToArray()));
                }
                else
                {
                    return (bytes.Item1, string.Empty);
                }

            });
        }

        private IConnectableObservable<(bool, string)> messageSources;

        public IObservable<(bool, string)> MessageSources => messageSources;

        public void Dispose()
        {
            serialPort?.Dispose();
            connection?.Dispose();
        }
	    

		#endregion

	}
}