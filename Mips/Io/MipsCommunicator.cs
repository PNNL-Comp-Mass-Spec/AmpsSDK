using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Mips.Commands;

namespace Mips.Io
{
    public class MipsCommunicator : IMipsCommunicator
    {
        #region Members

        /// <summary>
        /// Synchronization object.
        /// </summary>
        private readonly object sync = new object();
        private readonly Queue<ResponseMessage> responseQueue = new Queue<ResponseMessage>();

        #endregion

        #region Construction and Initialization

        public MipsCommunicator(SerialPort port)
        {
            this.port = port;
            this.port.PortName = port.PortName;
            this.port.BaudRate = port.BaudRate;
            this.port.NewLine = "\n";
            this.port.ErrorReceived += PortErrorReceived;
            this.port.RtsEnable = true; // must be true for MIPS / AMPS communication.
            this.port.WriteTimeout = 250;
            this.port.ReadTimeout = 250;
            IsEmulated = false;

            messageSources = ToResponseMessage(ToDecodedMessage(ToMessage(Read))).Publish(); // Only create one connection.
        }

        #endregion


        /// <summary>
        /// Writes the command to the device.
        /// </summary>
        /// <param name="command"></param>
        public void Write(MipsCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            if (command.Value == null)
            {
                throw new Exception("Command value cannot be null!");
            }
            lock (sync)
            {
                port.WriteLine(command.ToString());
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


        public SerialPort Port => port;

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
                    Observable.FromEventPattern<SerialDataReceivedEventHandler, SerialDataReceivedEventArgs>(
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


        private IObservable<ResponseMessage> ToResponseMessage(IObservable<string> input)
        {
            return input.Select(s =>
            {
                // If there is a command in the queue, the MIPS box would have responded in a FIFO ordering. 
                if (responseQueue.Any())
                {
                    var response = responseQueue.Dequeue();
                    return response.WithPayload(s);
                }
                else
                {
                    return new ResponseMessage(new MipsCommand("MIPS", "MIPS")).WithPayload(s);
                }
            });
        }
        private readonly IConnectableObservable<ResponseMessage> messageSources;

        public IObservable<ResponseMessage> MessageSources => messageSources;

        public void Dispose()
        {
            port?.Dispose();
            connection?.Dispose();
        }

        #endregion

    }
}