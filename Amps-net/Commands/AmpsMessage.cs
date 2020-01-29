using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmpsBoxSdk.Data;
using AmpsBoxSdk.Io;

namespace AmpsBoxSdk.Commands
{
    public abstract class AmpsMessage
    {
        public static readonly AmpsMessage[] EmptyArray = new AmpsMessage[0];

        protected AmpsCommand command;

        internal DateTime createdDateTime;
        internal long createdTimestamp;

        protected AmpsMessage(AmpsCommand command)
        {
            this.command = command;
            createdDateTime = DateTime.UtcNow;
            createdTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
        }

        public AmpsCommand Command => command;

        public virtual string CommandAndKey => Command.ToString();

        public static AmpsMessage Create(AmpsCommand command, AmpsSignalTable table)
        {
            return new CommandSignalTableMessage(command, table);
        }

        public static AmpsMessage Create(AmpsCommand command)
        {
            return new CommandMessage(command);
        }

        public static AmpsMessage Create(AmpsCommand command, double value)
        {
            return new CommandValueMessage(command, value);
        }

        public static AmpsMessage Create(AmpsCommand command, int value)
        {
            return new CommandValueMessage(command, value);
        }

        public static AmpsMessage Create(AmpsCommand command, string value)
        {
            return new CommandValueMessage(command, value);
        }
	   
		public static AmpsMessage Create(AmpsCommand command, string value1, string value2)
        {
            return new CommandValueValueMessage(command, value1, value2);
        }

        public static AmpsMessage Create(AmpsCommand command, int value, int value2)
        {
            return new CommandValueValueMessage(command, value, value2);
        }
        public static AmpsMessage CreateTable(AmpsCommand command, string value)
        {
            return new CommandTableValueMessage(command, value);
        }
        public static AmpsMessage Create(AmpsCommand command, IEnumerable<string> values)
        {
            return new CommandEnumerableMessage(command, values);
        }
        //internal void SetSource(ResultProcessor resultProcessor, ResultBox resultBox)
        //{ // note order here reversed to prevent overload resolution errors
        //    this.resultBox = resultBox;
        //    this.resultProcessor = resultProcessor;
        //}

        //internal void SetSource<T>(ResultBox<T> resultBox, ResultProcessor<T> resultProcessor)
        //{
        //    this.resultBox = resultBox;
        //    this.resultProcessor = resultProcessor;
        //}

        internal abstract void WriteImpl(IAmpsCommunicator physical);

        private string Read(AmpsBoxCommunicator physical)
        {
           return physical.ReadLine();
        }

        public void WriteTo(IAmpsCommunicator physical)
        {
            WriteImpl(physical);
        }
    }

    internal abstract class CommandBase : AmpsMessage
    {
        public CommandBase(AmpsCommand command) : base(command)
        {
        }
    }

    sealed class CommandMessage : CommandBase
    {
        public CommandMessage(AmpsCommand command) : base(command)
        {
        }

        internal override void WriteImpl(IAmpsCommunicator physical)
        {
            physical.WriteHeader(Command);
            physical.WriteEnd();
        }
    }

    sealed class CommandValueValueMessage : CommandBase
    {
        private readonly byte[] value1;
        private readonly byte[] value2;
        public CommandValueValueMessage(AmpsCommand command, int value1, int value2) : base(command)
        {
            this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
            this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
        }

        public CommandValueValueMessage(AmpsCommand command, int value1, double value2) : base(command)
        {
            this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
            this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
        }

        public CommandValueValueMessage(AmpsCommand command, string value1, string value2) : base(command)
        {
            this.value1 = Encoding.ASCII.GetBytes(value1.ToString());
            this.value2 = Encoding.ASCII.GetBytes(value2.ToString());
        }


        internal override void WriteImpl(IAmpsCommunicator physical)
        {         
            physical.WriteHeader(Command);
            physical.Write(value1, ",");
            physical.Write(value2, ",");
            physical.WriteEnd();
        }
    }

    sealed class CommandValueMessage : CommandBase
    {
        private readonly byte[] value;

        public CommandValueMessage(AmpsCommand command, int value) : base(command)
        {
            this.value = Encoding.ASCII.GetBytes(value.ToString());
        }

        public CommandValueMessage(AmpsCommand command, double value) : base(command)
        {
            this.value = Encoding.ASCII.GetBytes(value.ToString());
        }

        public CommandValueMessage(AmpsCommand command, string value) : base(command)
        {
            this.value = Encoding.ASCII.GetBytes(value);
        }

        internal override void WriteImpl(IAmpsCommunicator physical)
        {
            physical.WriteHeader(Command);
            physical.Write(value, ",");
            physical.WriteEnd();
        }
    }

    sealed class CommandSignalTableMessage : CommandBase
    {
        private readonly byte[] value;

        public CommandSignalTableMessage(AmpsCommand command, AmpsSignalTable signalTable) : base(command)
        {
            this.value = Encoding.ASCII.GetBytes(signalTable.RetrieveTableAsEncodedString());
        }

        internal override void WriteImpl(IAmpsCommunicator physical)
        {
            //Table format: STBLDAT;<data>;
            physical.WriteHeader(Command);
            physical.Write(value, ";");
            physical.WriteEnd(";");

        }
    }
    internal class CommandTableValueMessage : AmpsMessage
    {
        private byte[] value;

        public CommandTableValueMessage(AmpsCommand command, string table) : base(command)
        {
            this.value = Encoding.ASCII.GetBytes(table);
        }

        internal override void WriteImpl(IAmpsCommunicator physical)
        {
            physical.WriteHeader(Command);
            physical.Write(value, ";");
            physical.WriteEnd(";");
        }
    }
    internal class CommandEnumerableMessage : CommandBase
    {
        private byte[][] bytevalue;

        public CommandEnumerableMessage(AmpsCommand command, IEnumerable<string> values) : base(command)
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
        
        internal override void WriteImpl(IAmpsCommunicator physical)
        {
            physical.WriteHeader(Command);
            foreach (var value in bytevalue)
            {
                physical.Write(value, ",");
            }
            physical.WriteEnd();
        }
    }

}