using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmpsBoxLib.IO
{
    /// <summary>
    /// Reads an amps box time table for voltage timings 
    /// </summary>
    public interface IAmpsBoxTableReader
    {
        AmpsSignalTimeTable ReadTable(string path);
    }
}
