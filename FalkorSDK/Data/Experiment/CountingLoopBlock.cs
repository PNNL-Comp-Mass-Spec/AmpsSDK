// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountingLoopBlock.cs" company="">
//   
// </copyright>
// <summary>
//   The basic counting loop block.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Experiment
{
    using System;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// A block for basic loops.  The user only needs to set the <see cref="FalsePath"/>, which is the block to loop over; 
    /// the <see cref="TruePath"/>, which is the block to go to when the loop exits;
    ///  and the <see cref="RightSide"/> which is how many times to loop.
    /// </summary>
    public class CountingLoopBlock : ExperimentBlock<int>, IDecisionBlock<int>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CountingLoopBlock"/> class.
        /// </summary>
        public CountingLoopBlock()
        {
           this.Path = new BroadcastBlock<int>(x => x);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the comparison.
        /// </summary>
        public ComparisonType Comparison
        {
            get
            {
                return ComparisonType.EqualTo;
            }

            set
            {
                return;
            }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public BroadcastBlock<int> Path { get; private set; }

        /// <summary>
        /// Gets or sets the starting number or the current iteration of the loop.  It is advised that you do not set this number.
        /// </summary>
        public int LeftSide { get; set; }

        /// <summary>
        /// Gets or sets the how many times to loop.
        /// </summary>
        public int RightSide { get; set; }


        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// TODO The abort async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override async Task<bool> AbortAsync()
        {
            return await Task.Run(
                () =>
                    {
                        this.LeftSide = 1;
                        this.RightSide = 0;
                        return true;
                    });
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