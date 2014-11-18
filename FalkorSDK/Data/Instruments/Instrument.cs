// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Instrument.cs" company="">
//   
// </copyright>
// <summary>
//   Describes the physical layout of the instrument
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Instruments
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes the physical layout of the instrument
    /// </summary>
    public class Instrument : IInstrument
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the components.
        /// </summary>
        public IEnumerable<InstrumentComponent> Components { get; set; }

        #endregion
    }
}