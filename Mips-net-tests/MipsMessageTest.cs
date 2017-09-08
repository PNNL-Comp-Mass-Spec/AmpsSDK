using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Mips_net.Commands;

namespace MipsTest
{
	[TestClass]
	public class MipsCommadMapTest
	{
		private readonly byte[][] map;
		MipsCommandMap Default = null;
		

		[TestMethod]
		public void WriteHeaderTest()
		{
			
		}

		[TestMethod]
		public void CreateImplTest()
		{
			Dictionary<string, string> caseInsensitiveOverrides = new Dictionary<string, string>();
			HashSet<MipsCommand> exclusions = new HashSet<MipsCommand>();
			caseInsensitiveOverrides.Add("GERR", "GERR");
			caseInsensitiveOverrides.Add("About", "About");
			exclusions.Add(MipsCommand.ABOUT);

			var commands = (MipsCommand[])Enum.GetValues(typeof(MipsCommand));

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
					byte[] val = string.IsNullOrWhiteSpace(value) ? null : Encoding.UTF8.GetBytes(value);
						map[idx] = val;
					
				}
			}
			if ( Default != null)
				Default = null;
			var command = MipsCommand.ABOUT;
			byte[] vBytes = map[(int)command];

			var result = vBytes;
			
			// return Default;

			//return new CommandMap(map);
		}


		[TestMethod]
		public void AssertAvailableTest()
		{

			//if (map[(int)command] == null) throw new NotImplementedException(command.ToString());
		}


		[TestMethod]
		public void GetBytesTest()
		{
			var commands = (MipsCommand[])Enum.GetValues(typeof(MipsCommand));
			byte[][] map = new byte[commands.Length][];
			var command = MipsCommand.ABOUT;

			byte[] vBytes = map[(int)command];

			var result = vBytes;
		}

		[TestMethod]
		public void IsAvailable(MipsCommand command)
		{
			//return map[(int)command] != null;
		}

		[TestMethod]
		public void CommandEnumerableValueMessageTest()
		{
			var command = MipsCommand.SDCBALL;
			IEnumerable<int> values = from value in Enumerable.Range(1, 32) select value;
			byte[][] arrayvalue=new byte[values.Count()][];
			int i = 0;

			foreach (var value in values)
			{
				byte[] result=Encoding.ASCII.GetBytes(value.ToString());
				arrayvalue[i] = result;
				i++;
			}

		}
	}
}
