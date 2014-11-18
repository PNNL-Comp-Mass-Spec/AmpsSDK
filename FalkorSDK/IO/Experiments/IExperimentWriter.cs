// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExperimentWriter.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The ExperimentWriter interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Experiments
{
    using FalkorSDK.Data.Experiment;

    /// <summary>
    /// TODO The ExperimentWriter interface.
    /// </summary>
    public interface IExperimentWriter
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The write.
        /// </summary>
        /// <param name="path">
        /// TODO The path.
        /// </param>
        /// <param name="experiment">
        /// TODO The experiment.
        /// </param>
        void Write(string path, Experiment experiment);

        #endregion
    }
}