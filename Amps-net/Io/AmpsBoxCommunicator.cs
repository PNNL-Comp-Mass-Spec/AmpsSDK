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
using Infrastructure.Io;
using SerialDataReceivedEventArgs = System.IO.Ports.SerialDataReceivedEventArgs;
using SerialError = System.IO.Ports.SerialError;
using SerialErrorReceivedEventArgs = System.IO.Ports.SerialErrorReceivedEventArgs;

namespace AmpsBoxSdk.Io
{
    internal sealed  class AmpsBoxCommunicator : IAmpsCommunicator, IDisposable
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
        private readonly SerialPort serialPort;

        private IDisposable connection;
        #endregion

        #region Construction and Initialization

        public AmpsBoxCommunicator(SerialPort serialPort)
        {
            this.serialPort = serialPort ?? throw new ArgumentNullException(nameof(serialPort));
            IsEmulated = false;
            read = Observable.FromEventPattern<SerialDataReceivedEventHandler, SerialDataReceivedEventArgs>(
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
                              }).Publish().RefCount() ;
        }


        #endregion


        /// <summary>
        /// Writes ASCII / UTF8 Encoded value to stream
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

                foreach (var b in value)
                {
                    serialPort.BaseStream.WriteByte(b);
                }
            }
        }

        public void WriteEnd(string appendToEnd = null)
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
            }
        }



        public void WriteHeader(AmpsCommand command)
        {
            var commandBytes = CommandMap.Default.GetBytes(command);
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
          
        }

        public string ReadLine()
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
        /// Get or set read timeout for communicator.
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

        private IObservable<byte> read;

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

        public IObservable<byte> Read
        {
            get
            {
                return read;
            }
        }

        private IObservable<IEnumerable<byte>> ToMessage(IObservable<byte> input)
        {
            return input.Scan(new FillingCollection(), (buffer, newByte) =>
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
                var enumerable = bytes.ToArray();
                if (enumerable.Length <= 0) return string.Empty;
                var str = Encoding.ASCII.GetString(enumerable);
                return str;
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
