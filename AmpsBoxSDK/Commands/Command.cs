namespace AmpsBoxSdk.Commands
{
    public abstract class Command
    {
        protected Command(string name, string value)
        {
            this.Value = name;
            this.Value = value;
        }
        public string Value { get; }

        public string CommandName { get; }

        public abstract Command AddParameter(string separator, string parameter);

        public abstract Command AddParameter(string separator, int value);

        public abstract Command AddParameter(string separator, double value);

        public abstract Command AddParameter(string separator, bool state);

        public override string ToString()
        {
            return this.Value;
        }
    }
}