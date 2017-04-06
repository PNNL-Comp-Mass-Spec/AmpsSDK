using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Io
{
    internal sealed class AmpsBoxCommunicator : IDisposable
    {
        #region Members

        /// <summary>
        /// Synchronization object.
        /// </summary>
        private readonly object sync = new object();

        private readonly byte[] _lf = Encoding.ASCII.GetBytes("\n");
            #endregion

        #region Construction and Initialization

        public AmpsBoxCommunicator(SerialPort port)
        {
            this.port = port;
            this.port.PortName = port.PortName;
            this.port.BaudRate = port.BaudRate;
            this.port.NewLine = "\n";
            this.port.ErrorReceived += PortErrorReceived;
            this.port.RtsEnable = true; // must be true for MIPS / AMPS communication.
            this.port.WriteTimeout = 250;
            this.port.ReadTimeout = 250;
            this.IsEmulated = false;

            this.messageSources = ToDecodedMessage(ToMessage(this.Read)).Publish(); // Only create one connection.
        }

        #endregion


        /// <summary>
        /// Writes ASCII Encoded value to stream
        /// </summary>
        /// <param name="value"></param>
        internal void Write(byte[] value, string separator)
        {
            if (!this.port.IsOpen)
            {
                return;
            }
            lock (sync)
            {
                foreach (var b in value)
                {
                    this.port.BaseStream.WriteByte(b);
                }

                if (string.IsNullOrEmpty(separator)) return;

                var bytes = Encoding.ASCII.GetBytes(separator);
                foreach (var b in bytes)
                {
                    this.port.BaseStream.WriteByte(b);
                }
            }
        }

        internal void WriteEnd()
        {
            lock (sync)
            {
                foreach (var b in _lf)
                {
                    this.port.BaseStream.WriteByte(b);
                }
            }
        }



        internal void WriteHeader(AmpsCommand command)
        {
            

            var commandBytes = CommandMap.Default.GetBytes(command);
            if (commandBytes == null)
            {
                throw new NotImplementedException();
            }

            foreach (var commandByte in commandBytes)
            {
                this.port.BaseStream.WriteByte(commandByte);
            }
        }

        public void Close()
        {
            lock (this.sync)
            {
                if (this.port.IsOpen)
                {
                    this.port.Close();
                }
                this.connection.Dispose();
                this.connection = null;
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
        private void PortErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            switch (e.EventType)
            {
                case SerialError.Frame:
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
        private SerialPort port;

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
            lock (this.sync)
            {
                if (connection == null)
                {
                    connection = this.messageSources.Connect();
                }
                if (this.port.IsOpen) return;

                this.port.Open();
                
            }
        }

        private IObservable<byte> Read
        {
            get
            {
                return
                          Observable.FromEventPattern<SerialDataReceivedEventHandler, SerialDataReceivedEventArgs>(
                              h => this.port.DataReceived += h, h => this.port.DataReceived -= h).SelectMany(_ =>
                              {
                                  var buffer = new byte[1024];
                                  var ret = new List<byte>();
                                  int bytesRead;
                                  do
                                  {
                                      bytesRead = this.port.BaseStream.Read(buffer, 0, buffer.Length);
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
                else switch (newByte)
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

        private readonly IConnectableObservable<string> messageSources;

        public IObservable<string> MessageSources => this.messageSources;

        public void Dispose()
        {
            port?.Dispose();
            connection?.Dispose();
        }

        #endregion

    }


}
