// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunTimeTableBlock.cs" company="">
//   
// </copyright>
// <summary>
//   The run time table block.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FalkorSDK.Data.Experiment
{
    using System;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    using FalkorSDK.Data.Signals;
    using FalkorSDK.Devices;

    using ReactiveUI;

    /// <summary>
    /// The run time table block.
    /// </summary>
    public class RunTimeTableBlock : ReactiveObject, IActionBlock<SignalTable>
    {
        #region Fields

        /// <summary>
        /// The signal table device to run.
        /// </summary>
        private ISignalTableDevice signalTableDevice;

        /// <summary>
        /// The iterations.
        /// </summary>
        private int iterations;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RunTimeTableBlock"/> class.
        /// </summary>
        public RunTimeTableBlock()
        {
            this.ActionBlock = new ActionBlock<SignalTable>(signalTable => this.StartTable(signalTable));
        }

        /// <summary>
        /// The start table.
        /// </summary>
        /// <param name="signalTable">
        /// The signal table.
        /// </param>
        private void StartTable(SignalTable signalTable)
        {
            this.SignalTableDevice.WriteTableAsync(signalTable, 1);
        }

        #region Public Properties


        /// <summary>
        /// Gets or sets the signal table device to run.
        /// </summary>
        public ISignalTableDevice SignalTableDevice
        {
            get
            {
                return this.signalTableDevice;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.signalTableDevice, value);
            }
        }

        /// <summary>
        /// Gets or sets the iterations.
        /// </summary>
        public int Iterations
        {
            get
            {
                return this.iterations;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.iterations, value);
            }
        }

        #endregion

        /// <summary>
        /// Gets the action block.
        /// </summary>
        public ActionBlock<SignalTable> ActionBlock { get; private set; }
    }
}