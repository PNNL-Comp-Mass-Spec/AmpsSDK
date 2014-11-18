// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISignalTableFormatter.cs" company="">
//   
// </copyright>
// <summary>
//   Formats the time Table into an ASCII string
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using FalkorSDK.Data;
    using FalkorSDK.Data.Signals;

    /// <summary>
    /// Formats the time Table into an ASCII string
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    /// <typeparam name="U">
    /// </typeparam>
    public interface ISignalTableFormatter<T, U>
        where T : SignalTable
    {
        #region Public Methods and Operators

        /// <summary>
        /// Converts time in the Table into a readable string.
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <param name="converter">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string FormatTable(T table, ITimeUnitConverter<U> converter);

        #endregion
    }
}