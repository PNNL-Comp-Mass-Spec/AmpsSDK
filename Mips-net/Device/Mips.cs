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
			MipsMessage command = new MipsMessage("GVER", "GVER");
			var messagePacket = communicator.MessageSources;
			communicator.Write(command);
			var stream = await messagePacket.Where(x => x.RespondingFromCommand.CommandName == command.CommandName).
				Select(s => s.ResponsePayload).FirstAsync();
			return stream;
		}

		public async Task<string> GetError()
		{
			MipsMessage command = new MipsMessage("GERR", "GERR");
			var messagePacket = communicator.MessageSources;
			communicator.Write(command);
			var stream = await messagePacket.Where(x => x.RespondingFromCommand.CommandName == command.CommandName).
				Select(s => s.ResponsePayload).FirstAsync();
			return stream;
		}

		public async Task<string> GetName()
		{
			MipsMessage command = new MipsMessage("GNAME", "GNAME");
			var messagePacket = communicator.MessageSources;
			communicator.Write(command);
			var stream = await messagePacket.Where(x => x.RespondingFromCommand.CommandName == command.CommandName).
				Select(s => s.ResponsePayload).FirstAsync();
			return stream;
		}

		public async Task<Unit> SetName(string name)
		{
			MipsMessage command = new MipsMessage("SNAME", "SNAME");
			command = command.AddParameter(",", name);
			var messagePacket = communicator.MessageSources;
			communicator.Write(command);
			var stream = await messagePacket.Where(x => x.RespondingFromCommand.CommandName == command.CommandName).
				Select(s => Unit.Default).FirstAsync();
			return stream;
		}

		public async Task<string> GetConfiguration()
		{
			MipsMessage command = new MipsMessage("ABOUT", "ABOUT");
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
			MipsMessage command = new MipsMessage("SAVE", "SAVE");
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