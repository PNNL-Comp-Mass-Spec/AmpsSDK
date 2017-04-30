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
using RJCP.IO.Ports;
using SerialDataReceivedEventArgs = System.IO.Ports.SerialDataReceivedEventArgs;
using SerialError = System.IO.Ports.SerialError;
using SerialErrorReceivedEventArgs = System.IO.Ports.SerialErrorReceivedEventArgs;

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

        /// <summary>
        /// Gets the serial port
        /// </summary>
        private readonly SerialPortStream serialPort;
        #endregion

        #region Construction and Initialization

        public AmpsBoxCommunicator(SerialPortStream serialPort)
        {
            this.serialPort = serialPort ?? throw new ArgumentNullException(nameof(serialPort));
            IsEmulated = false; 
        }

        private void SerialPort_ErrorReceived(object sender, RJCP.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            throw new Exception(e.EventType.ToString());
        }

        #endregion


        /// <summary>
        /// Writes ASCII / UTF8 Encoded value to stream
        /// </summary>
        /// <param name="value"></param>
        internal void Write(byte[] value, string separator)
        {
            if (!serialPort.IsOpen)
            {
                return;
            }
            lock (sync)
            {
                if (string.IsNullOrEmpty(separator)) return;
                serialPort.DiscardInBuffer(); // ensure that no reply is sitting in queue! 
                var bytes = Encoding.ASCII.GetBytes(separator);
                foreach (var b in bytes)
                {
                    serialPort.WriteByte(b);
                }

                foreach (var b in value)
                {
                    serialPort.WriteByte(b);
                }
            }
        }

        internal void WriteEnd(string appendToEnd = null)
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
                        serialPort.WriteByte(b);
                    }
                }
                foreach (var b in _lf)
                {
                    serialPort.WriteByte(b);
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
                serialPort.WriteByte(commandByte);
            }
        }

        internal string ReadLine()
        {
            return this.serialPort.ReadLine();
        }

        public void Close()
        {
            lock (sync)
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                connection.Dispose();
                connection = null;
            }
        }

        #region Properties

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

        private IObservable<byte> Read
        {
            get
            {
                return
                          Observable.FromEventPattern<EventHandler<RJCP.IO.Ports.SerialDataReceivedEventArgs>, RJCP.IO.Ports.SerialDataReceivedEventArgs>(
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
            public byte[] LineEnding { get; }
            public List<byte> Message { get; set; }
            public bool Complete { get; set; }

            public bool IsError { get; set; }

            public FillingCollection()
            {
                LineEnding = Encoding.ASCII.GetBytes("\r\n");
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

                if (newByte == buffer.LineEnding[1])
                {
                    buffer.Complete = true;
                }
                else if (newByte == buffer.LineEnding[0])
                {
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
                    var str = Encoding.ASCII.GetString(bytes.ToArray());
                   return str;
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
            serialPort?.Dispose();
            connection?.Dispose();
        }

        #endregion

    }


}
