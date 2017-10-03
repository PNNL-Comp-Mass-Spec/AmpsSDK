using System;

namespace Mips_net.Io
{
    public interface IMipsCommunicator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        //void Write(MipsMessage command);

        IObservable<(bool, string)> MessageSources { get; }

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
		
    }
}