using System;
using System.Text;
using AmpsBoxSdk.Data;
using AmpsBoxSdk.Io;

namespace AmpsBoxSdk.Commands
{
    abstract class Message
    {
        public static readonly Message[] EmptyArray = new Message[0];

        protected AmpsCommand command;

        internal DateTime createdDateTime;
        internal long createdTimestamp;

        protected Message(AmpsCommand command)
        {
            this.command = command;
            createdDateTime = DateTime.UtcNow;
            createdTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
        }

        public AmpsCommand Command => command;

        public virtual string CommandAndKey => Command.ToString();

        public static Message Create(AmpsCommand command, AmpsSignalTable table)
        {
            return new CommandSignalTableMessage(command, table);
        }

        public static Message Create(AmpsCommand command)
        {
            return new CommandMessage(command);
        }

        public static Message Create(AmpsCommand command, double value)
        {
            return new CommandValueMessage(command, value);
        }

        public static Message Create(AmpsCommand command, int value)
        {
            return new CommandValueMessage(command, value);
        }

        public static Message Create(AmpsCommand command, string value)
        {
            return new CommandValueMessage(command, value);
        }

        public static Message Create(AmpsCommand command, string value1, string value2)
        {
            return new CommandValueValueMessage(command, value1, value2);
        }

        public static Message Create(AmpsCommand command, int value, int value2)
        {
            return new CommandValueValueMessage(command, value, value2);
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

        internal abstract void WriteImpl(AmpsBoxCommunicator physical);

        private string Read(AmpsBoxCommunicator physical)
        {
           return physical.ReadLine();
        }

        internal void WriteTo(AmpsBoxCommunicator physical)
        {
            try
            {
                WriteImpl(physical);
            }
            catch (Exception)
            {
                // these have specific meaning; don't wrap
                throw;
            }
        }
    }

    internal abstract class CommandBase : Message
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

        internal override void WriteImpl(AmpsBoxCommunicator physical)
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


        internal override void WriteImpl(AmpsBoxCommunicator physical)
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

        internal override void WriteImpl(AmpsBoxCommunicator physical)
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

        internal override void WriteImpl(AmpsBoxCommunicator physical)
        {
            //Table format: STBLDAT;<data>;
            physical.WriteHeader(Command);
            physical.Write(value, ";");
            physical.WriteEnd(";");

        }
    }
}