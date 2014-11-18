// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExperimentReader.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The ExperimentReader interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Experiments
{
    using FalkorSDK.Data.Experiment;

    /// <summary>
    /// TODO The ExperimentReader interface.
    /// </summary>
    public interface IExperimentReader
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The write.
        /// </summary>
        /// <param name="path">
        /// TODO The path.
        /// </param>
        /// <returns>
        /// The <see cref="Experiment"/>.
        /// </returns>
        Experiment Read(string path);

        #endregion
    }
}