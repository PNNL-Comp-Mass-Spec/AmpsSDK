// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransformDataBlock.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ITransformDataBlock type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.Data.Experiment
{
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// TODO The TransformDataBlock interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    /// <typeparam name="U">
    /// </typeparam>
    public interface ITransformDataBlock<T, U>
    {
        /// <summary>
        /// Gets the transform block.
        /// </summary>
        TransformBlock<T, U> TransformBlock { get; }
    }
}