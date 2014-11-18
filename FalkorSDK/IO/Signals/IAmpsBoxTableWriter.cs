using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmpsBoxLib.IO
{
    public interface IAmpsBoxTableWriter
    {
        void WriteTable(string path, AmpsSignalTimeTable table);
    }
}
