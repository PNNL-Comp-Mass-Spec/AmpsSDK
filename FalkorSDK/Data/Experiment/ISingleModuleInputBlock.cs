// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISingleModuleInputBlock.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The SingleModuleInputBlock interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.Data.Experiment
{
    /// <summary>
    /// TODO The SingleModuleInputBlock interface.
    /// </summary>
    /// <typeparam name="T">
    /// The output block that produces an output of Type T
    /// </typeparam>
    public interface ISingleModuleInputBlock<T>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the input block.
        /// </summary>
        IOutputBlock<T> InputBlock { get; set; }

        #endregion
    }
}