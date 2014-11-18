// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExperimentExecutionEngine.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The experiment execution engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Experiment
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// TODO The experiment execution engine.
    /// </summary>
    public class ExperimentExecutionEngine : IExperimentExecutionEngine<Experiment>
    {
        #region Public Events

        /// <summary>
        /// TODO The error.
        /// </summary>
        public event EventHandler<FalkorErrorEventArgs> Error;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentExecutionEngine"/> class.
        /// </summary>
        public ExperimentExecutionEngine()
        {
            
        }

        #region Public Methods and Operators


        /// <summary>
        /// TODO The abort async.
        /// </summary>
        /// <param name="experiment">
        /// TODO The experiment.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> AbortAsync(Experiment experiment)
        {
            await Task.Run(() =>
            {
          // var test = experiment.Blocks.AsParallel().Select(x => x.AbortAsync());
          // Task.WaitAll(test.ToArray());
            }); 
            return true;
        }


        /// <summary>
        /// TODO The run async.
        /// </summary>
        /// <param name="experiment">
        /// TODO The experiment.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> RunAsync(Experiment experiment)
        {
            // TODO: The experiment execution engine needs to LINK all blocks together. This means all buffer blocks, broadcast blocks, batch blocks, action blocks, etc. Then simply run it.
        // var currentBlock = experiment.StartBlock;
            {
                // while (currentBlock != null)
                try
                {
               // await currentBlock.Run();
                }
                catch (Exception ex)
                {
                    this.Error(
                        this, 
                        new FalkorErrorEventArgs("There was an error during the execution of an event.", ex));
                    return false;
                }

             // currentBlock.SourceBlock = currentBlock.SourceBlock;
            }

            return true;
        }

        #endregion
    }
}