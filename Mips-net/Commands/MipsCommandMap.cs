using System;
using System.Collections.Generic;
using System.Text;

namespace Mips_net.Commands
{
	public  class MipsCommandMap
    {
	    private readonly byte[][] map;
	   

		internal MipsCommandMap(byte[][] map)
	    {
		    this.map = map;
	    }
	    public static MipsCommandMap Default { get; } = CreateImpl(null, null);

	    internal byte[] GetBytes(MipsCommand command)
	    {
		    return map[(int)command];
	    }

		private static MipsCommandMap CreateImpl(Dictionary<string, string> caseInsensitiveOverrides, HashSet<MipsCommand> exclusions)
		{
		    
		    var commands = (MipsCommand[])Enum.GetValues(typeof(MipsCommand));

		    byte[][] map = new byte[commands.Length][];
		    for (int i = 0; i < commands.Length; i++)
		    {
			    int idx = (int)commands[i];
			    string name = commands[i].ToString(), value = name;

			    //if (exclusions != null && exclusions.Contains(commands[i]))
			    //{
				   // map[idx] = null;
			    //}
			    //else
			    //{
				    if (caseInsensitiveOverrides != null)
				    {
					    string tmp;
					    if (caseInsensitiveOverrides.TryGetValue(name, out tmp))
					    {
						    value = tmp;
					    }
				    }
				   
				    byte[] val = string.IsNullOrWhiteSpace(value) ? null : Encoding.UTF8.GetBytes(value);
				    map[idx] = val;
			   // }
		    }
			if (Default != null) return Default;

			return new MipsCommandMap(map);
		}
	}
}
