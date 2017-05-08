using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mips.Commands;

namespace MipsTest
{
	[TestClass]
	public class MessageTest
	{
		//public static readonly MipsMessage[] EmptyArray = new MipsMessage[0];

		protected MipsCommand command;

		internal DateTime createdDateTime = DateTime.UtcNow;
		internal long createdTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
		public MipsCommand Command => command;
		public virtual string CommandAndKey => Command.ToString();

		[TestMethod]
		public void TestMethod1()
		{

		
		}
	}
}
