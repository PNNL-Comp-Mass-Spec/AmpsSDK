// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Scheduler.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The scheduler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data
{
    using System;
    using System.Collections.Generic;

    using FalkorSDK.Data.Experiment;

    /// <summary>
    /// TODO The scheduler.
    /// </summary>
    public class Scheduler
    {
        #region Fields

        /// <summary>
        /// TODO The experiments.
        /// </summary>
        private readonly Queue<Experiment.Experiment> experiments;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Scheduler"/> class.
        /// </summary>
        public Scheduler()
        {
            this.experiments = new Queue<Experiment.Experiment>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the reference to the IEEE that is responsible for executing the Experiment.
        /// </summary>
        public IExperimentExecutionEngine<Experiment.Experiment> Engine { get; set; }

        /// <summary>
        /// Gets or sets the experiment meta-data writer.
        /// </summary>
        public object ExperimentMetaDataWriter { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the experiment to the list.
        /// </summary>
        /// <param name="experiment">
        /// </param>
        public void Add(Experiment.Experiment experiment)
        {
            if (this.experiments.Count > 0)
            {
                throw new Exception("The current scheduler only supports one sample now.");
            }

            this.experiments.Enqueue(experiment);
        }

        /// <summary>
        /// Dequeues the specified element from the list
        /// </summary>
        /// <param name="experiment">
        /// </param>
        public void Remove(Experiment.Experiment experiment)
        {
            if (this.experiments.Count < 1)
            {
                throw new Exception("The current scheduler has no more Items.");
            }

            if (this.experiments.Contains(experiment))
            {
                this.experiments.Dequeue();
            }

            throw new Exception("The experiment specified does not exist.");
        }

        /// <summary>
        /// Executes the current queue.
        /// </summary>
        /// <param name="experiment"></param>
        public void Run()
        {
            Experiment.Experiment experiment = this.experiments.Dequeue();

            try
            {
           // this.Engine.Run(experiment);
            }
            catch (Exception ex)
            {
                // TODO: Propagate error to the main system.
            }
            finally
            {
                // Here we create the meta-data for the given experiment.
                // ExperimentMetaDataWriter.WriteTableAsync(experiment);
            }
        }

        #endregion
    }
}