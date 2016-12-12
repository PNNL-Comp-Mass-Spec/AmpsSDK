// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsCommand.cs" company="">
//   
// </copyright>
// <summary>
//   AMPS Box command structure for supporting multiple versions of the software.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Text;

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
       /// Instantiates a new AmpsCommand object with the provided name and value.
       /// </summary>
       /// <param name="name"></param>
       /// <param name="value"></param>
        public AmpsCommand(string name, string value)
        {
            this.Value = value;
            this.CommandName = name;
            this.ExpectedResponse = 0x06;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the expected response
        /// </summary>
        [DataMember]
        public int ExpectedResponse { get; }

        /// <summary>
        /// Gets or sets the value of the command
        /// </summary>
        [DataMember]
        public string Value { get; }

        public string CommandName { get; }

        public AmpsCommand AddParameter(string separator, string parameter)
        {
            return new AmpsCommand(this.CommandName, this.Value + separator + parameter);
        }

        public override string ToString()
        {
            return this.Value;
        }

        #endregion
    }
}