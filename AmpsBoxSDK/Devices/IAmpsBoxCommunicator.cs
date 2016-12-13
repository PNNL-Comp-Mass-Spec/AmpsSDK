using System;
using AmpsBoxSdk.Commands;

namespace AmpsBoxSdk.Devices
{
    using System.ComponentModel.Composition;

    public interface IAmpsBoxCommunicator
    {
        #region Methods

        /// <summary>
        /// Write to the device.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        IObservable<string> Write(Command command);

        IObservable<string> WhenTableFinished { get; }
        
        /// <summary>
        /// Opens connection and allows 
        /// </summary>
        /// <returns></returns>
        void Open();
        /// <summary>
        /// Close communication.
        /// </summary>
        /// <returns>True on success.</returns>
        void Close();

        #endregion

        #region Properties

        /// <summary>
        /// Get or set read timeout for commincator.
        /// </summary>
        int ReadTimeout { get; set; }
        /// <summary>
        /// Get or set the read and write timeout for communicator.
        /// </summary>
        int ReadWriteTimeout { get; set; }
        /// <summary>
        /// Get or set whether we are emulating communication
        /// </summary>
        bool IsEmulated { get; set; }
        /// <summary>
        /// Gets whether the port is open.
        /// </summary>
        bool IsOpen { get; }


        #endregion

    }
}
