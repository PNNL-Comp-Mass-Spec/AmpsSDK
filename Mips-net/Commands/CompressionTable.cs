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
			
		}
		public CompressionTable(Queue<string> commandQueue)
		{
			this.CommandQueue = commandQueue;
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

		//throw new NotImplementedException();
		
		public  Queue<string> CommandQueue { get; set; }

		public static CompressionTable GetCompressionTable()
		{
			return new CompressionTable() ;
		}
	}
}