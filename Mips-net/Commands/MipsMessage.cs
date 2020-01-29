using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mips.Data;
using Mips.Io;

namespace Mips.Commands
{
	public abstract class MipsMessage
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
        public abstract override string ToString();

		public static MipsMessage Create(MipsCommand command)
		{
			return new CommandMessage(command);
		}

		public static MipsMessage Create(MipsCommand command, MipsSignalTable signalTable)
		{
			return new CommandSignalTableMessage(command, signalTable);
		}
		public static MipsMessage Create(MipsCommand command, CompressionTable compressionTable)
		{
			return new CommandCompressionTableMessage(command,compressionTable);
		}
		public static MipsMessage Create(MipsCommand command, int value)
		{
			return new CommandValueMessage(command, value);
		}
        public static MipsMessage CreateTable(MipsCommand command, string value)
        {
            return new CommandTableValueMessage(command, value);
        }
        public static MipsMessage Create(MipsCommand command, string value)
		{
			return new CommandValueMessage(command, value);
		}
		public static MipsMessage Create(MipsCommand command, bool value)
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
		public static MipsMessage Create(MipsCommand command, string value1, BitArray value2)
		{
			return new CommandValueValueMessage(command, value1, value2);
		}

		public static MipsMessage Create(MipsCommand command, int value1, int value2, int value3)
		{
			return new CommandValueValueValueMessage(command, value1, value2, value3);
		}
		public static MipsMessage Create(MipsCommand command, int value1, string value2, int value3)
		{
			return new CommandValueValueValueMessage(command, value1, value2, value3);
		}
		public static MipsMessage Create(MipsCommand command, string value1, int value2, double value3)
		{
			return new CommandValueValueValueMessage(command, value1, value2, value3);
		}
		public static MipsMessage Create(MipsCommand command, string value1, string value2, int value3)
		{
			return new CommandValueValueValueMessage(command, value1, value2, value3);
		}
		public static MipsMessage Create(MipsCommand command, int value1, int value2, string value3)
		{
			return new CommandValueValueValueMessage(command, value1, value2, value3);
		}
		public static MipsMessage Create(MipsCommand command, string value1, string value2, int value3,int value4,int value5)
		{
			return new CommandValuesMessage(command, value1, value2, value3,value4,value5);
		}
		public static MipsMessage Create(MipsCommand command, string value1,IEnumerable<int> values)
		{
			return new CommandValueEnumerableMessage(command, value1, values);
		}
		public static MipsMessage Create(MipsCommand command, IEnumerable<int> values)
		{
			return new CommandEnumerableMessage(command, values);
		}
        public static MipsMessage Create(MipsCommand command, IEnumerable<string> values)
        {
            return new CommandEnumerableMessage(command, values);
        }
        public static MipsMessage Create(MipsCommand command, IEnumerable<double> values)
		{
			return new CommandEnumerableMessage(command, values);
		}
		public static MipsMessage Create(MipsCommand command, int value,IEnumerable<int> values)
		{
			return new CommandValueEnumerableMessage(command, value,values);
		}

		internal abstract void WriteImpl(IMipsCommunicator physical);

		private string Read(MipsCommunicator physical)
		{
			return physical.ReadLine();
		}

