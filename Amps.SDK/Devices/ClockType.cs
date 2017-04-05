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
    using System.Runtime.Serialization;

    /// <summary>
    /// Type of clock to use to help sync pulses for voltage timing
    /// </summary>
    [DataContract]
    public enum ClockType
    {
        /// <summary>
        /// Externally driven clock
        /// </summary>
        [EnumMember]
        EXT,

        /// <summary>
        /// Internally driven clock
        /// </summary>
        [EnumMember]
        INT
    }
}