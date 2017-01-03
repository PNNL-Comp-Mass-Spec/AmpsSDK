using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Mips.Commands
{
    public class MipsCommand
    {
        public MipsCommand(string name, string value)
        {
            this.CommandName = name;
            this.Value = value;
            this.Ack = 0x06;
        }

        /// <summary>
        /// Gets or sets the expected response
        /// </summary>
        public int Ack { get; }

        public string CommandName { get; }

        public string Value { get; }

        public MipsCommand AddParameter(string separator, string parameter)
        {
            var stringBuilder = new StringBuilder();
            var command = stringBuilder.Append(this.Value).Append(separator).Append(parameter);
            return new MipsCommand(this.CommandName, command.ToString());
        }

        public override string ToString()
        {
            return this.Value;
        }

        public MipsCommand AddParameter(string separator, int value)
        {
            return AddParameter(separator, value.ToString());
        }

        public MipsCommand AddParameter(string separator, double value)
        {
            return AddParameter(separator, value.ToString(CultureInfo.CurrentCulture));
        }

        public  MipsCommand AddParameter(string separator, bool state)
        {
            return AddParameter(separator, state.ToString());
        }
    }
}