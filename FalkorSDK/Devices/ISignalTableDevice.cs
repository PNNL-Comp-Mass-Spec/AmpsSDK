// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISignalTableDevice.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The SignalTableDevice interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    using FalkorSDK.Data.Signals;

    /// <summary>
    /// TODO The SignalTableDevice interface.
    /// </summary>
    [InheritedExport]
    public interface ISignalTableDevice : IFalkorDevice
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The abort.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        Task<bool> AbortAsync();

        /// <summary>
        /// TODO The load table async.
        /// </summary>
        /// <param name="table">
        /// The table.
        /// </param>
        /// <param name="iterations">
        /// The iterations.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task LoadTableAsync(SignalTable table, int iterations = 1);

        /// <summary>
        /// TODO The write.
        /// </summary>
        /// <param name="table">
        /// TODO The Table.
        /// </param>
        /// <param name="iterations">
        /// TODO The iterations.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task WriteTableAsync(SignalTable table, int iterations);

        #endregion
    }
}