		public void WriteTo(IMipsCommunicator physical)
		{
			try
			{
				this.WriteImpl(physical);
			}
			catch
			{
				throw new InvalidOperationException();
			}
		}
	}

    internal class CommandTableValueMessage : MipsMessage
    {
        private byte[] value;

        public CommandTableValueMessage(MipsCommand command, string table):base(command)
        {
           this.value = Encoding.ASCII.GetBytes(table); 
        }

        public override string ToString()
        {
            return command.ToString() + " " + Encoding.UTF8.GetString(value);
        }

        internal override void WriteImpl(IMipsCommunicator physical)
        {
            physical.WriteHeader(Command);
            physical.Write(value, ";");
            physical.WriteEnd(";");
        }
    }

    internal class CommandEnumerableMessage : CommandBase
	{
		private byte[][] bytevalue;
        List<string> readable_values = new List<string>();

        public CommandEnumerableMessage(MipsCommand command,  IEnumerable<int> values) : base(command)
		{
            readable_values.AddRange(values.Select(x => x.ToString()));

			bytevalue = new byte[values.Count()][];
			int i = 0;
			foreach (var value in values)
			{
				byte[] result = Encoding.ASCII.GetBytes(value.ToString());
				bytevalue[i] = result;
				i++;
			}
		}

        public CommandEnumerableMessage(MipsCommand command, IEnumerable<string> values) : base(command)
        {
            readable_values.AddRange(values);

            bytevalue = new byte[values.Count()][];
            int i = 0;
            foreach (var value in values)
            {
                byte[] result = Encoding.ASCII.GetBytes(value);
                bytevalue[i] = result;
                i++;
            }
        }
        public CommandEnumerableMessage(MipsCommand command, IEnumerable<double> values) : base(command)
		{
            readable_values.AddRange(values.Select(x => x.ToString()));

            bytevalue = new byte[values.Count()][];
			int i = 0;
			foreach (var value in values)
			{
				byte[] result = Encoding.ASCII.GetBytes(value.ToString());
				bytevalue[i] = result;
				i++;
			}
		}

        public override string ToString()
        {
            return command.ToString() + " " + String.Join(" ", readable_values);
        }

        internal override void WriteImpl(IMipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			foreach (var value in bytevalue)
			{
				physical.Write(value, ",");
			}
			physical.WriteEnd();
		}
	}

	internal class CommandValueEnumerableMessage : CommandBase
	{
		private byte[][] bytevalue;
		private readonly byte[] value;

        IEnumerable<string> readable_values;
        string readable_value;

        public CommandValueEnumerableMessage(MipsCommand command,int value, IEnumerable<int> values) : base(command)
		{
            readable_values = values.Select(x => x.ToString());
            readable_value = value.ToString();

			bytevalue = new byte[values.Count()][];
			int i = 0;
			this.value = Encoding.ASCII.GetBytes(value.ToString());
			foreach (var val in values)
			{
				byte[] result = Encoding.ASCII.GetBytes(val.ToString());
				bytevalue[i] = result;
				i++;
			}
		}
		public CommandValueEnumerableMessage(MipsCommand command, string value, IEnumerable<int> values) : base(command)
		{
            readable_values = values.Select(x => x.ToString());
            readable_value = value;

            bytevalue = new byte[values.Count()][];
			int i = 0;
			this.value = Encoding.ASCII.GetBytes(value);
			foreach (var val in values)
			{
				byte[] result = Encoding.ASCII.GetBytes(val.ToString());
				bytevalue[i] = result;
				i++;
			}
		}

		internal override void WriteImpl(IMipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.Write(this.value, ",");
			foreach (var val in bytevalue)
			{
				physical.Write(val, ",");
			}
			physical.WriteEnd();
		}

        public override string ToString()
        {
            return command.ToString() + " " + readable_value + " " + String.Join(" ", readable_values);
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

        public override string ToString()
        {
            return "";
        }

        internal override void WriteImpl(IMipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.WriteEnd();
		}
	}

	internal sealed class CommandCompressionTableMessage : CommandBase
	{
		private readonly byte[] value;

		public CommandCompressionTableMessage(MipsCommand command, CompressionTable compressionTable) : base(command)
		{
			this.value = Encoding.ASCII.GetBytes(compressionTable.RetrieveTableAsEncodedString());
		}

        public override string ToString()
        {
            return "";
        }

        internal override void WriteImpl(IMipsCommunicator physical)
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
			this.value = Encoding.ASCII.GetBytes(signalTable.RetrieveTableAsEncodedString());
		}

        public override string ToString()
        {
            return "";
        }

        internal override void WriteImpl(IMipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.Write(value, ";");
			physical.WriteEnd(";");
		}
	}
	internal sealed class CommandValueMessage : CommandBase
	{
		private readonly byte[] value;

		public CommandValueMessage(MipsCommand command, int value) : base(command)
		{
			this.value = Encoding.ASCII.GetBytes(value.ToString());
		}
		public CommandValueMessage(MipsCommand command, bool value) : base(command)
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

        public override string ToString()
        {
            return command.ToString() + " " + Encoding.UTF8.GetString(value);
        }

        internal override void WriteImpl(IMipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.Write(value, ",");
			physical.WriteEnd();
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
		public CommandValueValueMessage(MipsCommand command, string value1, BitArray value2) : base(command)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var b in value2)
			{
				sb.Append((bool)b ? "1" : "0");
			}
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(sb.ToString());
		}

        public override string ToString()
        {
            return command.ToString()
            + " " + Encoding.UTF8.GetString(value1)
            + " " + Encoding.UTF8.GetString(value2);
        }

        internal override void WriteImpl(IMipsCommunicator physical)
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
		public CommandValueValueValueMessage(MipsCommand command, int value1, string value2, int value3) : base(command)
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
		public CommandValueValueValueMessage(MipsCommand command, string value1, int value2, double value3) : base(command)
		{
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
			this.value3 = Encoding.ASCII.GetBytes(value3.ToString());
		}
		public CommandValueValueValueMessage(MipsCommand command, string value1, string value2, int value3) : base(command)
		{
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
			this.value3 = Encoding.ASCII.GetBytes(value3.ToString());
		}

        public override string ToString()
        {
            return command.ToString()
            + " " + Encoding.UTF8.GetString(value1)
            + " " + Encoding.UTF8.GetString(value2)
            + " " + Encoding.UTF8.GetString(value3);
        }

        internal override void WriteImpl(IMipsCommunicator physical)
		{
			physical.WriteHeader(Command);
			physical.Write(value1, ",");
			physical.Write(value2, ",");
			physical.Write(value3, ",");
			physical.WriteEnd();
		}
	}
	internal class CommandValuesMessage : CommandBase
	{
		private readonly byte[] value1;
		private readonly byte[] value2;
		private readonly byte[] value3;
		private readonly byte[] value4;
		private readonly byte[] value5;

		public CommandValuesMessage(MipsCommand command, string value1, string value2, int value3, int value4, int value5) : base(command)
		{
			this.command = command;
			this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
			this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
			this.value3 = Encoding.ASCII.GetBytes(value3.ToString());
			this.value4 = Encoding.ASCII.GetBytes(value4.ToString());
			this.value5 = Encoding.ASCII.GetBytes(value5.ToString());
		}

        public override string ToString()
        {
            return command.ToString()
            + " " + Encoding.UTF8.GetString(value1)
            + " " + Encoding.UTF8.GetString(value2)
            + " " + Encoding.UTF8.GetString(value3)
            + " " + Encoding.UTF8.GetString(value4)
            + " " + Encoding.UTF8.GetString(value5);
        }

        internal override void WriteImpl(IMipsCommunicator physical)
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
}