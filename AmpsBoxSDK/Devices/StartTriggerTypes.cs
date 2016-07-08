// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartTriggerTypes.cs" company="">
//   
// </copyright>
// <summary>
//   Trigger types for starting a time Table
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Devices
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Trigger types for starting a time Table
    /// </summary>
    [DataContract]
    public enum StartTriggerTypes
    {
        /// <summary>
        /// TODO The software.
        /// </summary>
        [EnumMember]
        SW,

        /// <summary>
        /// TODO The external.
        /// </summary>
        [EnumMember]
        EXT,
        [EnumMember]
        EDGE,
        [EnumMember]
        POS,
        [EnumMember]
        NEG
    }
}