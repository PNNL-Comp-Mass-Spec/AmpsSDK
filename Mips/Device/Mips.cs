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
            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            var stream = await messagePacket.Where(x => x.RespondingFromCommand.CommandName == command.CommandName).
                Select(s => s.ResponsePayload).FirstAsync();
            return stream;
        }

        public Task<string> GetError()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetName()
        {
            throw new System.NotImplementedException();
        }

        public Task<Unit> SetName(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetConfiguration()
        {
            throw new System.NotImplementedException();
        }

        public Task<Unit> SetModuleRevisionLevel(int board, string moduleAddress, string level)
        {
            throw new System.NotImplementedException();
        }

        public Task<Unit> Rest()
        {
            throw new System.NotImplementedException();
        }

        public Task<Unit> Save()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<string>> GetCommands()
        {
            throw new System.NotImplementedException();
        }
    }
}