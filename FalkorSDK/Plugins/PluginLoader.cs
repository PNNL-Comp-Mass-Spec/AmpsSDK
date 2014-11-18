using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalkorSDK.Plugins
{
    public class PluginLoader<T>
    {
        public IEnumerable<T> Load(string path)
        {
            return new List<T>();
        }

        public IEnumerable<T> LoadDirectory(string path)
        {
            return new List<T>();
        }
    }
}
