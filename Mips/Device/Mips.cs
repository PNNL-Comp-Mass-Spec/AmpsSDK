using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Mips.Commands;
using Mips.Io;
using Mips.Module;

namespace Mips.Device
{
    public class Mips : IStandardModule
    {
        private readonly IMipsCommunicator communicator;
        public Mips(IMipsCommunicator communicator)
        {
            if (communicator == null)
            {
                throw new ArgumentNullException(nameof(communicator));
            }
            this.communicator = communicator;
        }
        public async Task<string> GetVersion()
        {
            MipsCommand command = new MipsCommand("GVER", "GVER");
            var messagePacket = communicator.MessageSources;
            communicator.Write(command);
            var stream = await messagePacket.Where(x => x.RespondingFromCommand.CommandName == command.CommandName).
                Select(s => s.ResponsePayload).FirstAsync();
            return stream;
        }

        public async Task<string> GetError()
        {
            MipsCommand command = new MipsCommand("GERR", "GERR");
            var messagePacket = communicator.MessageSources;
            communicator.Write(command);
            var stream = await messagePacket.Where(x => x.RespondingFromCommand.CommandName == command.CommandName).
                Select(s => s.ResponsePayload).FirstAsync();
            return stream;
        }

        public async Task<string> GetName()
        {
            MipsCommand command = new MipsCommand("GNAME", "GNAME");
            var messagePacket = communicator.MessageSources;
            communicator.Write(command);
            var stream = await messagePacket.Where(x => x.RespondingFromCommand.CommandName == command.CommandName).
                Select(s => s.ResponsePayload).FirstAsync();
            return stream;
        }

        public async Task<Unit> SetName(string name)
        {
            MipsCommand command = new MipsCommand("SNAME", "SNAME");
            command = command.AddParameter(",", name);
            var messagePacket = communicator.MessageSources;
            communicator.Write(command);
            var stream = await messagePacket.Where(x => x.RespondingFromCommand.CommandName == command.CommandName).
                Select(s => Unit.Default).FirstAsync();
            return stream;
        }

        public async Task<string> GetConfiguration()
        {
            MipsCommand command = new MipsCommand("ABOUT", "ABOUT");
            var messagePacket = communicator.MessageSources;
            communicator.Write(command);
            var stream = await messagePacket.Where(x => x.RespondingFromCommand.CommandName == command.CommandName).
                Select(s => s.ResponsePayload).FirstAsync();
            return stream;
        }

        public Task<Unit> SetModuleRevisionLevel(int board, string moduleAddress, string level)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Rest()
        {
            throw new NotImplementedException();
        }

        public async Task<Unit> Save()
        {
            MipsCommand command = new MipsCommand("SAVE", "SAVE");
            var messagePacket = communicator.MessageSources;
            communicator.Write(command);
            var stream = await messagePacket.Where(x => x.RespondingFromCommand.CommandName == command.CommandName).
                Select(s => Unit.Default).FirstAsync();
            return stream;
        }

        public async Task<IEnumerable<string>> GetCommands()
        {
            throw new NotImplementedException();
        }
    }
}