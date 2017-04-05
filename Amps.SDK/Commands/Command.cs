// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Command.cs" company="">
//   
// </copyright>
// <summary>
//   AMPS Box command structure for supporting multiple versions of the software.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace AmpsBoxSdk.Commands
{
    /// <summary>
    /// AMPS Box command structure for supporting multiple versions of the software.
    /// </summary>
    public class Command 
    {
        #region Constructors and Destructors

       /// <summary>
       /// Instantiates a new Command object with the provided name and value.
       /// </summary>
       /// <param name="name"></param>
       /// <param name="value"></param>
        public Command(string name, string value)
       {
           this.CommandName = name;
           this.Value = value;
            this.ExpectedResponse = 0x06;
        }

        #endregion

        #region Public Properties

        public string CommandName { get; }

        public string Value { get; }

        /// <summary>
        /// Gets or sets the expected response
        /// </summary>
        public int ExpectedResponse { get; }

        public Command AddParameter(string separator, string parameter)
        {
            return new Command(this.CommandName, this.Value + separator + parameter);
        }

        public override string ToString()
        {
            return this.Value;
        }

        public Command AddParameter(string separator, int value)
        {
            return AddParameter(separator, value.ToString());
        }

        public Command AddParameter(string separator, double value)
        {
            return AddParameter(separator, value.ToString(CultureInfo.CurrentCulture));
        }

        public Command AddParameter(string separator, bool state)
        {
            return AddParameter(separator, state.ToString());
        }

        #endregion
    }
}