// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFalkorADC.cs" company="">
//   
// </copyright>
// <summary>
//   Interface for any analog to digital converter devices. (Digitizers, etc)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using FalkorSDK.Data;

    /// <summary>
    /// Interface for any analog to digital converter devices. (Digitizers, etc)
    /// </summary>
    public interface IFalkorAdc
    {
        #region Public Properties

        /// <summary>
        /// Gets the properties.
        /// </summary>
        AnalogToDigitalConverterProperties Properties { get; }

        #endregion
    }
}