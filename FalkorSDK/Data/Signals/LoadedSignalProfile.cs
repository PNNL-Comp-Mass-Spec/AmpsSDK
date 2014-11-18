// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadedSignalProfile.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The loaded signal profile.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Signals
{
    using System.Collections.Generic;

    using FalkorSDK.Channel;

	/// <summary>
    /// TODO The loaded signal profile.
    /// </summary>
    public class LoadedSignalProfile
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadedSignalProfile"/> class.
        /// </summary>
        public LoadedSignalProfile()
        {
            this.MissingSignalNames = new List<AOChannel>();
            this.DuplicateSignals = new List<AOChannel>();
            this.Profile = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the duplicate signals.
        /// </summary>
        public ICollection<AOChannel> DuplicateSignals { get; set; }

        /// <summary>
        /// Gets or sets the missing signal names.
        /// </summary>
        public ICollection<AOChannel> MissingSignalNames { get; set; }

        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        public SignalOutputProfile Profile { get; set; }

        #endregion
    }
}