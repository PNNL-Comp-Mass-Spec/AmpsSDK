using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Devices
{
    using System.ComponentModel.Composition;
    using System.IO.Ports;

    using FalkorSDK.IO.Ports;

    [InheritedExport]
    public interface IAmpsBoxCommunicator
    {
        #region Methods

        /// <summary>
        /// Write to the device.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        void Write(string command);

        string Response { get; }
        /// <summary>
        /// Determine if the response is valid.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        bool ValidateResponse(string response);
        /// <summary>
        /// Parse a reply from the AmpsBox.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="shouldValidateResponse"></param>
        /// <returns></returns>
        string ParseResponse(string response, bool shouldValidateResponse);
        /// <summary>
        /// Open communication
        /// </summary>
        /// <returns>True on success.</returns>
        void Open();
        /// <summary>
        /// Close communication.
        /// </summary>
        /// <returns>True on success.</returns>
        void Close();

        void SetSerialPortProperties(SerialPortProperties properties);

        SerialPortProperties SerialPortProperties { get; }
        #endregion

        #region Properties

        string PortName { get; }

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
