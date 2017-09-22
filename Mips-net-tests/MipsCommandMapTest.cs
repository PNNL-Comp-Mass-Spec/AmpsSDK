using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Xunit.Abstractions;
using Xunit;
using Mips_net.Commands;
using Mips_net.Data;
using Mips_net.Device;

namespace MipsTest
{
	
	public class MipsCommandMapTest:IDisposable
	{
		private IMipsBox mipsBox;
		private ITestOutputHelper output;
		private SerialPort serialPort;
		public MipsCommandMapTest(ITestOutputHelper output)
		{
			this.output = output;

			serialPort = new SerialPort("COM4", 128000) { RtsEnable = true};

			mipsBox = MipsFactory.CreateMipsBox(serialPort);
		}

		//General Command
		[Fact]
		public void GetConfigTest()
		{
			var mipsBoxdata = mipsBox.GetConfig();
			output.WriteLine(mipsBoxdata.Result.NumberTWaveChannels.ToString());
		}

		[Fact]
		public void GetName()
		{
			var mipsBoxdata = mipsBox.GetName();
			output.WriteLine(mipsBoxdata.Result.ToString());
		}
		[Fact]
		public void GeTWaveNumberTest()
		{
			var mipsBoxdata = mipsBox.GetNumberTwaveChannels();
			output.WriteLine(mipsBoxdata.Result.ToString());
		}

		[Fact]
		public void GetMipsNameProperty()
		{
			var name = mipsBox.Name;
			output.WriteLine(mipsBox.Name);
		}

		[Theory]
		[InlineData("MIPSE")]
		public void SetNameTest(string name)
		{
			var value=mipsBox.SetName(name);
			output.WriteLine(value.ToString());
			
		}

		[Fact]
		public void GetVersionTest()
		{
			var version = mipsBox.GetVersion().Result;
			output.WriteLine(version);
		}

