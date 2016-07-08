// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsCommand.cs" company="">
//   
// </copyright>
// <summary>
//   AMPS Box command structure for supporting multiple versions of the software.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Commands
{
    using System.Runtime.Serialization;

    /// <summary>
    /// AMPS Box command structure for supporting multiple versions of the software.
    /// </summary>
    [DataContract]
    public class AmpsCommand : Command
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
        public AmpsCommand(string value, bool isSupported)
        {
            this.Value = value;
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
        public AmpsCommand(string value)
            : this(value, true)
        {
            this.ExpectedResponse = 0x06;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the expected response
        /// </summary>
        [DataMember]
        public int ExpectedResponse { get; private set; }

        /// <summary>
        /// Gets or sets whether this command is supported to being phased out.
        /// </summary>
        [DataMember]
        public bool IsSupported { get; set; }

        /// <summary>
        /// Gets or sets the value of the command
        /// </summary>
        [DataMember]
        public string Value { get; set; }

        public string Name { get; set; }

        #endregion
    }
}