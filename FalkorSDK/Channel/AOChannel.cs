namespace FalkorSDK.Channel
{
    public class AOChannel : Channel
    {
		public AOChannel(int address, string name, string description, bool isRf)
			: base (address, name, description)
		{
			 this.IsRf = isRf;
		}

        public bool IsRf { get; private set; }
    }
}