﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using AmpsBoxSdk.Commands;
using FTD2XX_NET;
using Mips.Commands;
using Serilog;

namespace Mips.Io
{
    public class MipsFtdiCommunicator : IMipsCommunicator
    {
        #region Members

        /// <summary>
        /// Synchronization object.
        /// </summary>
        private readonly object sync = new object();

        private readonly byte[] _lf = Encoding.ASCII.GetBytes("\n");
        private string serialNumber;
        private FTDI ftdi;
        private IDisposable connection;
       
        #endregion

        #region Construction and Initialization

        public MipsFtdiCommunicator(string serialNumber, bool shouldEmulate)
        {
            if (string.IsNullOrEmpty(serialNumber))
            {
                throw new ArgumentNullException(nameof(serialNumber));
            }

            this.serialNumber = serialNumber;
            this.ftdi = new FTDI();
            this.ftdi.SetTimeouts(500u, 500u);
            IsEmulated = shouldEmulate;
            read = Observable.Interval(TimeSpan.FromMilliseconds(50)).Where(x => this.ftdi.IsOpen).Select(x =>
                    {
                        uint bytesToRead = 0;
                        ftdi.GetRxBytesAvailable(ref bytesToRead);
                        return bytesToRead;
                    }).Where(bytesToRead => bytesToRead > 0).Select(bytesToRead =>
                    {
                        var buffer = new byte[1024];
                        var ret = new List<byte>();
                        uint bytesRead = 0;
                        do
                        {
                            ftdi.Read(buffer, bytesToRead, ref bytesRead);
                            ret.AddRange(buffer.Take((int)bytesRead));
                        } while (bytesRead >= buffer.Length);
                        return ret;
                    }).SelectMany(x => x).Publish().RefCount(); 
        }


        #endregion


        /// <summary>
        /// Writes ASCII / UTF8 Encoded value to stream
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        public void Write(byte[] value, string separator)
        {
            if (!ftdi.IsOpen)
            {
                return;
            }
            lock (sync)
            {
                if (string.IsNullOrEmpty(separator)) return;
                var bytes = Encoding.ASCII.GetBytes(separator);
                uint bytesWritten = 0;
                while (bytesWritten < bytes.Length)
                {
                    ftdi.Write(bytes, bytes.Length, ref bytesWritten);
                }
                bytesWritten = 0;
                while (bytesWritten < value.Length)
                {
                    ftdi.Write(value, value.Length, ref bytesWritten);
                }
            }
        }

        public void WriteEnd(string appendToEnd = null)
        {
            if (!ftdi.IsOpen)
            {
                return;
            }
            lock (sync)
            {
                uint bytesWritten = 0;
                if (!string.IsNullOrEmpty(appendToEnd))
                {
                    var bytes = Encoding.ASCII.GetBytes(appendToEnd);

                    while (bytesWritten < bytes.Length)
                    {
                        ftdi.Write(bytes, bytes.Length, ref bytesWritten);
                    }
                }
                bytesWritten = 0;
                while (bytesWritten < _lf.Length)
                {
                    ftdi.Write(_lf, _lf.Length, ref bytesWritten);
                }
               
               

            }
            System.Diagnostics.Debug.Write(Environment.NewLine);
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
                uint bytesWritten = 0;
                while (bytesWritten < commandBytes.Length)
                {
                    ftdi.Write(commandBytes, commandBytes.Length, ref bytesWritten);
                }
            }
            System.Diagnostics.Debug.Write(Encoding.UTF8.GetString(commandBytes));
            Log.Information($"MIPS {(Encoding.UTF8.GetString(commandBytes))}");

        }

        public string ReadLine()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            lock (sync)
            {
                if (ftdi.IsOpen)
                {
                    ftdi.Close();
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

        public bool IsOpen => this.ftdi.IsOpen;

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
                var status = ftdi.OpenBySerialNumber(serialNumber);
                status = this.ftdi.SetBaudRate(115200);
                //status = this.ftdi.SetRTS(true);
                status = this.ftdi.SetDataCharacteristics(FTDI.FT_DATA_BITS.FT_BITS_8, FTDI.FT_STOP_BITS.FT_STOP_BITS_1, FTDI.FT_PARITY.FT_PARITY_NONE);
               // status = this.ftdi.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_XON_XOFF, 17, 19);
            }
        }

        public IObservable<byte> Read
        {
            get
            {
                return read;

            }
            //get
            //{
            //    return Observable.Interval(TimeSpan.FromMilliseconds(50)).Where(x => this.ftdi.IsOpen).Select(x =>
            //      {
            //          uint bytesToRead = 0;
            //          ftdi.GetRxBytesAvailable(ref bytesToRead);
            //          return bytesToRead;
            //      }).Where(bytesToRead => bytesToRead > 0).Select(bytesToRead =>
            //                    {
            //          var buffer = new byte[1024];
            //          var ret = new List<byte>();
            //          uint bytesRead = 0;
            //          do
            //          {
            //              ftdi.Read(buffer, bytesToRead, ref bytesRead);
            //              ret.AddRange(buffer.Take((int)bytesRead));
            //          } while (bytesRead >= buffer.Length);
            //          return ret;
            //      }).SelectMany(x => x);
            //}
        }

        private IObservable<(bool, List<byte>)> ToMessage(IObservable<byte> input)
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
            }).Where(fc => fc.Complete).Select(fc => (fc.IsError, fc.Message));
        }

        private IObservable<(bool, string)> ToDecodedMessage(IObservable<(bool, List<byte>)> input)
        {
            return input.Select(bytes =>
            {

                if (bytes.Item2.Count != 0)
                {
                   // System.Diagnostics.Debug.Write(bytes.Item1);
                    System.Diagnostics.Debug.Write(Encoding.ASCII.GetString(bytes.Item2.ToArray()));
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
            ftdi.Close();
            connection?.Dispose();
        }

        #endregion
    }
}