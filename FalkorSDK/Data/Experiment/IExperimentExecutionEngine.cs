// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExperimentExecutionEngine.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The ExperimentExecutionEngine interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Experiment
{
    using System.Threading.Tasks;

    /// <summary>
    /// The Experiment Execution Engine interface.
    /// </summary>
    /// <typeparam name="T">
    /// Experiment
    /// </typeparam>
    public interface IExperimentExecutionEngine<in T>
        where T : Experiment
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The abort async.
        /// </summary>
        /// <param name="experiment">
        /// TODO The experiment.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> AbortAsync(T experiment);

        /// <summary>
        /// TODO The run async.
        /// </summary>
        /// <param name="experiment">
        /// TODO The experiment.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> RunAsync(T experiment);

        #endregion
    }
}