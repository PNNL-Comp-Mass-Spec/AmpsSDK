// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalibrationData.cs" company="">
//   
// </copyright>
// <summary>
//   Holds calibration data for a given signal.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Calibration
{
    using System.Collections.Generic;

    /// <summary>
    /// Holds calibration data for a given signal.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class CalibrationData<T>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalibrationData{T}"/> class. 
        /// Creates new calibration data
        /// </summary>
        public CalibrationData()
        {
            this.Coefficients = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalibrationData{T}"/> class. 
        /// Calibration data
        /// </summary>
        /// <param name="data">
        /// </param>
        public CalibrationData(T[] data)
        {
            foreach (T value in data)
            {
                this.Coefficients.Add(value);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the coefficients.
        /// </summary>
        public ICollection<T> Coefficients { get; private set; }

        #endregion
    }
}