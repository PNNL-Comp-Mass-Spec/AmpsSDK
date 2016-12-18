using System.Collections.Generic;
using Mips.Commands;

namespace Mips.Io
{
    public class ResponseMessage
    {
        public ResponseMessage(MipsCommand command)
        {
            this.RespondingFromCommand = command;
        }

        private ResponseMessage(MipsCommand command, IEnumerable<byte> payload)
        {
            this.RespondingFromCommand = command;
            this.ResponsePayload = new List<byte>(payload).AsReadOnly();
        }

        public ResponseMessage WithPayload(IEnumerable<byte> payload)
        {
            return new ResponseMessage(this.RespondingFromCommand, payload);
        }

        public IReadOnlyList<byte> ResponsePayload { get; }
        public MipsCommand RespondingFromCommand { get; }
    }
}