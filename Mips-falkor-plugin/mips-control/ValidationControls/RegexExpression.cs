using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mips_control.ValidationControls
{
	internal class RegexExpression
	{
		internal static Regex modeRegex = new Regex("^[012]$");
		internal static Regex switchStateRegex = new Regex("^[01]$");
		internal static  Regex commandValueRegex = new Regex("^[1-9][0-9]*$");
	}
}
