// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDigitizer.cs" company="">
//   
// </copyright>
// <summary>
//   Interface for a digitizer hardware
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// Interface for a digitizer hardware 
    /// </summary>
    [InheritedExport]
    public interface IDigitizer : IFalkorAdc, IFalkorDevice
    {
        #region Public Properties

        /// <summary>
        /// Gets the frame buffer block.
        /// </summary>
        ISourceBlock<Frame> FrameBufferBlock { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Collects a single frame from the digitizer object
        /// </summary>
        /// <param name="frameTargetBlock">
        /// The frame Target Block.
        /// </param>
        /// <returns>
        /// A frame as a scan, and the spectrum stored inside
        /// </returns>
        Task ReadAsync(ITargetBlock<Frame> frameTargetBlock );

        /// <summary>
        /// Forces the device to reset itself.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task ResetAsync();

        /// <summary>
        /// Forces the device to update itself with respect to settings.
        /// </summary>
        /// <param name="fileName">
        /// The file Name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateAsync(string fileName = "");

        #endregion
    }
}