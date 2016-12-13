// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsCommand.cs" company="">
//   
// </copyright>
// <summary>
//   AMPS Box command structure for supporting multiple versions of the software.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
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
        public AmpsCommand(string name, string value) : base (name, value)
        {
            this.ExpectedResponse = 0x06;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the expected response
        /// </summary>
        [DataMember]
        public int ExpectedResponse { get; }

        public override Command AddParameter(string separator, string parameter)
        {
            return new AmpsCommand(this.CommandName, this.Value + separator + parameter);
        }

        public override string ToString()
        {
            return this.Value;
        }

        public override Command AddParameter(string separator, int value)
        {
            return AddParameter(separator, value.ToString());
        }

        public override Command AddParameter(string separator, double value)
        {
            return AddParameter(separator, value.ToString(CultureInfo.CurrentCulture));
        }

        #endregion
    }
}