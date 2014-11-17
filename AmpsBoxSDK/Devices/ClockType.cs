// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClockType.cs" company="">
//   
// </copyright>
// <summary>
//   Type of clock to use to help sync pulses for voltage timing
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Devices
{
    /// <summary>
    /// Type of clock to use to help sync pulses for voltage timing
    /// </summary>
    public enum ClockType
    {
        /// <summary>
        /// Externally driven clock
        /// </summary>
        External, 

        /// <summary>
        /// Internally driven clock
        /// </summary>
        Internal
    }
}