using System.Collections.Generic;
using System.Globalization;

namespace Mips.Commands
{
    public class MipsCommand
    {
        private List<byte> response;
        public MipsCommand(string name, string value)
        {
            this.CommandName = name;
            this.Value = value;
            this.ExpectedResponse = 0x06;
            this.response = new List<byte>();
        }

        private MipsCommand(string name, string value, IEnumerable<byte> response) : this(name, value)
        {
            this.response.AddRange(response);
        }

        /// <summary>
        /// Gets or sets the expected response
        /// </summary>
        public int ExpectedResponse { get; }

        public string CommandName { get; }

        public string Value { get; }

        public MipsCommand AddParameter(string separator, string parameter)
        {
            return new MipsCommand(this.CommandName, this.Value + separator + parameter);
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