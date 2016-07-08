// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorCodes.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ErrorCodes type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace AmpsBoxSdk.Devices
{
    using System.Runtime.Serialization;

    /// <summary>
    /// TODO The error codes.
    /// </summary>
    [DataContract]
    public enum ErrorCodes
    {
        [EnumMember]
        Nominal = 0,
        /// <summary>
        /// TODO The bad command.
        /// </summary>
        [EnumMember]
        BadCommand = 1,

        /// <summary>
        /// TODO The bad arg.
        /// </summary>
        [EnumMember]
        BadArg = 2,

        /// <summary>
        /// The local already.
        /// </summary>
        [EnumMember]
        LocalAlready = 3,

        /// <summary>
        /// The table already.
        /// </summary>
        [EnumMember]
        TableAlready = 4,

        /// <summary>
        /// The no table loaded.
        /// </summary>
        [EnumMember]
        NoTableLoaded = 5,

        /// <summary>
        /// The no table mode.
        /// </summary>
        [EnumMember]
        NoTableMode = 6,

        /// <summary>
        /// The table not ready.
        /// </summary>
        [EnumMember]
        TableNotReady = 7,

        /// <summary>
        /// The token timeout.
        /// </summary>
        [EnumMember]
        TokenTimeout = 8,

        /// <summary>
        /// The expected colon.
        /// </summary>
        [EnumMember]
        ExpectedColon = 9,

        /// <summary>
        /// The table too big.
        /// </summary>
        [EnumMember]
        TableTooBig = 10,

        /// <summary>
        /// The channel low or board.
        /// </summary>
        [EnumMember]
        ChannelLowOrBoard = 11,

        /// <summary>
        /// The channel high or board.
        /// </summary>
        [EnumMember]
        ChannelHighOrBoard = 12,

        /// <summary>
        /// The channel high.
        /// </summary>
        [EnumMember]
        ChannelHigh = 13,

        /// <summary>
        /// The board low or board.
        /// </summary>
        [EnumMember]
        BoardLowOrBoard = 14,

        /// <summary>
        /// The board high or board.
        /// </summary>
        [EnumMember]
        BoardHighOrBoard = 15,

        /// <summary>
        /// The board high.
        /// </summary>
        [EnumMember]
        BoardHigh = 16,

        /// <summary>
        /// The board not support.
        /// </summary>
        [EnumMember]
        BoardNotSupport = 17,

        /// <summary>
        /// The bad baud.
        /// </summary>
        [EnumMember]
        BadBaud = 18,

        /// <summary>
        /// The expected comma.
        /// </summary>
        [EnumMember]
        ExpectedComma = 19,

        /// <summary>
        /// The inception.
        /// </summary>
        [EnumMember]
        Inception = 20,

        /// <summary>
        /// The missing open bracket.
        /// </summary>
        [EnumMember]
        MissingOpenBracket = 21,

        /// <summary>
        /// The invalid channel.
        /// </summary>
        [EnumMember]
        InvalidChannel = 22,

        /// <summary>
        /// The dio hardware not present.
        /// </summary>
        [EnumMember]
        DioHardwareNotPresent = 23,

        /// <summary>
        /// The temperature range.
        /// </summary>
        [EnumMember]
        TemperatureRange = 24,

        /// <summary>
        /// The esi hv out of range.
        /// </summary>
        [EnumMember]
        EsiHvOutOfRange = 25,

        /// <summary>
        /// The temperature control loop gain.
        /// </summary>
        [EnumMember]
        TemperatureControlLoopGain = 26,

        [EnumMember]
        NotLocMode = 27,

        [EnumMember]
        WrongTriggerMode = 28,

        [EnumMember]
        CannotFindEntry = 29,

        [EnumMember]
        ArgumentOutOfRange = 101,

        [EnumMember]
        NoSdCard = 102,

        [EnumMember]
        FailCreateFile = 103,

        [EnumMember]
        FileNameTooLong = 104,

        [EnumMember]
        FailOpenFile = 105,

        [EnumMember]
        FailDeleteFile = 106,

        [EnumMember]
        NotSupportedInRevision = 107
        

    }
}