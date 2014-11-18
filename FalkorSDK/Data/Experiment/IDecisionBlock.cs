// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDecisionBlock.cs" company="">
//   
// </copyright>
// <summary>
//   The DecisionBlock interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.Data.Experiment
{
    using System;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// The DecisionBlock interface, used for loops and branching decision trees.
    /// </summary>
    /// <typeparam name="T">
    /// The type that will be compared to determing the branch
    /// </typeparam>
    public interface IDecisionBlock<T>
        where T : IComparable
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the comparison to be performed.
        /// </summary>
        ComparisonType Comparison { get; set; }

        /// <summary>
        /// Gets or sets the false path.
        /// </summary>
        BroadcastBlock<T> Path { get; }

        /// <summary>
        /// Gets or sets the left side of the comparison.
        /// </summary>
        T LeftSide { get; set; }

        /// <summary>
        /// Gets or sets the right side of the comparison.
        /// </summary>
        T RightSide { get; set; }


        #endregion
    }
}