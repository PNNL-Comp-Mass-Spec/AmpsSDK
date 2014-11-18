// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Frame.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Frame type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UIMFLibrary;

    /// <summary>
    /// The Frame class is used to contain information about a Frame, sometimes refered to as <see cref="Spectra"/>, 
    /// which is a set of accumulations that has been summed where each set intersects.
    /// </summary>
    public class Frame
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Frame"/> class.
        /// </summary>
        /// <param name="spectra">
        /// The spectra.
        /// </param>
        public Frame(ConcurrentDictionary<uint, IDictionary<int, int>> spectra)
        {
            // TODO: Use AddorUpdate instead of TryAdd in order to provide more clarity when using ConcurrentDictionary.
            this.Accumulations = 0;
            this.Spectra = spectra;
            this.DateTime = DateTime.UtcNow;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of accumulations that have been summed into the <see cref="Spectra"/>.
        /// </summary>
        public int Accumulations { get; private set; }

        /// <summary>
        /// Gets the dateTime that the fream was first created on.
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        public FrameParameters Parameters { get; set; }

        // TODO: convince towards using private set for Spectra.

        /// <summary>
        /// Gets or sets the spectra.
        /// </summary>
        public ConcurrentDictionary<uint, IDictionary<int, int>> Spectra { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Add an accumulation into the Spectra.
        /// </summary>
        /// <param name="accumulation">
        /// The accumulation.
        /// </param>
        public void AddAccumulation(ConcurrentDictionary<uint, IDictionary<int, int>> accumulation)
        {
            this.MergeAndSumAccumulations(accumulation);
            this.Accumulations++;
        }

        /// <summary>
        /// Merges two frames and updates the accumulations to reflect to total accumulations in each.
        /// </summary>
        /// <param name="frame">
        /// The frame that will be merged into this one.
        /// </param>
        public void MergeFrames(Frame frame)
        {
            var accumulationsBeforeMerge = this.Accumulations;
            var accumulationsToBeMergedIn = frame.Accumulations;

            // If the person who created the frame that was passed to this didn't use the add function or set the 
            // number of accumulations we are going to assume that there is only one (1) accumulation in the spectra; 
            if (accumulationsToBeMergedIn == 0 && frame.Spectra.Count != 0)
            {
                accumulationsToBeMergedIn = 1;
            }

            this.MergeAndSumAccumulations(frame.Spectra);

            this.Accumulations = accumulationsBeforeMerge + accumulationsToBeMergedIn;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sums two mobility scans together.
        /// </summary>
        /// <param name="accumulation">
        /// One or more mobility scans.
        /// </param>
        private void MergeAndSumAccumulations(ConcurrentDictionary<uint, IDictionary<int, int>> accumulation)
        {
            Parallel.ForEach(
                accumulation, 
                scan => this.Spectra.AddOrUpdate(
                    scan.Key, 
                    scan.Value, 
                    (key, value) =>
                        {
                            foreach (var innerKvP in value)
                            {
                                if (!scan.Value.ContainsKey(innerKvP.Key))
                                {
                                    scan.Value.Add(innerKvP.Key, innerKvP.Value);
                                }
                                else
                                {
                                    scan.Value[innerKvP.Key] += innerKvP.Value;
                                }
                            }

                            return scan.Value;
                        }));
        }

        #endregion
    }
}