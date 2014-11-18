// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WaitBlock.cs" company="">
//   
// </copyright>
// <summary>
//   An experiment block intended for blocking or sleeping
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Experiment
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading;
    using System.Threading.Tasks;

    using ReactiveUI;

    /// <summary>
    /// An experiment block intended for blocking or sleeping
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class WaitBlock : ExperimentBlock<double>
    {
        #region Fields

        /// <summary>
        /// TODO The wait time.
        /// </summary>
        private double waitTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitBlock"/> class.
        /// </summary>
        /// <param name="waitTime">
        /// The time to wait in milliseconds.
        /// </param>
        [ImportingConstructor]
        public WaitBlock(double waitTime = 0)
        {
            this.WaitTime = waitTime;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the time for this block to wait in milliseconds.
        /// </summary>
        public double WaitTime
        {
            get
            {
                return this.waitTime;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.waitTime, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// TODO The abort.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override Task<bool> AbortAsync()
        {
            return Task.FromResult(true);
        }


        /// <summary>
        /// TODO The load.
        /// </summary>
        /// <param name="file">
        /// TODO The file.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override Task<bool> Load(string file)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO The load.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override Task<bool> Load()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO The save.
        /// </summary>
        /// <param name="file">
        /// TODO The file.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override Task<bool> SaveAsync(string file)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO The save.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The run async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override Task RunAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}