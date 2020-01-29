using System.Collections.Generic;
using System.Text;

namespace AmpsBoxSdk.Io
{
    internal class FillingCollection
    {
        public byte[] LineEnding { get; }
        public List<byte> Message { get; }
        public bool Complete { get; set; }

        public bool IsError { get; set; }

        public FillingCollection()
        {
            LineEnding = Encoding.ASCII.GetBytes("\r\n");
            this.Message = new List<byte>();
        }
    }
}
