using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Mips.Data;
using Mips.Io;

namespace Mips.Commands
{
    abstract class MipsMessage
    {
	    public static readonly MipsMessage[] EmptyArray = new MipsMessage[0];

	    protected MipsCommand command;

	    internal DateTime createdDateTime;
	    internal long createdTimestamp;

	    public MipsMessage(MipsCommand command)
	    {
			this.command = command;
		    createdDateTime = DateTime.UtcNow;
		    createdTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
		}
	    public MipsCommand Command => command;
	   // public virtual string CommandAndKey => Command.ToString();

	    public static MipsMessage Create(MipsCommand command)
	    {
		    return new CommandMessage(command);
		}

	    public static MipsMessage Create(MipsCommand command, MipsSignalTable signalTable)
	    {
			return new CommandSignalTableMessage(command,signalTable);
	    }
		internal abstract void WriteImpl(MipsCommunicator physical);
		private string Read(MipsCommunicator physical)
		{
			  return physical.ReadLine();
		}

		internal void WriteTo(MipsCommunicator physical)
	    {
		    try
		    {
			    WriteImpl(physical);
		    }
		    catch
		    {

		    }
	    }

    }
	internal abstract class CommandBase : MipsMessage
	{
		public CommandBase(MipsCommand command) : base(command)
		{
		}
	}

	 internal sealed class CommandSignalTableMessage : CommandBase
	{
		private readonly byte[] value;

		public CommandSignalTableMessage(MipsCommand command, MipsSignalTable signalTable) : base(command)
		{
			
		}
	}

	internal sealed class CommandMessage : CommandBase
	{
		private readonly byte[] value;
		public CommandMessage(MipsCommand command) : base(command)
		{

		}
	}
}