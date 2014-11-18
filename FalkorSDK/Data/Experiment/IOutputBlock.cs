// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOutputBlock.cs" company="">
//   
// </copyright>
// <summary>
//   The OutputBlock interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.Data.Experiment
{
    /// <summary>
    /// The OutputBlock interface that all blocks which have output implement.
    /// </summary>
    /// <typeparam name="T">
    /// The output, which is always a single object, even if multiple values are output.
    /// </typeparam>
    public interface IOutputBlock<out T>
    {
        #region Public Properties

        /// <summary>
        /// Gets the output.
        /// </summary>
        T Output { get; }

        #endregion
    }
}