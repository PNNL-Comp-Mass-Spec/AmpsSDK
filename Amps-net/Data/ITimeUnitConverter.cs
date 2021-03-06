﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITimeUnitConverter.cs" company="">
//   
// </copyright>
// <summary>
//   Converts one unit of time to another.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Data
{
    /// <summary>
    /// Converts one unit of time to another.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface ITimeUnitConverter<T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Converts the time specified to the given units
        /// </summary>
        /// <param name="from">
        /// </param>
        /// <param name="to">
        /// </param>
        /// <param name="time">
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T ConvertTo(TimeUnits from, TimeUnits to, T time);

        #endregion
    }
}