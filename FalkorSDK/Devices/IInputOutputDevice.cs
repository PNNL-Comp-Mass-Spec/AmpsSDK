// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInputOutputDevice.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The InputOutputDevice interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System;
    using System.Collections.Generic;

    using FalkorSDK.Data.Signals;

    using ReactiveUI;

    /// <summary>
    /// TODO The InputOutputDevice interface.
    /// </summary>
    public interface IInputOutputDevice : IAnalogOutputDevice, IAnalogInputDevice, IDigitalOutputDevice, IDigitalInputDevice
    {
    }
}