// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IActionBlock.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IActionBlock type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.Data.Experiment
{
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// TODO The ActionBlock interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IActionBlock<T>
    {
        /// <summary>
        /// Gets the action block.
        /// </summary>
        ActionBlock<T> ActionBlock { get; }
    }
}