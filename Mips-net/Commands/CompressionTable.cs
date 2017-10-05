using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;
using Mips_net.Device;

namespace Mips_net.Commands
{
	public  class CompressionTable
	{
		private Queue<string> commandQueue;
		private StringBuilder commandBuilder;
		public CompressionTable()
		{
			this.CommandQueue = new Queue<string>();
		}
		public CompressionTable(IEnumerable<string> commandQueue) : this()
		{
			foreach (var command in commandQueue)
			{
				CommandQueue.Enqueue(command);
			}
		}

		public string RetrieveTableAsEncodedString()
		{
			if (CommandQueue != null)
			{
				commandBuilder = new StringBuilder();
				foreach (var command in CommandQueue)
				{
					commandBuilder = commandBuilder.Append(command);
				}
			}
			
			return commandBuilder.ToString();
		
			
		}

		public Queue<string> CommandQueue { get; }

	}
}