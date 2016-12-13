using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reactive;
using System.Reactive.Linq;
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
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("GVER", "GVER");
                var version = await this.communicator.Write(command);
                return version;
            });
        }

        public IObservable<ErrorCodes> GetError()
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("GERR", "GERR");
                var error = await this.communicator.Write(command);
                return (ErrorCodes)Enum.Parse(typeof(ErrorCodes), error);
            });
        }

        public IObservable<string> GetName()
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("GNAME", "GNAME");
                var name = await this.communicator.Write(command);
                return name;
            });
        }

        public IObservable<Unit> SetName(string name)
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("SNAME", "SNAME");
                command.AddParameter(",", name);
                await this.communicator.Write(command);
            });
        }

        public IObservable<Unit> Reset()
        {
            throw new NotImplementedException("This is a dangerous function!");
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("RESET", "RESET");
                await this.communicator.Write(command);
            });
        }

        public IObservable<Unit> Save()
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("SAVE", "SAVE");
                await this.communicator.Write(command);
            });
        }

        public IObservable<IEnumerable<string>> GetCommands()
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("GNAME", "GNAME");
                var commands = await this.communicator.Write(command);
                return commands.Split(new[] {','});
            });
        }

        public IObservable<Unit> SetSerialBaudRate(int baudRate)
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("SBAUD", "SBAUD");
                command.AddParameter(",", baudRate);
                await this.communicator.Write(command);
            });
        }
    }
}