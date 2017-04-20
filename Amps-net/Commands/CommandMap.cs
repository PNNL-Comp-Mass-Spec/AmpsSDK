using System;
using System.Collections.Generic;
using System.Text;

namespace AmpsBoxSdk.Commands
{
    internal class CommandMap
    {
        private readonly byte[][] map;

        internal CommandMap(byte[][] map)
        {
            this.map = map;
        }

        public static CommandMap Default { get; } = CreateImpl(null, null);

        internal void AssertAvailable(AmpsCommand command)
        {
            if (map[(int)command] == null) throw new NotImplementedException(command.ToString());
        }

        internal byte[] GetBytes(AmpsCommand command)
        {
            return map[(int)command];
        }

        internal bool IsAvailable(AmpsCommand command)
        {
            return map[(int)command] != null;
        }

        /// <summary>
        /// See Object.ToString()
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            AppendDeltas(sb);
            return sb.ToString();
        }

        internal void AppendDeltas(StringBuilder sb)
        {
            for (int i = 0; i < map.Length; i++)
            {
                var key = ((AmpsCommand)i).ToString();
                var value = map[i] == null ? "" : Encoding.UTF8.GetString(map[i]);
                if (key != value)
                {
                    if (sb.Length != 0) sb.Append(',');
                    sb.Append('$').Append(key).Append('=').Append(value);
                }
            }
        }

        private static CommandMap CreateImpl(Dictionary<string, string> caseInsensitiveOverrides, HashSet<AmpsCommand> exclusions)
        {
            var commands = (AmpsCommand[])Enum.GetValues(typeof(AmpsCommand));

            byte[][] map = new byte[commands.Length][];
            bool haveDelta = false;
            for (int i = 0; i < commands.Length; i++)
            {
                int idx = (int)commands[i];
                string name = commands[i].ToString(), value = name;

                if (exclusions != null && exclusions.Contains(commands[i]))
                {
                    map[idx] = null;
                }
                else
                {
                    if (caseInsensitiveOverrides != null)
                    {
                        string tmp;
                        if (caseInsensitiveOverrides.TryGetValue(name, out tmp))
                        {
                            value = tmp;
                        }
                    }
                    if (value != name) haveDelta = true;
                    // TODO: bug?
                    haveDelta = true;
                    byte[] val = string.IsNullOrWhiteSpace(value) ? null : Encoding.UTF8.GetBytes(value);
                    map[idx] = val;
                }
            }
            if (!haveDelta && Default != null) return Default;

            return new CommandMap(map);
        }
    }
}