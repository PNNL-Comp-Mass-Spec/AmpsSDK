// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISignalTableFormatter.cs" company="">
//   
// </copyright>
// <summary>
//   Formats the time AnalogOutputWaveform into an ASCII string
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Data
{
    /// <summary>
    /// Formats the time AnalogOutputWaveform into an ASCII string
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    /// <typeparam name="U">
    /// </typeparam>
    public interface ISignalTableFormatter<T, U>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Converts time in the AnalogOutputWaveform into a readable string.
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