// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerialBaudRateFactory.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The serial baud rate factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    /// <summary>
    /// TODO The serial baud rate factory.
    /// </summary>
    public class SerialBaudRateFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The create.
        /// </summary>
        /// <param name="baudRate">
        /// TODO The baud rate.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int Create(SerialBaudRateSetting baudRate)
        {
            int baud = 9600;
            switch (baudRate)
            {
                case SerialBaudRateSetting.Baud4800:
                    baud = 4800;
                    break;
                case SerialBaudRateSetting.Baud9600:
                    baud = 9600;
                    break;
                case SerialBaudRateSetting.Baud19200:
                    baud = 19200;
                    break;
                default:
                    baud = 9600;
                    break;
            }

            return baud;
        }

        #endregion
    }

    /// <summary>
    /// TODO The serial baud rate setting.
    /// </summary>
    public enum SerialBaudRateSetting
    {
        /// <summary>
        /// TODO The baud 4800.
        /// </summary>
        Baud4800, 

        /// <summary>
        /// TODO The baud 9600.
        /// </summary>
        Baud9600, 

        /// <summary>
        /// TODO The baud 19200.
        /// </summary>
        Baud19200
    }
}