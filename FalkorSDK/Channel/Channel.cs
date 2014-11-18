namespace FalkorSDK.Channel
{
	using System;

	public class Channel
    {
		public string Description { get; set; }

		public string Name { get; set; }

		public Guid Id { get; } = Guid.NewGuid();

		public int Address { get; set; }

		public Channel(int address, string name, string description)
		{
			this.Address = address;
			this.Name = name;
			this.Description = description;
		}

	  public event EventHandler CloseRequested;

		public void Remove()
		{
		    var closeRequested = this.CloseRequested;
		    if (closeRequested != null)
		    {
		        closeRequested(this, EventArgs.Empty);
		    }
		}

	}

}