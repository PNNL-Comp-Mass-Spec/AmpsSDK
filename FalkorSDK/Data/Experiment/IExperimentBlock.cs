// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExperimentBlock.cs" company="">
//   
// </copyright>
// <summary>
//   The Experiment Block interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Experiment
{
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// The Interface that all Experiment Blocks inherit from for common functionality.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IExperimentBlock<T>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the Experiment Block.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the next block to run.
        /// </summary>
        ISourceBlock<T> SourceBlock { get; }

        /// <summary>
        /// Gets the target block.
        /// </summary>
        ITargetBlock<T> TargetBlock { get; }

            #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The abort command to gracefully shutdown the hardware involved in the Experiments in an asychronous manner.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> AbortAsync();

        /// <summary>
        /// Load a block.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> Load();

        /// <summary>
        /// Load a block from file.
        /// </summary>
        /// <param name="file">
        /// The file to load from.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> Load(string file);

        /// <summary>
        /// SaveAsync the block to file.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// SaveAsync the block to file.
        /// </summary>
        /// <param name="file">
        /// The file to save to.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> SaveAsync(string file);

        #endregion
    }
}