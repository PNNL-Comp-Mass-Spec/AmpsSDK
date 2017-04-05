using System.Collections.Generic;
using AmpsBoxSdk.Modules;

namespace AmpsBoxSdk.Commands
{
    public class CommandProvider : ICommandProvider
    {
        private Dictionary<string, Command> commands;

        protected CommandProvider()
        {
            this.commands = new Dictionary<string, Command>();
        }

        public Command GetCommand(string command)
        {
            if (this.commands.ContainsKey(command))
            {
                return commands[command];
            }
            else
            {
                return null;
            }
        }

        public void GenerateCommands(IStandardModule module)
        {
            var commands = module.GetCommands();
        }

        public void CacheCommand(Command command)
        {
            if (!this.commands.ContainsKey(command.CommandName))
            {
                this.commands.Add(command.CommandName, command);
            }
        }
    }
}