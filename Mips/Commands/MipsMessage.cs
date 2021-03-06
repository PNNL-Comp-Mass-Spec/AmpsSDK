﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
			return new CommandSignalTableMessage(command, signalTable);
		}

		public static MipsMessage Create(MipsCommand command, int value)
		{
			return new CommandValueMessage(command, value);
		}

		public static MipsMessage Create(MipsCommand command, double value)
		{
			return new CommandValueMessage(command, value);
		}

		public static MipsMessage Create(MipsCommand command, int value1, int value2)
		{
			return new CommandValueValueMessage(command, value1, value2);
		}

		public static MipsMessage Create(MipsCommand command, string value1, string value2)
		{
			return new CommandValueValueMessage(command, value1, value2);
		}

		public static MipsMessage Create(MipsCommand command, int value1, string value2)
		{
			return new CommandValueValueMessage(command, value1, value2);
		}

		public static MipsMessage Create(MipsCommand command, int value1, double value2)
		{
			return new CommandValueValueMessage(command, value1, value2);
		}

		public static MipsMessage Create(MipsCommand command, string value1, double value2)
		{
			return new CommandValueValueMessage(command, value1, value2);
		}

		public static MipsMessage Create(MipsCommand command, double value1, double value2)
		{
			return new CommandValueValueMessage(command, value1, value2);
		}

		public static MipsMessage Create(MipsCommand command, string value1, int value2)
		{
			return new CommandValueValueMessage(command, value1, value2);
		}

		public static MipsMessage Create(MipsCommand command, int value1, int value2, int value3)
		{
			return new CommandValueValueValueMessage(command, value1, value2, value3);
		}

		public static MipsMessage Create(MipsCommand command, int value1, int value2, string value3)
		{
			return new CommandValueValueValueMessage(command, value1, value2, value3);
		}
		public static MipsMessage Create(MipsCommand command, int value1, int value2, int value3,int value4,int value5)
		{
			return new CommandValuesMessage(command, value1, value2, value3,value4,value5);
		}
		public static MipsMessage Create(MipsCommand command, string value1,IEnumerable<int> values)
		{
			return new CommandEnumerableValueValueMessage(command, value1, values);
		}
		public static MipsMessage Create(MipsCommand command, IEnumerable<int> values)
		{
			return new CommandEnumerableValueMessage(command, values);
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
				throw new InvalidOperationException();
			}
		}

	}
	internal class CommandEnumerableValueMessage : CommandBase
	{
		private byte[][] bytevalue;
		
		public CommandEnumerableValueMessage(MipsCommand command,  IEnumerable<int> values) : base(command)
		{
			bytevalue = new byte[values.Count()][];
			int i = 0;
			foreach (var value in values)
			{
				byte[] result = Encoding.ASCII.GetBytes(value.ToString());
				bytevalue[i] = result;
				i++;
			}
		}

		internal override void WriteImpl(MipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			foreach (var value in bytevalue)
			{
				physical.Write(value, ",");
			}
			physical.WriteEnd();
		}
	}

	internal class CommandEnumerableValueValueMessage : CommandBase
	{
		private byte[][] bytevalue;
		private readonly byte[] value1;

		public CommandEnumerableValueValueMessage(MipsCommand command,string value1, IEnumerable<int> values):base(command)
		{
			bytevalue = new byte[values.Count()][];
			int i = 0;
			this.value1= Encoding.ASCII.GetBytes(value1.ToString());
			foreach (var value in values)
			{
				bytevalue[i] = Encoding.ASCII.GetBytes(value.ToString());
				i++;
			}
		}

		internal override void WriteImpl(MipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.Write(value1, ",");
			foreach (var value in bytevalue)
			{
				physical.Write(value, ",");
			}
			physical.WriteEnd();
		}
	}

	internal class CommandValuesMessage : CommandBase
	{
		private readonly byte[]  value1;
		private readonly byte[] value2;
		private readonly byte[] value3;
		private readonly byte[] value4;
		private readonly byte[] value5;

		public CommandValuesMessage(MipsCommand command, int value1, int value2, int value3, int value4, int value5):base(command)
		{
			this.command = command;
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
			this.value3 = Encoding.ASCII.GetBytes(value3.ToString());
			this.value4 = Encoding.ASCII.GetBytes(value4.ToString());
			this.value5 = Encoding.ASCII.GetBytes(value5.ToString());
		}

		internal override void WriteImpl(MipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.Write(value1, ",");
			physical.Write(value2, ",");
			physical.Write(value3, ",");
			physical.Write(value4, ",");
			physical.Write(value5, ",");
			physical.WriteEnd();
		}
	}

	internal abstract class CommandBase : MipsMessage
	{
		public CommandBase(MipsCommand command) : base(command)
		{
		}
	}



	internal  sealed class CommandMessage : CommandBase
	{
		public CommandMessage(MipsCommand command) : base(command)
		{

		}

		internal override void WriteImpl(MipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.WriteEnd();
		}
	}

	internal sealed class CommandValueMessage : CommandBase
	{
		private readonly byte[] value;

		public CommandValueMessage(MipsCommand command, int value) : base(command)
		{
			this.value = Encoding.ASCII.GetBytes(value.ToString());
		}

		public CommandValueMessage(MipsCommand command, double value) : base(command)
		{
			this.value = Encoding.ASCII.GetBytes(value.ToString());
		}

		public CommandValueMessage(MipsCommand command, string value) : base(command)
		{
			this.value = Encoding.ASCII.GetBytes(value.ToString());
		}

		internal override void WriteImpl(MipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.Write(value, ",");
			physical.WriteEnd();
		}
	}

	internal sealed class CommandSignalTableMessage : CommandBase
	{
		private readonly byte[] value;

		public CommandSignalTableMessage(MipsCommand command, MipsSignalTable signalTable) : base(command)
		{
			this.value = Encoding.ASCII.GetBytes(value.ToString());
		}

		internal override void WriteImpl(MipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.Write(value, ";");
			physical.WriteEnd(";");
		}
	}
	internal class CommandValueValueMessage : CommandBase
	{
		private readonly byte[] value1;
		private readonly byte[] value2;

		public CommandValueValueMessage(MipsCommand command, int value1, int value2) : base(command)
		{
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
		}

		public CommandValueValueMessage(MipsCommand command, string value1, string value2) : base(command)
		{
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
		}

		public CommandValueValueMessage(MipsCommand command, int value1, string value2) : base(command)
		{
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
		}

		public CommandValueValueMessage(MipsCommand command, int value1, double value2) : base(command)
		{
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
		}

		public CommandValueValueMessage(MipsCommand command, string value1, double value2) : base(command)
		{
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
		}

		public CommandValueValueMessage(MipsCommand command, double value1, double value2) : base(command)
		{
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
		}

		public CommandValueValueMessage(MipsCommand command, string value1, int value2) : base(command)
		{
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
		}

		internal override void WriteImpl(MipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.Write(value1, ",");
			physical.Write(value2, ",");
			physical.WriteEnd();
		}
	}
	internal sealed class CommandValueValueValueMessage : CommandBase
	{
		private readonly byte[] value1;
		private readonly byte[] value2;
		private readonly byte[] value3;

		public CommandValueValueValueMessage(MipsCommand command, int value1, int value2, int value3) : base(command)
		{
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
			this.value3 = Encoding.ASCII.GetBytes(value3.ToString());
		}
		public CommandValueValueValueMessage(MipsCommand command, int value1, int value2, string value3) : base(command)
		{
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
			this.value3 = Encoding.ASCII.GetBytes(value3.ToString());
		}

		internal override void WriteImpl(MipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.Write(value1, ",");
			physical.Write(value2, ",");
			physical.Write(value3, ",");
			physical.WriteEnd();
		}
	}
}