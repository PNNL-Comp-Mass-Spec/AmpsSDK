using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalkorSDK
{
    public class FalkorErrorArgs: EventArgs
    {
        public FalkorErrorArgs()
            : this("", null)
        {
            
        }

        public FalkorErrorArgs(string message)
            : this(message, null)
        {
            
        }

        public FalkorErrorArgs(string message, Exception ex)
        {

        }

        public string Message { get; private set; }
        public Exception Exception { get; private set; }
    }
}
