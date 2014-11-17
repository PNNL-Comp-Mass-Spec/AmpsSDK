// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsCommand.cs" company="">
//   
// </copyright>
// <summary>
//   AMPS Box command structure for supporting multiple versions of the software.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Commands
{
    /// <summary>
    /// AMPS Box command structure for supporting multiple versions of the software.
    /// </summary>
    public class AmpsCommand
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsCommand"/> class.
        /// </summary>
        /// <param name="type">
        /// TODO The type.
        /// </param>
        /// <param name="value">
        /// TODO The value.
        /// </param>
        /// <param name="isSupported">
        /// TODO The is supported.
        /// </param>
        public AmpsCommand(AmpsCommandType type, string value, bool isSupported)
        {
            this.Value = value;
            this.CommandType = type;
            this.IsSupported = isSupported;
            this.ExpectedResponse = 0x06;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsCommand"/> class.
        /// </summary>
        /// <param name="type">
        /// TODO The type.
        /// </param>
        /// <param name="value">
        /// TODO The value.
        /// </param>
        public AmpsCommand(AmpsCommandType type, string value)
            : this(type, value, true)
        {
            this.ExpectedResponse = 0x06;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public AmpsCommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the expected response
        /// </summary>
        public int ExpectedResponse { get; private set; }

        /// <summary>
        /// Gets or sets whether this command is supported to being phased out.
        /// </summary>
        public bool IsSupported { get; set; }

        /// <summary>
        /// Gets or sets the value of the command
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}