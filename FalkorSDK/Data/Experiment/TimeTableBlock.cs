// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeTableBlock.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The time Table block.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Experiment
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    using FalkorSDK.Data.Signals;
    using FalkorSDK.Devices;

    using ReactiveUI;

    /// <summary>
    /// TODO The time Table block.
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TimeTableBlock : ReactiveObject, IActionBlock<SignalTable>
    {
        #region Fields

        /// <summary>
        /// TODO The iterations.
        /// </summary>
        private int iterations;

        /// <summary>
        /// TODO The length.
        /// </summary>
        private int length;

        /// <summary>
        /// TODO The selected signal table.
        /// </summary>
        private SignalTable selectedSignalTable;

        /// <summary>
        /// TODO The signal table device.
        /// </summary>
        private ISignalTableDevice signalTableDevice;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeTableBlock"/> class.
        /// </summary>
        [ImportingConstructor]
        public TimeTableBlock()
        {
            this.ActionBlock = new ActionBlock<SignalTable>(timeTable => this.LoadTimeTable(timeTable));
        }

        /// <summary>
        /// The load time table.
        /// </summary>
        /// <param name="signalTable">
        /// The signal table.
        /// </param>
        private void LoadTimeTable(SignalTable signalTable)
        {
            this.SignalTableDevice.LoadTableAsync(signalTable);
        }

        #endregion

        #region Public Properties

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

        /// <summary>
        /// Gets or sets the selected signal Table.
        /// </summary>
        public SignalTable SelectedSignalTable
        {
            get
            {
                return this.selectedSignalTable;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedSignalTable, value);
            }
        }

        /// <summary>
        /// Gets or sets the signal Table device.
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

        #endregion

        /// <summary>
        /// Gets the action block.
        /// </summary>
        public ActionBlock<SignalTable> ActionBlock { get; private set; }
    }
}