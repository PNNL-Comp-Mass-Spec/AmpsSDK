// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExperimentBlock.cs" company="">
//   
// </copyright>
// <summary>
//   Block of an experiment that represents a discrete action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Experiment
{
    using System;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    using ReactiveUI;

    /// <summary>
    /// Block of an experiment that represents a discrete action.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class ExperimentBlock<T> : ReactiveObject, IExperimentBlock<T>
    {
        #region Fields

        /// <summary>
        /// TODO The name.
        /// </summary>
        private string name;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the block
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.name, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets or sets the source block.
        /// </summary>
        public ISourceBlock<T> SourceBlock { get; protected set; }

        /// <summary>
        /// Gets or sets the target block.
        /// </summary>
        public ITargetBlock<T> TargetBlock { get; protected set; }

        /// <summary>
        /// Aborts the experimental block.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract Task<bool> AbortAsync();

        /// <summary>
        /// Returns true if the block passed in and this block are the same.
        /// </summary>
        /// <param name="obj">
        /// The object to compare against.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((ExperimentBlock<T>)obj);
        }

        /// <summary>
        /// TODO The get hash code.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Name != null ? this.Name.GetHashCode() : 0) * 397;
            }
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
        public abstract Task<bool> Load(string file);

        /// <summary>
        /// TODO The load.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public abstract Task<bool> Load();

        /// <summary>
        /// TODO The save.
        /// </summary>
        /// <param name="file">
        /// TODO The file.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public abstract Task<bool> SaveAsync(string file);

        /// <summary>
        /// TODO The save.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public abstract Task<bool> SaveAsync();

        #endregion

        #region Methods

        /// <summary>
        /// TODO The equals.
        /// </summary>
        /// <param name="other">
        /// TODO The other.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool Equals(ExperimentBlock<T> other)
        {
            return string.Equals(this.Name, other.Name);
        }

        /// <summary>
        /// TODO The run async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public abstract Task RunAsync();

        #endregion
    }
}