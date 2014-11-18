// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalTableExceptions.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The Table length not specified.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Signals
{
    using System;

    /// <summary>
    /// TODO The Table length not specified.
    /// </summary>
    public class TableLengthNotSpecified : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TableLengthNotSpecified"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public TableLengthNotSpecified(string message)
            : base(message)
        {
        }

        #endregion
    }

    /// <summary>
    /// TODO The Table name not specified.
    /// </summary>
    public class TableNameNotSpecified : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TableNameNotSpecified"/> class.
        /// </summary>
        /// <param name="message">
        /// TODO The message.
        /// </param>
        public TableNameNotSpecified(string message)
            : base(message)
        {
        }

        #endregion
    }

    /// <summary>
    /// TODO The Table time units not specified.
    /// </summary>
    public class TableTimeUnitsNotSpecified : Exception
    {
    }
}