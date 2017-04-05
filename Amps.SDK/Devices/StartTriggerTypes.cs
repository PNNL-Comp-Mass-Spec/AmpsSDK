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
        [EnumMember]
        SW,

        [EnumMember]
        BOTH,

        [EnumMember]
        EXT
    }
}