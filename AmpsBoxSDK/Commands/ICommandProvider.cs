using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpsBoxSdk.Modules;

namespace AmpsBoxSdk.Commands
{
    public interface ICommandProvider
    {
        Command GetCommand(string command);

        void GenerateCommands(IStandardModule module);

        void CacheCommand(Command command);
    }
}
