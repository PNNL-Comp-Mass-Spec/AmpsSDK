using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;

namespace Falkor.Plugin.Amps.Device
{
    [DataContract]
    public class AmpsBoxCommunicator :  IAmpsBoxCommunicator, ISerialPortCommunicator, IObservable<string>
    {
        #region Members

        /// <summary>
        /// Synchronization object.
        /// </summary>
        private readonly object sync = new object();

        private List<IObserver<string>> observers;
            #endregion

        #region Construction and Initialization

        public AmpsBoxCommunicator(SerialPort port)
        {
            this.observers = new List<IObserver<string>>();
            this.port = port;
            this.port.PortName = port.PortName;
            this.port.BaudRate = port.BaudRate;
            this.port.NewLine = "\n";
            this.port.ErrorReceived += PortErrorReceived;
            this.port.RtsEnable = true; // must be true for MIPS / AMPS communication.

            this.IsEmulated = false;
        }

        #endregion

       
        /// <summary>
        /// Writes the command to the device.
        /// </summary>
        /// <param name="command"></param>
        public IObservable<string> Write(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            return Observable.Start(() =>
            {
                string response;
                lock (this.sync)
                {
                    this.port.WriteLine(command.ToString());
                    response = this.port.ReadLine();
                    this.port.DiscardInBuffer();
                    this.port.DiscardOutBuffer();
                }
                if (!ValidateResponse(response))
                {
                    foreach (var observer in observers)
                    {
                        observer.OnError(new Exception());
                    }
                }
                return ParseResponse(response, true);
            });
        }
        /// <summary>
        /// Determine if the response is valid.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool ValidateResponse(string response)
        {
            var cleanedResponse = Regex.Replace(response, @"\d", string.Empty);
            cleanedResponse = Regex.Replace(cleanedResponse, @"\.", string.Empty);
            cleanedResponse = Regex.Replace(cleanedResponse, @"\?", string.Empty);
            cleanedResponse = Regex.Replace(cleanedResponse, @"\s", string.Empty);

            int asciiVal = 0;

            var messageRegex = Regex.Replace(cleanedResponse, @"\p{Cc}", a => $"{(byte) a.Value[0]:X2}");

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
        ///// Parses a response from the Amps Box.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="shouldValidateResponse"></param>
        /// <returns></returns>
        private string ParseResponse(string response, bool shouldValidateResponse)
        {
            if (string.IsNullOrEmpty(response))
            {
                return string.Empty;
            }

            string localStringData = response;
            localStringData = Regex.Replace(localStringData, @"\s", string.Empty);
            Regex.Replace(
                localStringData,
                @"\p{Cc}",
                a => $"[{(byte) a.Value[0]:X2}]");

            var newData = Regex.Replace(localStringData, @"\p{Cc}", string.Empty);
           
            return newData;

        }

        public void Close()
        {
            lock (this.sync)
            {
                if (this.port.IsOpen)
                {
                    this.port.Close();
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
                    throw new IOException(sender.ToString() + "IO Frame Error");
                case SerialError.Overrun:
                    throw new IOException("IO Overrun Error");
                case SerialError.RXOver:
                    throw new IOException("IO RXOver Error");
                case SerialError.RXParity:
                    throw new IOException("IO RXParity Error");
                case SerialError.TXFull:
                    throw new IOException("IO TXFull Error");
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


        public SerialPort Port { get { return this.port; } }
        public void Open()
        {
            lock (this.sync)
            {
                if (this.port.IsOpen) return;
                this.port.Open();
            };
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new Unsubscriber(observers, observer);
        }

        public IObservable<string> WhenTableFinished { get; }

        #endregion

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<string>> observers;
            private readonly IObserver<string> observer;

            public Unsubscriber(List<IObserver<string>> observers, IObserver<string> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (observer != null && observers.Contains(observer))
                    observers.Remove(observer);
            }
        }
    }


}
