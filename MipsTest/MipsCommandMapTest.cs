using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RJCP.IO.Ports;
using Xunit.Abstractions;
using Mips.Device;
using Xunit;

namespace MipsTest
{
	[TestClass]
	public class MipsCommandMapTest
	{
		public MipsCommandMapTest()
		{
			
		}
		private IMipsBox box;
		private ITestOutputHelper output;
		private SerialPortStream serialPort;
		public MipsCommandMapTest(ITestOutputHelper output)
		{
			this.output = output;
			serialPort = new SerialPortStream("COM1", 9600,8, Parity.Even, StopBits.One) { RtsEnable = false, Handshake = Handshake.XOn };

			box = MipsFactory.CreateMipsBox(serialPort);
		}
		[TestMethod]
		public void GetVersionTest()
		{
			var version = box.GetVersion().Result;
			output.WriteLine(version);
		}
		[TestMethod]
		public void GetErrorTest()
		{
			var value = box.GetError().Result;
			output.WriteLine(value);
		}
		[TestMethod]
		public void GetNameTest()
		{
			var value = box.GetName().Result;
			output.WriteLine(value);
		}
		[TestMethod]
		public void SetNameTest()
		{
			//var value = "";
			//box.SetName(Value);
			//output.WriteLine(value);
		}
		[TestMethod]
		public void AboutTest()
		{
			//var value = box.About().Result;
			//output.WriteLine(value);
		}
		[TestMethod]
		public void SetrevTest()
		{
			var value1 = 1;
			var value2 = "";

				
		}
		[TestMethod]
		public void ResetTest()
		{
			//var value = box.Reset().Result;
			//output.WriteLine(value);
		}
		//[TestMethod]
		//public void SaveTest()
		//{
		//	var value = box.Save().Result;
		//	output.WriteLine(value);
		//}
		//[TestMethod]
		//public void GetChannelTest()
		//{
		//	var value = box.GetChannel().Result;
		//	output.WriteLine(value);
		//}
		//[TestMethod]
		//public void MuteTest()
		//{
		//	var value = box.Mute().Result;
		//	output.WriteLine(value);
		//}
		[TestMethod]
		public void GetCommandTest()
		{

		}
		[TestMethod]
		public void EchoTest()
		{
			
		}
		[TestMethod]
		public void TrigoutTest()
		{
			
		}

		[TestMethod]
		public void DelayTest()
		{
		}

		
		[TestMethod]
		public void GetAnalogInputStatusTest()
		{
			
		}
		[TestMethod]
		public void SetAnalogInputStatusTest()
		{
			
		}
		[TestMethod]
		public void ThreadsTest()
		{
			
		}
		[TestMethod]
		public void SetThreadControlTest()
		{
			
		}
		[TestMethod]
		public void RDEVTest()
		{
			
		}
		[TestMethod]
		public void TableIOTest()
		{
			
		}
		[TestMethod]
		public void ADCTest()
		{
			
		}
		[TestMethod]
		public void OverrideLEDTest()
		{
			
		}
		[TestMethod]
		public void LEDColorTest()
		{
			
		}
		[TestMethod]
		public void GetDisplayTest()
		{
			var value = box.GetName().Result;
			output.WriteLine(value);
		}

	}
}
