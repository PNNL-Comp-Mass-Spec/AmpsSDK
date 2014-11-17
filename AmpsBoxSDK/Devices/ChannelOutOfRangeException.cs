namespace AmpsBoxSdk.Devices
{
	using System;

	/// <summary>
	///  Thrown when a channel specified is not available on the equipment.
	/// </summary>
	public class ChannelOutOfRangeException : Exception
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ChannelOutOfRangeException"/> class.
		/// </summary>
		/// <param name="message">
		/// TODO The message.
		/// </param>
		public ChannelOutOfRangeException(string message)
			: base(message)
		{
		}

		#endregion
	}
}