		[Fact]
		public void GetErrorTest()
		{
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void AboutTest()
		{
			var error = mipsBox.About().Result;
			output.WriteLine(error.ToString());
			output.WriteLine(Enum.GetNames(typeof(MipsCommand)).Length.ToString());
		}
		[Fact]
		public void GetCommandsTest()
		{
			var commands = mipsBox.GetCommand().Result;
			foreach (var command in commands)
			{
				output.WriteLine(command);
			}

		}
		[Theory]
		[InlineData(Modules.RF)]
		public void GetChannleTest(Modules channel)
		{
			var value = mipsBox.GetChannel(channel).Result;
			output.WriteLine(value.ToString());
		}
		[Theory]
		[InlineData(State.OFF)]
		public void MuteTest(State mute)
		{
			 mipsBox.Mute(mute);
		}
		[Theory]
		[InlineData(Status.FALSE)]
		public void EchoTest(Status echo)
		{
			mipsBox.Echo(echo);
		}
		[Fact]
		public void GetAnalogInputTest()
		{
			var analog = mipsBox.GetAnalogInputStatus().Result;
			
			output.WriteLine(analog.ToString());
		}
		[Theory]
		[InlineData(Status.FALSE)]
		public void SetAnalogInputTest(Status status)
		{
			mipsBox.SetAnalogInputStatus(status);
			output.WriteLine("Done");
		}

		[Fact]
		public void ThreadsTest()
		{
			var threads = mipsBox.Threads().Result;
			foreach (var v in threads)
			{
				output.WriteLine(v);
			}
		}
		[Theory]
		[InlineData("DCbias","TRUE")]
		public void SetThreadsTest(string name,string value1)
		{
			 mipsBox.SetThreadControl(name,value1);
			output.WriteLine("Done");
		}
		[Theory]
		[InlineData(1)]
		public void RDEV2Test(int channel)
		{
			var value = mipsBox.ReadADC2(channel).Result;
			output.WriteLine(value);
		}

		
		[Theory]
		[InlineData(1)]
		public void RDEVTest(int channel)
		{
			var ADC = mipsBox.ReadADC(channel).Result;
			output.WriteLine(ADC);
		}
		[Theory]
		[InlineData(3)]
		public void ADCTest(int channel)
		{
			var ADC = mipsBox.ReadADCChannel(channel).Result;
			output.WriteLine(ADC.ToString());
		}
		[Theory]
		[InlineData(true)]
		public void LEDOverrideTest(bool LEDValue)
		{
			
			mipsBox.LEDOverride(LEDValue);
			
		}
		[Theory]
		[InlineData(4)]
		public void LEDColorTest(int value)
		{

			mipsBox.LEDColor(value);

		}
		
		//Clock Generation
		[Fact]
		public void GetClockWidthTest()
		{
			var width = mipsBox.GetClockPulseWidth().Result;
			output.WriteLine(width.ToString());
			
		}
		[Theory]
		[InlineData(5)]
		public void SetClockWidthTest(int value1)
		{
			mipsBox.SetClockPulseWidth(value1);
			output.WriteLine("Done");
		}
		[Fact]
		public void GetClockFreQTest()
		{
			var width = mipsBox.GetClockFrequency().Result;
			output.WriteLine(width.ToString());

		}
		[Theory]
		[InlineData(200)]
		public void SetClockFreQTest(int value1)
		{
			mipsBox.SetClockFrequency(value1);
			
		}
		//DC Bias Module

		[Theory]
		[InlineData("1",0)]
		public void SetDcVoltageTest(string channel, double volts)
		{
			mipsBox.SetDcVoltage(channel,volts);
			output.WriteLine("Done");

		}
		[Theory]
		[InlineData("1")]
		public void GetDcSetpointTest(string channel)
		{
			var volts=mipsBox.GetDcSetpoint(channel).Result;
			output.WriteLine(volts.ToString());

		}
		[Theory]
		[InlineData("1")]
		public void GetDcReadbackTest(string channel)
		{
			var volts = mipsBox.GetDcReadback(channel).Result;
			output.WriteLine(volts.ToString());

		}
		[Theory]
		[InlineData("1",0)]
		public void SetDcOffsetTest(string channel, double offsetVolts)
		{
			 mipsBox.SetDcOffset(channel,offsetVolts);
			output.WriteLine("Done");

		}
		[Theory]
		[InlineData("1")]
		public void GetDcOffsetTest(string channel)
		{
			var volts=mipsBox.GetDcOffset(channel).Result;
			output.WriteLine(volts.ToString());

		}
		[Theory]
		[InlineData("1")]
		public void GetMaximumVoltageTest(string channel)
		{
			var volts = mipsBox.GetMaximumVoltage(channel).Result;
			output.WriteLine(volts.ToString());

		}
		[Theory]
		[InlineData("1")]
		public void GetMinimumVoltageTest(string channel)
		{
			var volts = mipsBox.GetMinimumVoltage(channel).Result;
			output.WriteLine(volts.ToString());

		}
		[Theory]
		[InlineData(State.OFF)]
		public void SetDcPowerStateTest(State state)
		{
			mipsBox.SetDcPowerState(state);
			output.WriteLine("Done");

		}
		[Fact]
		public void GetDcPowerStateTest()
		{
			var volts = mipsBox.GetDcPowerState().Result;
			output.WriteLine(volts.ToString());

		}

		
		[Fact]
		public void SetAllDcChannelsTest()
		{
			var channels = from values in Enumerable.Repeat(0.00, 8)
							select values;
			mipsBox.SetAllDcChannels(channels);
			

		}
		[Fact]
		public void GetAllDcSetpointsTest()
		{

			IEnumerable<double> result= mipsBox.GetAllDcSetpoints().Result;
			foreach (var v in result)
			{
				output.WriteLine(v.ToString());
			}
			

		}
		[Fact]
		public void GetAllDcReadbacksTest()
		{

			IEnumerable<double> result = mipsBox.GetAllDcReadbacks().Result;
			foreach (var v in result)
			{
				output.WriteLine(v.ToString());
			}


		}
		[Theory]
		[InlineData(2)]
		public void SetUniversalOffsetTest(double voltageOffset)
		{
			mipsBox.SetUniversalOffset(voltageOffset);
			output.WriteLine("Done");

		}
		[Theory]
		[InlineData(1,8)]
		public void SetNumberOfChannelsOnboardTest(int board, int numberChannels)
		{
			mipsBox.SetNumberOfChannelsOnboard(board,numberChannels);
			output.WriteLine("Done");

		}
		[Theory]
		[InlineData(Status.TRUE)]
		public void UseSingleOffsetTwoModulesTest(Status status)
		{
			mipsBox.UseSingleOffsetTwoModules(status);
			output.WriteLine("Done");

		}
		[Theory]
		[InlineData(Status.FALSE)]
		public void EnableOffsetReadback(Status enableReadback)
		{
			mipsBox.EnableOffsetReadback(enableReadback);
			output.WriteLine("Done");

		}
		[Theory]
		[InlineData(1,Status.FALSE)]
		public void EnableBoardOffsetOptionTest(int board, Status enable)
		{
			mipsBox.EnableBoardOffsetOption(board,enable);
			output.WriteLine("Done");

		}
		//DC BIAS PROFILE

		[Fact]
		public void SetDCbiasProfileTest()
		{
			int profile = 1;
			var channels = from values in Enumerable.Repeat(5, 8)
					select values;
			mipsBox.SetDCbiasProfile(profile,channels);
			
		}
		[Fact]
		public void OutputWithDCbiasProfileTest()
		{
			int profile = 1;
			mipsBox.OutputWithDCbiasProfile(profile);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void CopiesToDCbiasProfileTest()
		{
			int profile = 1;
			mipsBox.CopiesToDCbiasProfile(profile);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void ToggleProfileTest()
		{
			int profile1 = 1;
			int profile2 = 2;
			int time = 10;
			mipsBox.ToggleProfile(profile1, profile2,  time);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void StopToggleTest()
		{
			mipsBox.StopToggleProfile();
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Theory]
		[InlineData(1)]
		public void GetDCbiasProfileTest(int profile)
		{
			var volts = mipsBox.GetDCbiasProfile(profile).Result;
			foreach (var v in volts)
			{
				output.WriteLine(v.ToString());
			}

		}
		//Rf Driver

		[Theory]
		[InlineData("1",1000010)]
		public void SetFrequencyTest(string channel, int frequencyInHz)
		{
			mipsBox.SetFrequency(channel,frequencyInHz);
			output.WriteLine("Done");
		}
		[Theory]
		[InlineData("1")]
		public void GetFrequencyTest(string channel)
		{
			var result=mipsBox.GetFrequency(channel).Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData("1",2)]
		public void SetLevelTest(string channel, int peakToPeakVoltage)
		{
			mipsBox.SetLevel( channel, peakToPeakVoltage);
			output.WriteLine("Done");
		}
		[Theory]
		[InlineData("1")]
		public void GetPeakToPeakVoltageSetpointTest(string channel)
		{
			var value = mipsBox.GetPeakToPeakVoltageSetpoint(channel).Result;
			output.WriteLine(value.ToString());
		}
		[Theory]
		[InlineData("1", 2)]
		public void SetDriveLevelTest(string channel, int drive)
		{
			mipsBox.SetDriveLevel(channel, drive);
			output.WriteLine("Done");
		}
		[Theory]
		[InlineData("1")]
		public void GetOutputDriveLevelPercentTest(string channel)
		{
			var volts = mipsBox.GetOutputDriveLevelPercent(channel).Result;
			output.WriteLine(volts.ToString());
		}
		[Theory]
		[InlineData("1")]
		public void GetPositiveComponentTest(string channel)
		{
			var volts = mipsBox.GetPositiveComponent(channel).Result;
			output.WriteLine(volts.ToString());

		}
		[Theory]
		[InlineData("1")]
		public void GetNegativeComponentTest(string channel)
		{
			var volts = mipsBox.GetNegativeComponent(channel).Result;
			output.WriteLine(volts.ToString());

		}
		[Theory]
		[InlineData("1")]
		public void GetChannelPowerTest(string channel)
		{
			var volts = mipsBox.GetChannelPower(channel).Result;
			output.WriteLine(volts.ToString());

		}
		[Fact]
		public void GetParameterTest()
		{
			var parameters= mipsBox.GetParameters().Result;
			foreach (var v in parameters)
			{
				output.WriteLine(v.ToString());
			}

		}

		//DIO Module

		[Theory]
		[InlineData("A",1)]
		public void SetDigitalOutputTest(string channel, int state)
		{
			 mipsBox.SetDigitalOutput(channel, state);
			output.WriteLine("Done");

		}
		[Theory]
		[InlineData("A")]
		public void GetDigitalStateTest(string channel)
		{
			var volts = mipsBox.GetDigitalState(channel).Result;
			output.WriteLine(volts.ToString());

		}
		//ESI module

		[Theory]
		[InlineData(1, 1)]
		public void SetEsiVoltageTest(int channel, int volts)
		{
			mipsBox.SetEsiVoltage(channel, volts);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

		}
		[Theory]
		[InlineData(1)]
		public void GetEsiVoltageTest(int channel)
		{
			var val=mipsBox.GetEsiSetpointVoltage(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(val.ToString());
			output.WriteLine(error.ToString());

		}
		[Theory]
		[InlineData(14)]
		public void GetEsiReadbackVoltageTest(int channel)
		{
			var val = mipsBox.GetEsiReadbackVoltage(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(val.ToString());
			output.WriteLine(error.ToString());

		}
		[Theory]
		[InlineData(1)]
		public void GetEsiReadbackCurrentTest(int channel)
		{
			var val = mipsBox.GetEsiReadbackCurrent(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(val.ToString());
			output.WriteLine(error.ToString());

		}
		[Theory]
		[InlineData("Q",DigitalEdge.RISING)]
		public void ReportDigitalInputState(string input,DigitalEdge edge)
		{

			var val = mipsBox.ReporInputChannelState(input,edge).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

		}
		[Theory]
		[InlineData("Q","A")]
		public void MirrorTest(string input, string outChannel)
		{

			var val = mipsBox.MirrorInputToOutput(input, outChannel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

		}
		[Theory]
		[InlineData(1)]
		public void GetMaximumEsiVoltageTest(int channel)
		{
			var val = mipsBox.GetEsiReadbackCurrent(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

		}

		//MACRO Module

		[Fact]
		public void ListMacroTest()
		{
			var list = mipsBox.ListMacro().Result;
			output.WriteLine(list);

		}
		[Fact]
		public void DeleteMacroTest()
		{
			mipsBox.DeleteMacro("TRIAL");
			
		}
		[Fact]
		public void RecordMacroTest()
		{
			mipsBox.RecordMacro("TRIAL");
			mipsBox.StopMacro();
		}
		[Fact]
		public void PlayMacroTest()
		{
			mipsBox.PlayMacro("TRIAL");
		}

		//TWAVE Module
		[Theory]
		[InlineData("1")]
		public void GetTWaveFrequencyTest(string channel)
		{
			var result=mipsBox.GetTWaveFrequency(channel).Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData("1",99000)]
		public void SetTWaveFrequencyTest(string channel, int frequency)
		{
			mipsBox.SetTWaveFrequency(channel, frequency);
		}
		[Theory]
		[InlineData("1")]
		public void GetTWavePulseVoltageTest(string channel)
		{
			var result = mipsBox.GetTWavePulseVoltage(channel).Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData("1", 15)]
		public void SetTWavePulseVoltageTest(string channel,double voltage)
		{
			mipsBox.SetTWavePulseVoltage(channel, voltage);
		}
		[Theory]
		[InlineData("1")]
		public void GetTWaveGuard1VoltageTest(string channel)
		{
			var result = mipsBox.GetTWaveGuard1Voltage( channel).Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData("1", 10)]
		public void SetTWaveGuard1VoltageTest(string channel,double voltage)
		{
			mipsBox.SetTWaveGuard1Voltage(channel, voltage);
		}
		[Theory]
		[InlineData("1")]
		public void GetTWaveGuard2VoltageTest(string channel)
		{
			var result = mipsBox.GetTWaveGuard2Voltage(channel).Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData("1", 10)]
		public void SetTWaveGuard2VoltageTest(string channel, int voltage)
		{
			mipsBox.SetTWaveGuard2Voltage(channel, voltage);
		}
		[Fact]
		public void SetTWaveSequenceTest()
		{
			string channel = "2";
			bool[] boolArray = {true, false};
			BitArray bitArray= new BitArray(boolArray);
			mipsBox.SetTWaveSequence(channel, bitArray);
		}

		[Fact]
		public void BitArrayTest()
		{
			string channel = "11";
			List<bool> bitPattern = new List<bool>();
			foreach (var v in channel.ToCharArray())
			{
				bitPattern.Add(true);
			}

			BitArray sequence = new BitArray(bitPattern.ToArray());
			foreach (bool b in sequence)
			{
				output.WriteLine(b.ToString());
			}
		}
		[Theory]
		[InlineData("2")]
		public void GetTWaveSequenceTest(string channel)
		{
			var result = mipsBox.GetTWaveSequence(channel).Result;
			foreach (var v in result)
			{
				output.WriteLine(v.ToString());
			}
			

		}
		[Theory]
		[InlineData("1")]
		public void GetTWaveDirectionTest(string channel)
		{
			var result = mipsBox.GetTWaveDirection(channel).Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData("1", TWaveDirection.FWD)]
		public void SetTWaveDirectionTest(string channel, TWaveDirection direction)
		{
			mipsBox.SetTWaveDirection(channel, direction);
		}
		[Fact]
		public void GetTWaveMultipassTableStringTest()
		{
			var result = mipsBox.GetTWaveCompressionCommand().Result;
			output.WriteLine(result.ToString());
		}
		[Fact]
		public void SetTWaveMultipassControlTableTest()
		{ 
			mipsBox.SetTWaveCompressionCommand(CompressionTable.GetCompressionTable());
		}
		[Fact]
		public void GetCompressorModeTest()
		{
			var result = mipsBox.GetCompressorMode().Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData(StateCommands.N)]
		public void SetCompressorModeTest(StateCommands mode)
		{
			mipsBox.SetCompressorMode( mode);
		}
		[Fact]
		public void GetCompressorOrderTest()
		{
			var result = mipsBox.GetCompressorOrder().Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData(3)]
		public void SetCompressorOrderTest(int order)
		{
			mipsBox.SetCompressorOrder(order);
		}
		[Fact]
		public void GetCompressorTriggerDelayTest()
		{
			var result = mipsBox.GetCompressorTriggerDelay().Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData(1)]
		public void SetCompressorTriggerDelayTest(double delayMilliseconds)
		{
			mipsBox.SetCompressorTriggerDelay(delayMilliseconds);
		}
		[Fact]
		public void GetCompressionTime()
		{
			var result = mipsBox.GetCompressionTime().Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData(1)]
		public void SetCompressionTime(int timeMilliseconds)
		{
			mipsBox.SetCompressionTime(timeMilliseconds);
		}
		[Fact]
		public void GetNormalTime()
		{
			var result = mipsBox.GetNormalTime().Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData(1)]
		public void SetNormalTime(int timeMilliseconds)
		{
			mipsBox.SetNormalTime(timeMilliseconds);
		}
		[Fact]
		public void GetNonCompressTimeTest()
		{
			var result = mipsBox.GetNonCompressTime().Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData(1)]
		public void SetNonCompressTimeTEst(int timemilliSeconds)
		{
			mipsBox.SetNonCompressTime(timemilliSeconds);
		}
		[Theory]
		[InlineData(SwitchState.Open)]
		public void SetSwitchState(SwitchState state)
		{
			mipsBox.SetSwitchState( state);
			
		}
		[Fact]
		public void GetSwitchState()
		{
			var result = mipsBox.GetSwitchState().Result;
			output.WriteLine(result.ToString());
		}

		//Sweep FREQ


		[Theory]
		[InlineData("1")]
		public void GetStartFrequency(string channel)
		{
			var result = mipsBox.GetSweepStartFrequency(channel).Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData("1", 100000)]
		public void SetStartFrequencyTest(string channel, int frequency)
		{
			mipsBox.SetSweepStartFrequency(channel, frequency);

		}
		[Theory]
		[InlineData("1")]
		public void GetStopFrequencyTest(string channel)
		{
			var result = mipsBox.GetSweepStopFrequency(channel).Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData("1", 200000)]
		public void SetStopFrequencyTest(string channel, int frequency)
		{
			mipsBox.SetSweepStopFrequency(channel, frequency);

		}
		[Theory]
		[InlineData("1")]
		public void GetSweepTimeTest(string channel)
		{
			var result = mipsBox.GetSweepTime(channel).Result;
			output.WriteLine(result.ToString());
		}
		[Theory]
		[InlineData("1", 10)]
		public void SetSweepTimeTest(string channel,double timeInSeconds)
		{
			mipsBox.SetSweepTime(channel, timeInSeconds);

		}
		[Theory]
		[InlineData("1")]
		public void StartSweep(string channel)
		{
			 mipsBox.StartSweep(channel);
			
		}
		[Theory]
		[InlineData("1")]
		public void StopSweepTest(string channel)
		{
			mipsBox.StopSweep(channel);

		}

		//WiFi MOdule

		[Theory]
		[InlineData("1")]
		public void GetStatusTest(string channel)
		{
			var result = mipsBox.GetSweepStatus(channel).Result;
			output.WriteLine(result.ToString());

		}
		[Fact]
		public void GetHostNameTets()
		{
			var result = mipsBox.GetHostName().Result;
			output.WriteLine(result.ToString());

		}
		[Fact]
		public void GetSSIDTest()
		{
			var result = mipsBox.GetSSID().Result;
			output.WriteLine(result.ToString());

		}
		[Fact]
		public void GetWifiPassowedTest()
		{
			var result = mipsBox.GetWiFiPassword().Result;
			output.WriteLine(result.ToString());

		}
		[Theory]
		[InlineData("MIPSnet")]
		public void SetHostNameTest(string name)
		{
			 mipsBox.SetHostName(name);
			

		}
		[Theory]
		[InlineData("MIPS")]
		public void SetSSIDTest(string name)
		{
			mipsBox.SetSSID(name);
			
		}
		[Theory]
		[InlineData("MIPS1234")]
		public void SetWiFIPasswordTest(string name)
		{
			mipsBox.SetWiFiPassword(name);
			
		}
		[Theory]
		[InlineData( true)]
		public void WiFIInterfaceTest(bool name)
		{
			 mipsBox.EnablesInterface(name);
			
		}

		[Fact]
		public void MipsSignalTableTest()
		{
			var signalTable = new MipsSignalTable();
			signalTable = signalTable.AddTimePoint(0, new LoopData());
			signalTable[0].CreateOutput("1", 10);
			signalTable = signalTable.AddTimePoint(10, new LoopData());
			signalTable[10].CreateOutput("1", 5);

			signalTable.Points.LastOrDefault()?.ReferenceTimePoint(signalTable.Points.FirstOrDefault(), 10);
			var encodedString = signalTable.RetrieveTableAsEncodedString();
			output.WriteLine(encodedString);

			

			mipsBox.SetSignalTable(signalTable).Wait();
			 var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			mipsBox.SetMode(Modes.TBL).Wait();
			 error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			
			mipsBox.SetClock(ClockType.MCK128).Wait();
			error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			mipsBox.SetTrigger(SignalTableTrigger.SW).Wait();
			error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			var freq = mipsBox.GetClockFreuency().Result;
			output.WriteLine(freq.ToString());

			mipsBox.SetTableNumber(1).Wait();
			error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			mipsBox.SetAdvanceTableNumber(State.ON).Wait();
			error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			var state = mipsBox.GetAdvanceTableNumber().Result;
			output.WriteLine(state.ToString());

			mipsBox.SetChannelValue(10, "A", 1).Wait();
			error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			var val = mipsBox.GetChannelValue(10, "A").Result;
			output.WriteLine(val.ToString());

			var status = mipsBox.GetStatusReply().Result;
			output.WriteLine(status.ToString());

			mipsBox.SetChannelCount(10, "1", 2).Wait();
			error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			mipsBox.StartTimeTable().Wait();
			error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			mipsBox.SetTableDelay(100).Wait();
			error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			mipsBox.SetSoftwareGeneration(Status.TRUE).Wait();
			error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			mipsBox.EnablesRelpy(Status.TRUE).Wait();
			error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());


			mipsBox.StopTable().Wait();
			 error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());

			mipsBox.AbortTimeTable().Wait();
			error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}

		//Eithernet
		[Fact]
		public void GetIPTest()
		{
			var ip=	mipsBox.GetIP().Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(ip);
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetIPTest()
		{
			string ip = "";
			mipsBox.SetIP(ip);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetPortNumber()
		{
			
			var port=mipsBox.GetPortNumber().Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(port.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetPortNumber()
		{
			int port = 2015;
			mipsBox.SetPortNumber(port);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetGatewayIPTest()
		{
			var gate=mipsBox.GetGatewayIP().Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
			output.WriteLine(gate.ToString());
		}
		[Fact]
		public void SetGatewayIPTest()
		{
			string ip = "000.000.000.000";
			mipsBox.SetGatewayIP( ip);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}

		//FAIMS

		[Fact]
		public void SetPositiveOutputTest()
		{
			int slope = 10;
			int offset = 10;
			mipsBox.SetPositiveOutput(slope,offset);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetNegativeOutputTest()
		{
			int slope = 10;
			int offset = 10;
			mipsBox.SetNegativeOutput(slope,offset);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}

		//Filamet Module

		[Fact]
		public void GetFilamentEnableTest()
		{
			string channel = "A";
			//int current = 10;
			var value=mipsBox.GetFilamentEnable(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetFilamentEnableTest()
		{
			string channel = "A";
			State state = State.OFF;
			mipsBox.SetFilamentEnable(channel,state);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetFilamentSetPointCurrentTest()
		{
			string channel = "A";
			//int current = 10;
			var value=mipsBox.GetFilamentSetPointCurrent(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetFilamentActualCurrentTest()
		{
			string channel = "A";
			//int current = 10;
			var value = mipsBox.GetFilamentActualCurrent(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetFilamentCurrentTest()
		{
			string channel = "A";
			int current = 5;
			mipsBox.SetFilamentCurrent(channel, current);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetFilamentSetPointVoltageTest()
		{
			string channel = "1";
			//int current = 10;
			var value = mipsBox.GetFilamentSetPointVoltaget(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetFilamentActualVoltageTest()
		{
			string channel = "A";
			//int current = 10;
			var value = mipsBox.GetFilamentActualVoltage(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetFilamentSetVoltageTest()
		{
			string channel = "A";
			int current = 5;
			mipsBox.SetFilamentSetVoltage(channel, current);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetFilamentPowerTest()
		{
			string channel = "A";
			//int current = 10;
			var value = mipsBox.GetFilamentPower(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetFilamentRampRateTest()
		{
			string channel = "A";
			//int current = 10;
			var value = mipsBox.GetFilamentRampRate(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetCyclingCurrent1Test()
		{
			string channel = "A";
			//int current = 10;
			var value = mipsBox.GetCyclingCurrent1(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetFilamentRampRateTest()
		{
			string channel = "A";
			int current = 5;
			mipsBox.SetFilamentRampRate(channel, current);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetCyclingCurrent1Test()
		{
			string channel = "A";
			int current = 5;
			mipsBox.SetCyclingCurrent1(channel, current);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetCyclingCurrent2Test()
		{
			string channel = "A";
			//int current = 10;
			var value = mipsBox.GetCyclingCurrent2(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetCyclingCurrent2Test()
		{
			string channel = "A";
			int current = 5;
			mipsBox.SetCyclingCurrent2(channel, current);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetCycleTest()
		{
			string channel = "A";
			//int current = 10;
			var value = mipsBox.GetCycle(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetCycleStatusTest()
		{
			string channel = "A";
			//int current = 10;
			var value = mipsBox.GetCycleStatus(channel).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetCycleTest()
		{
			string channel = "A";
			int current = 5;
			mipsBox.SetCycle(channel, current);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetCycleStatusTest()
		{
			string channel = "A";
			//int current = 5;
			mipsBox.SetCycleStatus(channel, State.OFF);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetCurrentToHostsTest()
		{
			//string channel = "A";
			double res = 10;
			var value = mipsBox.GetCurrentToHost(res).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetResistorTest()
		{
			string channel = "A";
			int time = 5;
			mipsBox.SetResistor(channel, time);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetResistorTest()
		{
			//string channel = "A";
			//double res = 10;
			var value = mipsBox.GetResistor().Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetEmissionCurrentTest()
		{
			//string channel = "A";
			double res = 10;
			var value = mipsBox.GetEmissionCurrent().Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(value.ToString());
			output.WriteLine(error.ToString());
		}
		//Delay Trigger

		[Fact]
		public void SetTriggerChannelLevelTest()
		{
			string channel = "Q";
			TriggerLevel trigger=TriggerLevel.POS;
			mipsBox.SetTriggerChannelLevel(channel,trigger);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetTriggerDelayTest()
		{
			//string channel = "Q";
			double delayTime = 10;
			mipsBox.SetTriggerDelay(delayTime);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetTriggerDelayTest()
		{
			var result=mipsBox.GetTriggerDelay().Result;
			output.WriteLine(result.ToString());

			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetTriggerPeriodTest()
		{
			//string channel = "Q";
			int delayPeriod = 10;
			mipsBox.SetTriggerPeriod(delayPeriod);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetTriggerPeriodTest()
		{
			var result = mipsBox.GetTriggerPeriod().Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(result.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetTriggerRepeatCountTest()
		{
			
			int count = 10;
			mipsBox.SetTriggerRepeatCount(count);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetTriggerRepeatCountTest()
		{
			var result = mipsBox.GetTriggerRepeatCount().Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(result.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetTriggerModuleTest()
		{
			ArbMode module = ArbMode.ARB;
			mipsBox.SetTriggerModule(module);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void EnableDelayTriggerTest()
		{

			Status enable = Status.TRUE;
			mipsBox.EnableDelayTrigger(enable);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetDelayTriggerStatusTest()
		{
			var result = mipsBox.GetDelayTriggerStatus().Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(result.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetArbModeTest()
		{
			int module = 1;
			ArbMode mode=ArbMode.ARB;
			var result=mipsBox.SetArbMode(module,mode).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(result.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetArbModeTest()
		{
			int module = 1;
			var result = mipsBox.GetArbMode(module).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(result.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetArbFrequencyTest()
		{
			int module = 1;
			int freq = 10000;
			 mipsBox.SetArbFrequency(module, freq);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetArbFrequencyTest()
		{
			int module = 1;
			var result = mipsBox.GetArbFrequency(module).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(result.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetArbVoltsPeakToPeakTest()
		{
			int module = 1;
			var result = mipsBox.GetArbVoltsPeakToPeak(module).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(result.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetArbVoltsPeakToPeakTest()
		{
			int module = 1;
			int volts= 10000;
			mipsBox.SetArbVoltsPeakToPeak(module, volts);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetArbOffsetVoltageTest()
		{
			int module = 1;
			var result = mipsBox.GetArbOffsetVoltage(module).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(result.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetArbOffsetVoltageTest()
		{
			int module = 1;
			int volts = 5;
			mipsBox.SetArbOffsetVoltage(module, volts);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetAuxOutputVoltageTest()
		{
			int module = 1;
			var result = mipsBox.GetAuxOutputVoltage(module).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(result.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetAuxOutputVoltageTest()
		{
			int module = 1;
			int volts = 5;
			mipsBox.SetAuxOutputVoltage(module, volts);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void StartArbTest()
		{
			int module = 1;
			mipsBox.StartArb(module);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void StopArbTest()
		{
			int module = 1;
			mipsBox.StopArb(module);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetTwaveDirectionTest()
		{
			int module = 1;
			var result = mipsBox.GetTwaveDirection(module).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(result.ToString());
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetTwaveDirectionTest()
		{
			int module = 1;
			TWaveDirection dir=TWaveDirection.REV;
			mipsBox.SetTwaveDirection(module, dir);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetWaveformTest()
		{
			int module = 1;
			IEnumerable<int> result = mipsBox.GetWaveform(module).Result;
			foreach (var v in result)
			{
				output.WriteLine(v.ToString());
			}
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetWaveformTest()
		{
			int module = 1;
			IList<int> list=new List<int>();
			IEnumerable<int> val = from values in Enumerable.Repeat(2, 33)
									select values;
			foreach (var v in val)
			{
				list.Add(v);
			}
			mipsBox.SetWaveform(module, list);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetWaveformTypeTest()
		{
			int module = 1;
			var result=mipsBox.GetWaveformType(module).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
			output.WriteLine(result.ToString());
		}
		[Fact]
		public void SetWaveformTypeTest()
		{
			int module = 1;
			ArbWaveForms waveForms=ArbWaveForms.ARB;
			mipsBox.SetWaveformType(module, waveForms);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetBufferLengthTest()
		{
			int module = 1;
			var result = mipsBox.GetBufferLength(module).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
			output.WriteLine(result.ToString());
		}
		[Fact]
		public void SetBufferLengthTest()
		{
			int module = 1;
			int length = 5;
			mipsBox.SetBufferLength(module, length);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void GetBufferRepeatTest()
		{
			int module = 1;
			var result = mipsBox.GetBufferRepeat(module).Result;
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
			output.WriteLine(result.ToString());
		}
		[Fact]
		public void SetBufferRepeatTest()
		{
			int module = 1;
			int length = 5;
			mipsBox.SetBufferRepeat(module, length);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		
		[Fact]
		public void SetAllChannelValueTest()
		{
			int module = 1;
			int length = 5;
			mipsBox.SetAllChannelValue(module, length);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetChannelValueTest()
		{
			int module = 1;
			int channel =1;
			int value = 10;
			mipsBox.SetChannelValue(module, channel,value);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}
		[Fact]
		public void SetChannelRangeTest()
		{
			int module = 1;
			int channel = 1;
			int start = 1;
			int stop = 1;
			int offset = 1;
			mipsBox.SetChannelRange(module, channel,start,stop,offset);
			var error = mipsBox.GetError().Result;
			output.WriteLine(error.ToString());
		}

		//ARB CompressionCommand Module

		[Fact]
		public void GetArbCompressorModeTest()
		{
			var result = mipsBox.GetArbCompressorMode().Result;
			output.WriteLine(result.ToString());
		}
		[Fact]
		public void SetArbCompressorModeTest()
		{
			StateCommands mode=StateCommands.N;
			mipsBox.SetArbCompressorMode(mode);
		}
		[Fact]
		public void GetArbCompressorOrderTest()
		{
			var result = mipsBox.GetArbCompressorOrder().Result;
			output.WriteLine(result.ToString());
		}
		[Fact]
		public void SetArbCompressorOrderTest()
		{
			int order = 2;
			mipsBox.SetArbCompressorOrder(order);
		}
		[Fact]
		public void GetArbTriggerDelayTest()
		{
			var result = mipsBox.GetArbTriggerDelay().Result;
			output.WriteLine(result.ToString());
		}
		[Fact]
		public void SetArbTriggerDelayTest()
		{
			double order = 2;
			mipsBox.SetArbTriggerDelay(order);
		}
		[Fact]
		public void GetArbCompressionTimeTest()
		{
			var result = mipsBox.GetArbCompressionTime().Result;
			output.WriteLine(result.ToString());
		}
		[Fact]
		public void SetArbCompressionTimeTest()
		{
			double order = 10;
			mipsBox.SetArbCompressionTime(order);
		}
		[Fact]
		public void GetArbNormalTimeTest()
		{
			var result = mipsBox.GetArbNormalTime().Result;
			output.WriteLine(result.ToString());
		}
		[Fact]
		public void SetArbNormalTimeTest()
		{
			double order = 10;
			mipsBox.SetArbNormalTime(order);
		}
		[Fact]
		public void GetArbNonCompressionTimeTest()
		{
			var result = mipsBox.GetArbNonCompressionTime().Result;
			output.WriteLine(result.ToString());
		}
		[Fact]
		public void StArbNonCompressionTimeTest()
		{
			int order = 10;
			mipsBox.SetArbNonCompressionTime(order);
		}
		[Fact]
		public void GetArbSwitchStateTest()
		{
			var result = mipsBox.GetArbSwitchState().Result;
			output.WriteLine(result.ToString());
		}
		[Fact]
		public void SetArbSwitchStateTest()
		{
			SwitchState state=SwitchState.Close;
			mipsBox.SetArbSwitchState(state);
		}
		[Fact]
		public void SetArbTrigger()
		{
			SwitchState state = SwitchState.Close;
			mipsBox.SetArbTrigger();
		}


		public void Dispose()
		{
			serialPort.Dispose();
		}

	}
}
