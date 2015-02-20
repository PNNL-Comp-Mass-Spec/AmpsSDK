using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpsBoxSdk.Devices
{
    public interface IAmpsBoxCommunicator
    {
        #region Methods
        /// <summary>
        /// Read from the device asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<string> ReadAsync(string response);
        /// <summary>
        /// Write to the device asynchronously.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<string> WriteAsync(string command);
        /// <summary>
        /// Read from the device.
        /// </summary>
        /// <returns></returns>
        Task<string> Read();
        /// <summary>
        /// Write to the device.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        string Write(string command);
        /// <summary>
        /// Determine if the response is valid.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        Task<bool> IsValidCommunicationAsync(string response);
        /// <summary>
        /// Open communication
        /// </summary>
        /// <returns>True on success.</returns>
        bool Open();
        /// <summary>
        /// Close communication.
        /// </summary>
        /// <returns>True on success.</returns>
        bool Close();
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
        /// Get or set whether we are emulating commincation or communicating.
        /// </summary>
        bool IsEmulated { get; set; }
        #endregion

    }
}
