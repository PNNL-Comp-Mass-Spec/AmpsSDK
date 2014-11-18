// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Experiment.cs" company="">
//   
// </copyright>
// <summary>
//   Experiment class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    /// <summary>
    /// Experiment class
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Experiment
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Experiment"/> class.
        /// </summary>
        /// <param name="name">
        /// TODO The name.
        /// </param>
        [ImportingConstructor]
        public Experiment()
        {
	        this.Name = "Experiment";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Experiment"/> class.
        /// </summary>
        /// <param name="name">
        /// TODO The name.
        /// </param>
        public Experiment(string name = "")
        {
            this.Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the experiment.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create pipelines.
        /// </summary>
        /// <param name="experimentBlocks">
        /// The experiment blocks.
        /// </param>
        public virtual void CreatePipelines(IEnumerable<IExperimentBlock<object>> experimentBlocks)
        {
            
        }

        /// <summary>
        /// TODO The validate.
        /// </summary>
        /// <param name="reason">
        /// TODO The reason.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [Obsolete]
        public virtual bool Validate(ref string reason)
        {
            return true;
        }

        #endregion
    }
}