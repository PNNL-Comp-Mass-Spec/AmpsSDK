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
    /// <summary>
    /// TODO The error codes.
    /// </summary>
    public enum ErrorCodes
    {
        /// <summary>
        /// TODO The bad command.
        /// </summary>
        BadCommand = 1, 

        /// <summary>
        /// TODO The bad arg.
        /// </summary>
        BadArg = 2, 

        /// <summary>
        /// The local already.
        /// </summary>
        LocalAlready = 3, 

        /// <summary>
        /// The table already.
        /// </summary>
        TableAlready = 4, 

        /// <summary>
        /// The no table loaded.
        /// </summary>
        NoTableLoaded = 5, 

        /// <summary>
        /// The no table mode.
        /// </summary>
        NoTableMode = 6, 

        /// <summary>
        /// The table not ready.
        /// </summary>
        TableNotReady = 7, 

        /// <summary>
        /// The token timeout.
        /// </summary>
        TokenTimeout = 8, 

        /// <summary>
        /// The expected colon.
        /// </summary>
        ExpectedColon = 9, 

        /// <summary>
        /// The table too big.
        /// </summary>
        TableTooBig = 10, 

        /// <summary>
        /// The channel low or board.
        /// </summary>
        ChannelLowOrBoard = 11, 

        /// <summary>
        /// The channel high or board.
        /// </summary>
        ChannelHighOrBoard = 12, 

        /// <summary>
        /// The channel high.
        /// </summary>
        ChannelHigh = 13, 

        /// <summary>
        /// The board low or board.
        /// </summary>
        BoardLowOrBoard = 14, 

        /// <summary>
        /// The board high or board.
        /// </summary>
        BoardHighOrBoard = 15, 

        /// <summary>
        /// The board high.
        /// </summary>
        BoardHigh = 16, 

        /// <summary>
        /// The board not support.
        /// </summary>
        BoardNotSupport = 17, 

        /// <summary>
        /// The bad baud.
        /// </summary>
        BadBaud = 18, 

        /// <summary>
        /// The expected comma.
        /// </summary>
        ExpectedComma = 19, 

        /// <summary>
        /// The inception.
        /// </summary>
        Inception = 20, 

        /// <summary>
        /// The missing open bracket.
        /// </summary>
        MissingOpenBracket = 21, 

        /// <summary>
        /// The invalid channel.
        /// </summary>
        InvalidChannel = 22, 

        /// <summary>
        /// The dio hardware not present.
        /// </summary>
        DioHardwareNotPresent = 23, 

        /// <summary>
        /// The temperature range.
        /// </summary>
        TemperatureRange = 24, 

        /// <summary>
        /// The esi hv out of range.
        /// </summary>
        EsiHvOutOfRange = 25, 

        /// <summary>
        /// The temperature control loop gain.
        /// </summary>
        TemperatureControlLoopGain = 26

    }
}