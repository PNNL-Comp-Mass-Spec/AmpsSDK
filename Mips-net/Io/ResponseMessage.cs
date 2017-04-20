using System.Collections.Generic;
using Mips.Commands;

namespace Mips.Io
{
    public class ResponseMessage
    {
        public ResponseMessage(MipsCommand command)
        {
            RespondingFromCommand = command;
        }

        private ResponseMessage(MipsCommand command, string payload)
        {
            RespondingFromCommand = command;
            ResponsePayload = payload;
        }

        public ResponseMessage WithPayload(string payload)
        {
            return new ResponseMessage(RespondingFromCommand, payload);
        }

        public string ResponsePayload { get; }
        public MipsCommand RespondingFromCommand { get; }
    }
}