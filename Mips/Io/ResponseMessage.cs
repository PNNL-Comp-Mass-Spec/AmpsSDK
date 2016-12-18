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

        private ResponseMessage(MipsCommand command, string payload)
        {
            this.RespondingFromCommand = command;
            this.ResponsePayload = payload;
        }

        public ResponseMessage WithPayload(string payload)
        {
            return new ResponseMessage(this.RespondingFromCommand, payload);
        }

        public string ResponsePayload { get; }
        public MipsCommand RespondingFromCommand { get; }
    }
}