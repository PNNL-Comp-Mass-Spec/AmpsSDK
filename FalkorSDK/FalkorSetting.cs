// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FalkorSetting.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The falkor setting.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK
{
    /// <summary>
    /// TODO The falkor setting.
    /// </summary>
    public class FalkorSetting
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FalkorSetting"/> class.
        /// </summary>
        /// <param name="name">
        /// TODO The name.
        /// </param>
        /// <param name="value">
        /// TODO The value.
        /// </param>
        public FalkorSetting(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public object Value { get; private set; }

        #endregion
    }
}