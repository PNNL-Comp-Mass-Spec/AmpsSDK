// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransformManyBlock.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ITransformManyBlock type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.Data.Experiment
{
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// TODO The TransformManyBlock interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    /// <typeparam name="U">
    /// </typeparam>
    public interface ITransformManyBlock<T, U>
    {
        /// <summary>
        /// Gets the transform many block.
        /// </summary>
        TransformManyBlock<T, U> TransformManyBlock { get; } 
    }
}