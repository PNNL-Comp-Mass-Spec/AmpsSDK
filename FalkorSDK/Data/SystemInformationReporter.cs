// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemInformationReporter.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The system information reporter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;

    /// <summary>
    /// TODO The system information reporter.
    /// </summary>
    public class SystemInformationReporter
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The build application information.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string BuildApplicationInformation()
        {
            Assembly assem = Assembly.GetEntryAssembly();
            AssemblyName assemName = assem.GetName();
            Version ver = assemName.Version;
            string name = "[ApplicationInfo]\r\n";
            name += string.Format("Application {0}\r\nVersion {1}\r\n", assemName.Name, ver);

            OperatingSystem os = Environment.OSVersion;
            ver = os.Version;
            name += string.Format("Operating System: {0} ({1})\r\n", os.VersionString, ver);

            name += string.Format("Computer Name: {0}\r\n", Environment.MachineName);
            ver = Environment.Version;
            name += string.Format("CLR Version {0}\r\n", ver);

            return name;
        }

        /// <summary>
        /// TODO The build system information.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string BuildSystemInformation()
        {
            string name = "[SystemInfo]\r\n";
            name += string.Format("Machine Name = {0}\r\n", Environment.MachineName);
            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry entry = Dns.GetHostEntry(hostName);
                IPAddress[] addresses = entry.AddressList;
                int i = 0;
                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        name += string.Format("IPAddress{0} = {1}\r\n", i++, address);
                    }
                }

                name = name.TrimEnd('\r', '\n');
            }
            catch
            {
            }

            return name;
        }

        #endregion
    }
}