using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    public class StandardModule : IStandardModule
    {
        private IAmpsBoxCommunicator communicator;
        [ImportingConstructor]
        public StandardModule(IAmpsBoxCommunicator communicator)
        {
            this.communicator = communicator;
        }

        public IObservable<string> GetVersion()
        {
            Command command = new AmpsCommand("GVER", "GVER");
            var messagePacket = this.communicator.MessageSources;

            var connection = messagePacket.Connect();
            this.communicator.Write(command);
            var stream = messagePacket.Select(bytes => Encoding.ASCII.GetString(bytes.ToArray()));
            return stream;
        }

        public IObservable<ErrorCodes> GetError()
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("GERR", "GERR");

                var messagePacket = this.communicator.MessageSources;
                var connection = messagePacket.Connect();
                string error = "";
                messagePacket.Subscribe(s =>
                {
                    error = s.ToString();
                    connection.Dispose();
                });
                this.communicator.Write(command);
                return (ErrorCodes)Enum.Parse(typeof(ErrorCodes), error);
            });
        }

        public IObservable<string> GetName()
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("GNAME", "GNAME");
               this.communicator.Write(command);
                string name = string.Empty;
                return name;
            });
        }

        public IObservable<Unit> SetName(string name)
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("SNAME", "SNAME");
                command.AddParameter(",", name);
               this.communicator.Write(command);
            });
        }

        public IObservable<Unit> Reset()
        {
            throw new NotImplementedException("This is a dangerous function!");
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("RESET", "RESET");
               this.communicator.Write(command);
            });
        }

        public IObservable<Unit> Save()
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("SAVE", "SAVE");
                this.communicator.Write(command);
            });
        }

        public IObservable<string> GetCommands()
        {

            Command command = new AmpsCommand("GCMDS", "GCMDS");
           
            var messagePacket = this.communicator.MessageSources;
            var connection = messagePacket.Connect();
            this.communicator.Write(command);
            return messagePacket.Select(x => Encoding.ASCII.GetString(x.ToArray()));
        }

        public IObservable<Unit> SetSerialBaudRate(int baudRate)
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("SBAUD", "SBAUD");
                command.AddParameter(",", baudRate);
               this.communicator.Write(command);
            });
        }
    }
}