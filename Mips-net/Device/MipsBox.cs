using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mips_net.Commands;
using Mips_net.Data;
using Mips_net.Io;

namespace Mips_net.Device
{
	[DataContract]
    internal sealed class MipsBox : IMipsBox
    {
		private readonly MipsCommunicator communicator;
	    private Lazy<MipsBoxDeviceData> deviceData;

	    private Queue<MipsMessage> messageQueue = new Queue<MipsMessage>();
	    private Queue<string> responseQueue = new Queue<string>();

		public MipsBox(MipsCommunicator communicator)
		{
			this.communicator = communicator?? throw new ArgumentNullException(nameof(communicator));
			this.communicator.Open();
			var source = this.communicator.MessageSources;

			source.Where(x => x != "tblcmplt" && !x.Contains("ABORTED") && x != "tblrdy" && !string.IsNullOrEmpty(x) && x != "TableNotReady").Select(s =>
			{
				this.responseQueue.Enqueue(s);
				return s;
			}).Subscribe();

			this.TableCompleteOrAborted = source
				.Where(x => x.Equals("tblcmplt", StringComparison.OrdinalIgnoreCase) || x.Contains("ABORTED")).Select(x => Unit.Default);
			this.TableCompleteOrAborted.Subscribe(unit =>
			{
				System.Diagnostics.Trace.WriteLine($"{Environment.NewLine}complete{DateTime.Now}");
			});

			this.ModeReady = source.Where(x => x.Equals("tblcmplt", StringComparison.OrdinalIgnoreCase)).Select(x => Unit.Default);
			this.ModeReady.Subscribe();
			ClockFrequency = 16000000;
		}
	    private async Task ProcessQueue(bool response=false)
	    {
		    
		    while (messageQueue.Count>0)
		    {
			    var message = messageQueue.Dequeue();
				message.WriteTo(this.communicator);
				Thread.Sleep(50);
			    while (response && responseQueue.Count==0)
			    {
				    Thread.Sleep(50);
			    }
			    break;
		    }
		   
	    }

	    public IObservable<Unit> TableCompleteOrAborted { get; }

	    public IObservable<Unit> ModeReady { get; }

		[DataMember]
	    public int ClockFrequency { get; set; }
	    [DataMember]
	    public string Name => GetName().Result;
	    public async Task<MipsBoxDeviceData> GetConfig()
	    {
		    var dcBiasChannels = await this.GetNumberDcBiasChannels();
		    var rfChannels = await this.GetNumberRfChannels();
		    //var digitalChannels = await this.GetNumberDigitalChannels();
		   // var twaveChannels = await this.GetNumberTwaveChannels();
		   // var arbChannels = await this.GetNumberARBChannels();

			this.deviceData = new Lazy<MipsBoxDeviceData>(() => new MipsBoxDeviceData((uint)dcBiasChannels,
								(uint)rfChannels,  (uint) 0, (uint)2,(uint)4));

		    //var twaveChannels = await this.GetNumberTwaveChannels();
		    //var arbChannels = await this.GetNumberARBChannels();
		    //var esiChannels = await this.GetNumberESIChannels();
		    //var faimsChannels = await this.GetNumberFaimsChannels();
		    //var filamentChannels = await this.GetNumberFilamentChannels();


			this.deviceData = new Lazy<MipsBoxDeviceData>(() => new MipsBoxDeviceData((uint)dcBiasChannels,
								(uint)rfChannels,  (uint) 0, (uint)2,(uint)4));

		    return this.deviceData.Value;
	    }

	    
	    public async Task<int> GetNumberARBChannels()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.Arb.ToString());
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result= responseQueue.Dequeue();
			int.TryParse(result, out int channels);
		    return channels;
		}
	    public async Task<int> GetNumberESIChannels()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.ESI.ToString());
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    int.TryParse(result, out int channels);
		    return channels;
	    }
	    public async Task<int> GetNumberFaimsChannels()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.FAIMS.ToString());
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    int.TryParse(result, out int channels);
		    return channels;
	    }
	    public async Task<int> GetNumberFilamentChannels()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.FIL.ToString());
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    int.TryParse(result, out int channels);
		    return channels;
	    }
		
		public async Task<int> GetNumberTwaveChannels()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.TWAVE.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
			int.TryParse(result, out int channels);
		    return channels;
		  
	    }

		public async Task<int> GetNumberDigitalChannels()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.DIO.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    int.TryParse(result, out int channels);
		    return channels;
		}

	    public  async Task<int> GetNumberRfChannels()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.RF.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    int.TryParse(result, out int channels);
		    return channels;
		}

	    public async Task<int> GetNumberDcBiasChannels()
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.DCB.ToString());
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue(true);
			var result = responseQueue.Dequeue();
			int.TryParse(result, out int channels);
			return channels;
		}

		public async Task<Unit> SetName(string name)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SNAME, name);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue(false);
			return Unit.Default;

		}


		public async Task<string> GetVersion()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GVER);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    return result;
		}

		public async Task<ErrorCode> GetError()
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.GERR);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue(true);
			var result = responseQueue.Dequeue();
			Enum.TryParse(result, out ErrorCode error);
			return error;
		}
	    public async Task<IEnumerable<string>> About()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.ABOUT);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			List<string> responses = new List<string>();
			Thread.Sleep(100);
		    while (responseQueue.Count > 0)
		    {
			    var response = responseQueue.Dequeue();
				if (string.IsNullOrEmpty(response))
			    {
				    continue;
			    }
			    responses.Add(response);
		    }
		    return responses;
		}
	    public async Task<Unit> RevisionLevel(int board,int module,int rev)
	    {
			//var mipsmessage = MipsMessage.Create(MipsCommand.SMREV);
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}

		public async Task<Unit> Reset()
	    {
		    //var mipsmessage = MipsMessage.Create(MipsCommand.RESEST);
		    //mipsmessage.WriteTo(communicator);
		    //var messagePacket = communicator.MessageSources;

		    //return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}
	    public async Task<Unit> Save()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SAVE);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
		}
	    public async Task<int> GetChannel(Modules module)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN,module.ToString());
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    int.TryParse(result, out int channels);
		    return channels;
		}
		public async Task<Unit> Mute(State mute)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.MUTE,mute.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
		}
	    public async Task<Unit> Echo(Status echo)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.ECHO,echo.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
		}
	    public async Task<Unit> TriggerOut(TriggerValue trigger)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.TRIGOUT, trigger.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
		}
	    public async Task<Unit> TriggerOut(int trigger)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.TRIGOUT, trigger);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
		}
		public async Task<Unit> Delay(double delay)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.DELAY, delay);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
		}
	    public async Task<IEnumerable<string>> GetCommand()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GCMDS);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    List<string> responses = new List<string>();
		    while (responseQueue.Count>0)
		    {
				var response = responseQueue.Dequeue();
			    if (string.IsNullOrEmpty(response))
			    {
				    continue;
			    }
			    responses.Add(response);
		    }
		    return responses;
		}
		  
	    public async Task<Status> GetAnalogInputStatus()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GAENA);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    Enum.TryParse(result, out Status status);
		    return status;
		}
	    public async Task<Unit> SetAnalogInputStatus(Status status)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SAENA,status.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}
	    public async Task<IEnumerable<string>> Threads()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.THREADS);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			List<string> responses = new List<string>();
		    while (responseQueue.Count > 0)
			{
				var response = responseQueue.Dequeue();
				if (string.IsNullOrEmpty(response))
				{
					continue;
				}
				responses.Add(response);
			}
			return responses;
		}
	    public async Task<Unit> SetThreadControl(string threadName, string threadValue)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.STHRDENA, threadName.ToString(),threadValue.ToString());
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			return Unit.Default;
		}
	    public async Task<Unit> SetADCAddress(int board, double address)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.SDEVADD, board, address);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			return Unit.Default;
		}

	    public async Task<string> ReadADC(int channel)
	    {
		    switch (channel)
		    {
			    case 0:
			    case 1:
			    case 2:
			    case 3:
			    case 4:
			    case 5:
			    case 6:
			    case 7:
				    var mipsmessage = MipsMessage.Create(MipsCommand.RDEV, channel);
					messageQueue.Enqueue(mipsmessage);
				    await ProcessQueue(true);
				   var  result = responseQueue.Dequeue();
				    return result;
			   default:
				    return Unit.Default.ToString();
		    }
	    }

	    public async Task<string> ReadADC2(int channel)
	    {
			//switch (channel)
			//{
			//	case 0:
			//	case 1:
			//	case 2:
			//	case 3:
			//		var mipsmessage = MipsMessage.Create(MipsCommand.RDEV2, channel);
			//		mipsmessage.WriteTo(communicator);
			//		var messagePacket = communicator.MessageSources;

			//		return await messagePacket.Select(bytes => bytes).FirstAsync();
			//	default:
			//		return Unit.Default.ToString();
			//}
		    throw new NotImplementedException();
		}

	    public async Task<Unit> SetIOTableMode(bool enable)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.TBLTSKENA,enable.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;

		}
	    public async Task<int> ReadADCChannel(int channel)
		{
			switch (channel)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					var mipsmessage = MipsMessage.Create(MipsCommand.ADC, channel);
					messageQueue.Enqueue(mipsmessage);
					await ProcessQueue(true);
					var result = responseQueue.Dequeue();
					int.TryParse(result, out int adcChannel);
					return adcChannel;
				default:
					return 0;
			}
		}
	    public async Task<Unit> LEDOverride(bool LEDValue)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.LEDOVRD, LEDValue.ToString());
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			var result = responseQueue.Dequeue();
			return Unit.Default;
		}
	    public async Task<Unit> LEDColor(int color)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.LED, color);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			var result = responseQueue.Dequeue();
			return Unit.Default;
		}
	    public async Task<Unit> DisplayOff(Status status)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.DSPOFF, status.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
		}

		//Clock Generation

	    public async Task<int> GetClockPulseWidth()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GWIDTH);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}
	    public async Task<Unit> SetClockPulseWidth(int microseconds)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.SWIDTH, microseconds);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			return Unit.Default;
		}

	    public async Task<int> GetClockFrequency()
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.GFREQ);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue(true);
			var response = responseQueue.Dequeue();
			int.TryParse(response, out int result);
			return result;
		}

	    public async Task<Unit> SetClockFrequency(int frequencyInHz)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.SFREQ,frequencyInHz);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue(true);
			var result = responseQueue.Dequeue();
			return Unit.Default;
		}

	    public async Task<Unit> ConfigureClockBurst(int numberCycles)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.BURST,numberCycles);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			var result = responseQueue.Dequeue();
			return Unit.Default;
		}

		//DC Bias Module

	    public async Task<Unit> SetDcVoltage(string channel, double volts)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCB,channel,volts);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetDcSetpoint(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCB, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<double> GetDcReadback(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCBV, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetDcOffset(string channel, double offsetVolts)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCBOF, channel, offsetVolts);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetDcOffset(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCBOF, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<double> GetMinimumVoltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCMIN, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<double> GetMaximumVoltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCMAX, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetDcPowerState(State state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCPWR, state.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<State> GetDcPowerState()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCPWR);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    Enum.TryParse(result, out State value);
		    return value;
		}

	    public async Task<Unit> SetAllDcChannels(IEnumerable<double> channels)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCBALL, channels);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

		public async Task<IEnumerable<double>> GetAllDcSetpoints()
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCBALL);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue(true);
			List<double> responses = new List<double>();
			var response = responseQueue.Dequeue();
			var values = response.Split(',');
			for(int i=0;i<values.Length;i++)
			{
				double.TryParse(values[i], out double result);
				responses.Add(result);
			}
			return responses;


		}

		public async Task<IEnumerable<double>> GetAllDcReadbacks()
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCBALLV);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue(true);
			List<double> responses = new List<double>();
			var response = responseQueue.Dequeue();
			var values = response.Split(',');
			for (int i = 0; i < values.Length; i++)
			{
				double.TryParse(values[i], out double result);
				responses.Add(result);
			}
			return responses;


		}

		public async Task<Unit> SetUniversalOffset(double voltageOffset)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCBDELTA, voltageOffset);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<Unit> SetNumberOfChannelsOnboard(int board, int numberChannels)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SDCBCHNS, board, numberChannels);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> UseSingleOffsetTwoModules(Status status)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SDCBONEOFF, status.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> EnableOffsetReadback(Status enableReadback)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.DCBOFFRBENA, enableReadback.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> EnableBoardOffsetOption(int board, Status enable)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCBOFFENA, board, enable.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

		

		//DC BIAS PROFILE

	    public async Task<Unit> SetDCbiasProfile(int profile, IEnumerable<int> channels)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCBPRO, profile, channels);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<IEnumerable<double>> GetDCbiasProfile(int profile)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCBPRO, profile);
			mipsmessage.WriteTo(communicator);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
			List<double> responses = new List<double>();
		    var response = responseQueue.Dequeue();
		    var values = response.Split(',');
		    for (int i = 0; i < values.Length; i++)
		    {
			    double.TryParse(values[i], out double result);
			    responses.Add(result);
		    }
		    return responses;

		}

		public  async Task<Unit> OutputWithDCbiasProfile(int profile)
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.ADCBPRO, profile);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			var result = responseQueue.Dequeue();
			return Unit.Default;
			//throw new NotImplementedException();

		}

	    public async Task<Unit> CopiesToDCbiasProfile(int profile)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.CDCBPRO, profile);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
			throw new NotImplementedException();
		}

	    public async Task<Unit> ToggleProfile(int profile1, int profile2, int time)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TDCBPRO, profile1, profile2, time);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
			throw new NotImplementedException();
		}

	    public async Task<Unit> StopToggleProfile()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TDCBSTOP);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
			throw new NotImplementedException();
		}

	    //Rf Driver

		public async Task<Unit> SetFrequency(string channel, double frequencyInHz)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SRFFRQ,channel,frequencyInHz);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetRfPeakToPeak(string channel, double peakToPeakVoltage)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SRFVLT,channel,peakToPeakVoltage);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetDriveLevel(string channel, double drive)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SRFDRV,channel,drive);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetFrequency(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFFRQ, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<double> GetRFPositive(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFPPVP, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<double> GetRFNegative(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFPPVN, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<double> GetOutputDriveLevelPercent(string channel)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GRFDRV, channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
	    }

	    public async Task<double> GetPeakToPeakVoltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFVLT, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<double> GetChannelPower(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFPWR, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

		public async Task<IEnumerable<double>> GetParameters()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFALL);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			List<double> responses = new List<double>();
		    var response = responseQueue.Dequeue();
		    var values = response.Split(',');
		    for (int i = 0; i < values.Length; i++)
		    {
			    double.TryParse(values[i], out double result);
			    responses.Add(result);
		    }
		    return responses;

		}

		//DioModule

		public async Task<Unit> SetDigitalOutput(string channel, int state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDIO, channel, state);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<int> GetDigitalState(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDIO, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<Unit> ReporInputChannelState(string channel, DigitalEdge edge)
	    {
			switch (channel)
			{
				case "Q":
				case "R":
				case "S":
				case "T":
				case "U":
				case "V":
				case "W":
				case "X":
					var mipsmessage = MipsMessage.Create(MipsCommand.RPT, channel, edge.ToString());
					messageQueue.Enqueue(mipsmessage);
					await ProcessQueue();
					return Unit.Default;
				default:
					return Unit.Default;
			}
			throw new NotImplementedException();

	    }

	    public async Task<Unit> MirrorInputToOutput(string input, string output)
	    {
			switch (input)
			{
				case "Q":
				case "R":
				case "S":
				case "T":
				case "U":
				case "V":
				case "W":
				case "X":
					switch (output)
					{
						case "A":
						case "B":
						case "C":
						case "D":
						case "E":
						case "F":
						case "G":
						case "H":
						case "I":
						case "J":
						case "K":
						case "L":
						case "M":
						case "N":
						case "O":
						case "P":
						case "OFF":
							var mipsmessage = MipsMessage.Create(MipsCommand.MIRROR, input, output);
							messageQueue.Enqueue(mipsmessage);
							await ProcessQueue();
							return Unit.Default;
						default:
							return Unit.Default;
					}

				default:
					return Unit.Default;
			}
			throw new NotImplementedException();

	    }

	   // IEsiModule

	    public async Task<Unit> SetEsiVoltage(int channel, int volts)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SHV, channel, volts);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    
	    public async Task<int> GetEsiSetpointVoltage(int channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GHV, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<int> GetEsiReadbackVoltage(int channel)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GHVV, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<int> GetEsiReadbackCurrent(int channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GHVI, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<int> GetMaximumEsiVoltage(int channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GHVMAX, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

		

		//MACRO Modules

	    public async Task<Unit> RecordMacro(string name)
		{
			//var mipsmessage = MipsMessage.Create(MipsCommand.MRECORD, name);
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
	    }

	    public async Task<Unit> StopMacro()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.MSTOP);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> PlayMacro(string name)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.MPLAY,name);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<string> ListMacro()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.MLIST);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    return result;
		}

	    public async Task<Unit> DeleteMacro(string name)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.MDELETE,name);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

		//TWAVE Module

	    public async Task<double> GetTWaveFrequency(string channel)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GTWF, channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    double.TryParse(result, out double freq);
		    return freq;
			
				
		}

	    public async Task<Unit> SetTWaveFrequency(string channel, double frequency)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWF, channel,frequency);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<double> GetTWavePulseVoltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWPV, channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response= responseQueue.Dequeue();
		    double.TryParse(response, out double result);
			return result;
		}

	    public async Task<Unit> SetTWavePulseVoltage(string channel,double voltage)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWPV, channel,voltage);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<Unit> SetTWaveGuard1Voltage(string channel,double voltage)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWG1V, channel,voltage);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetTWaveGuard1Voltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWG1V, channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetTWaveGuard2Voltage(string channel,double voltage)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWG2V, channel,voltage);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetTWaveGuard2Voltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWG2V, channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	   public async Task<BitArray> GetTWaveSequence(string channel)
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWSEQ, channel);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue(true);
			var result = responseQueue.Dequeue();
			var boolArray = new List<bool>();
			foreach (var v in result.ToCharArray())
			{
				boolArray.Add(v == '0' ? false : true);

			}
			var sequence = new BitArray(boolArray.ToArray());
			return sequence;
			throw new NotImplementedException();
	    }

	    public async Task<Unit> SetTWaveSequence(string channel, BitArray sequence)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.STWSEQ, channel,sequence);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<TWaveDirection> GetTWaveDirection(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWDIR, channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    Enum.TryParse(response, true, out TWaveDirection result);
			return result;
		}

	    public async Task<Unit> SetTWaveDirection(string channel, TWaveDirection direction)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWDIR, channel,direction.ToString());
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<Unit> SetTWaveCompressionCommand(CompressionTable compressionTable)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCTBL, compressionTable);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<string> GetTWaveCompressionCommand()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCTBL);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    return result;
		}

	    

	    public async Task<StateCommands> GetCompressorMode()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCMODE);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response= responseQueue.Dequeue();
		    Enum.TryParse(response, true, out StateCommands result);
			return result;
		}

	    public async Task<Unit> SetCompressorMode(StateCommands mode)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCMODE, mode.ToString());
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<int> GetCompressorOrder()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCORDER);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<Unit> SetCompressorOrder(int order)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCORDER, order);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetCompressorTriggerDelay()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCTD);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetCompressorTriggerDelay(double delayMilliseconds)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCTD, delayMilliseconds);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

		public async Task<double> GetCompressionTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCTC);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	   

	    public async Task<Unit> SetCompressionTime(int timeMilliseconds)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCTC, timeMilliseconds);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    

	    public async Task<double> GetNormalTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCTN);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetNormalTime(int timeMilliseconds)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCTN, timeMilliseconds);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    
	    public async Task<double> GetNonCompressTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCTNC);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetNonCompressTime(int timemilliSeconds)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCTNC,timemilliSeconds);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    return Unit.Default;
		}

	    public async Task<Unit> ForceMultipassTrigger()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TWCTRG);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
		}

	    

	    public async Task<SwitchState> GetSwitchState()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCSW);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    Enum.TryParse(response, true, out SwitchState result);
			return result;
		}

	    public async Task<Unit> SetSwitchState(SwitchState state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCSW);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}
	    public async Task<Unit> SetTWaveToCommonClockMode(bool setToMode)
	    {
		    throw new NotImplementedException();
	    }

	    public async Task<Unit> SetTWaveToCompressorMode(bool setToMode)
	    {
		    throw new NotImplementedException();
	    }



		//TWaveSweepModule

		public async Task<Unit> SetSweepStartFrequency(string channel, double frequency)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWSSTRT,channel,frequency);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetSweepStartFrequency(string channel)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GTWSSTRT, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetSweepStopFrequency(string channel, double stopFrequency)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWSSTP,channel,stopFrequency);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

		public async Task<double> GetSweepStopFrequency(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWSSTP,channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

		public async Task<Unit> SetSweepTime(string channel, double timeInSeconds)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWSTM,channel,timeInSeconds);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

		public async Task<double> GetSweepTime(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWSTM,channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
			var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

		public async Task<Unit> StartSweep(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWSGO,channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

		public async Task<Unit> StopSweep(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWSHLT,channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default; 
		}

		public async Task<string> GetSweepStatus(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWSTA,channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    return result;
		}

		//WiFi Module

	    public async Task<string> GetHostName()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GHOST);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    return result;
		}

	    public async Task<string> GetSSID()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GSSID);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    return result;
		}

	    public async Task<string> GetWiFiPassword()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GPSWD);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    return result;
		}

	    public async Task<Unit> SetHostName(string name)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SHOST,name);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetSSID(string id)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SSSID,id);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetWiFiPassword(string password)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SPSWD,password);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> EnablesInterface(bool enables)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWIFIENA,enables.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}
	    //IPulse GeneratorModule

	    public async Task<Unit> SetSignalTable(MipsSignalTable table)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLDAT,table);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetClock(ClockType clockType)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLCLK, clockType.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}
	    public async Task<Unit> SetClock(double clockValue)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.STBLCLK, clockValue);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

		public async Task<Unit> SetTrigger(SignalTableTrigger trigger)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLTRG, trigger.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> AbortTimeTable()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TBLABRT);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetMode(Modes mode)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SMOD, mode.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> StartTimeTable()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TBLSTRT);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> StopTable()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TBLSTOP);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<int> GetClockFreuency()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTBLFRQ);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<Unit> SetTableNumber(int number)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLNUM, number);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<int> GetTableNumber()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTBLNUM);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<Unit> SetAdvanceTableNumber(State state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLADV,state.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<State> GetAdvanceTableNumber()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTBLNUM);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    Enum.TryParse(result, out State value);
		    return value;
		}

	    public async Task<Unit> SetChannelValue(int count, string channel, int newValue)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLVLT,count,channel, newValue);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<int> GetChannelValue(int count, string channel)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.GTVLVLT, count, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;

		}

	    public async Task<Unit> SetChannelCount(int count, string channel, int newCount)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.STBLCNT, count, channel, newCount);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetTableDelay(int delay)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.STBLDLY, delay);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<Unit> SetSoftwareGeneration(Status value)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.SOFTLDAC, value.ToString());
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> EnablesRelpy(Status enables)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.STBLREPLY, enables.ToString());
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var result = responseQueue.Dequeue();
		    return Unit.Default;
		}

	    public async Task<Status> GetStatusReply()
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.GTBLREPLY);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    Enum.TryParse(result, out Status value);
		    return value;
		}


		//DELAY TRIGGER


		public async Task<Unit> SetTriggerChannelLevel(string channel, TriggerLevel trigger)
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.SDTRIGINP, channel, trigger.ToString());
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			return Unit.Default;
			
		}

		public async Task<Unit> SetTriggerDelay(double delayTime)
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.SDTRIGDLY, delayTime);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			return Unit.Default;
			throw new NotImplementedException();
		}

		public async Task<double> GetTriggerDelay()
		{
			//var mipsmessage = MipsMessage.Create(MipsCommand.GDTRIGDLY);
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes =>
			//{
			//	double.TryParse(bytes, out double result);
			//	return result;

			//}).FirstAsync();
			throw new NotImplementedException();
		}

		public async Task<Unit> SetTriggerPeriod(double delayPeriod)
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.SDTRIGPRD, delayPeriod);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			return Unit.Default;
			throw new NotImplementedException();
		}

		public async Task<double> GetTriggerPeriod()
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.GDTRIGPRD);
			 messageQueue.Enqueue(mipsmessage);
			await ProcessQueue(true);
			var response = responseQueue.Dequeue();
			double.TryParse(response, out double result);
			return result;
			throw new NotImplementedException();
		}

		public async Task<Unit> SetTriggerRepeatCount(int count)
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.SDTRIGRPT, count);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			return Unit.Default;
			throw new NotImplementedException();
		}

		public async Task<int> GetTriggerRepeatCount()
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.GDTRIGRPT);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue(true);
			var response = responseQueue.Dequeue();
			int.TryParse(response, out int result);
			return result;
			throw new NotImplementedException();
		}

		public async Task<Unit> SetTriggerModule(ArbMode mode)
		{
			//var mipsmessage = MipsMessage.Create(MipsCommand.SDTRIGMOD, mode.ToString());
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}

		public async Task<Unit> EnableDelayTrigger(Status enable)
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.SDTRIGENA, enable.ToString());
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue();
			return Unit.Default;
			throw new NotImplementedException();
		}

		public async Task<string> GetDelayTriggerStatus()
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.GDTRIGENA);
			messageQueue.Enqueue(mipsmessage);
			await ProcessQueue(true);
			var result = responseQueue.Dequeue();
			return result;
			throw new NotImplementedException();
		}

		//Ethernet Module

		public async Task<string> GetIP()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GEIP);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    return result;
			//throw new NotImplementedException();
		}

	    public async Task<Unit> SetIP(string ip)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SEIP,ip);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<int> GetPortNumber()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GEPORT);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
			//throw new NotImplementedException();
		}

	    public async Task<Unit> SetPortNumber(int port)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SEPORT,port);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
			// throw new NotImplementedException();
		}

	    public async Task<string> GetGatewayIP()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GEGATE);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    return result; ;
			//throw new NotImplementedException();
		}

	    public async Task<Unit> SetGatewayIP(string ip)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GEGATE,ip);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
			//throw new NotImplementedException();
		}
		//FAIMS Module

	    public async Task<Unit> SetPositiveOutput(int slope, int offset)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SRFHPCAL,slope,offset);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<Unit> SetNegativeOutput(int slope, int offset)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SRFHPCAL,slope,offset);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}


		//Filament Module


	    public async Task<State> GetFilamentEnable(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLENA,channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    Enum.TryParse(result, out State value);
		    return value;
		}

		public async Task<Unit> SetFilamentEnable(string channel, State state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLENA,channel,state.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

		public async Task<double> GetFilamentSetPointCurrent(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLI,channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

		public async Task<double> GetFilamentActualCurrent(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLAI, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetFilamentCurrent(string channel, int current)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLI, channel,current);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<double> GetFilamentSetPointVoltaget(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLSV, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<double> GetFilamentActualVoltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLASV, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetFilamentSetVoltage(string channel, int volts)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLSV, channel,volts);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetFilamentLoadVoltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLV, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<double> GetFilamentPower(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLPWR,channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<double> GetFilamentRampRate(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLRT, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetFilamentRampRate(string channel, int ramp)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLRT,channel,ramp);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetCyclingCurrent1(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLP1, channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetCyclingCurrent1(string channel, int current)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLP1,channel,current);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<double> GetCyclingCurrent2(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLP2,channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetCyclingCurrent2(string channel, int current)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLP2,channel,current);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<int> GetCycle(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLCY,channel);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<Unit> SetCycle(string channel, int cycle)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLCY,channel,cycle);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<State> GetCycleStatus(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLENAR,channel);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    Enum.TryParse(result, out State value);
		    return value;
		}

	    public async Task<Unit> SetCycleStatus(string channel, State state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLENAR,channel,state.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<double> GetCurrentToHost(double resistor)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.RFLPARMS, resistor);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetResistor(string channel, int time)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLSRES,channel,time);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		  return Unit.Default;
		}

	    public async Task<double> GetResistor()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GFLSRES);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result; 
	    }

	    public async Task<double> GetEmissionCurrent()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GFLECUR);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
	    }

	    //ARB Module

		public async Task<Unit> SetArbMode(string module, ArbMode mode)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBMODE, module, mode.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
			//throw new NotImplementedException();
		}

	    public async Task<ArbMode> GetArbMode(string module)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.GARBMODE, module);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    Enum.TryParse(result, out ArbMode value);
		    return value;
		}

	    public async Task<Unit> SetArbFrequency(string module, double frequencyInHz)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFREQ,module,frequencyInHz);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<int> GetArbFrequency(string module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFREQ, module);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<Unit> SetArbVoltsPeakToPeak(string module,double peakToPeakVolts)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFVRNG, module, peakToPeakVolts);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<int> GetArbVoltsPeakToPeak(string module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFVRNG, module);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<Unit> SetArbOffsetVoltage(string module, double value)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SWFVOFF, module, value);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<int> GetArbOffsetVoltage(string module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFVOFF, module);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

		public async Task<Unit> SetAuxOutputVoltage(string module, double value)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFVAUX, module, value);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		   return Unit.Default;
		}

	    public async Task<int> GetAuxOutputVoltage(string module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFVAUX, module);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<Unit> StopArb(string module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFDIS, module);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> StartArb(string module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFENA, module);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetTwaveDirection(string module, TWaveDirection direction)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFDIR, module, direction.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<TWaveDirection> GetTwaveDirection(string module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFDIR, module);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    Enum.TryParse(result, out TWaveDirection value);
		    return value;
		}

	    public async Task<Unit> SetWaveform(string module, IEnumerable<int> points)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFARB, module, points);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<IEnumerable<int>> GetWaveform(string module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFARB, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => bytes)
				.Scan(new List<int>(),
					(list, bytes) =>
					{
						var value = bytes.Split(',');
						if (value.Length == 32)
						{
							foreach (var v in value)
							{
								int.TryParse(v, out int result);
								list.Add(result);
							}

						}

						return list;
					}).FirstAsync();

		}

		public async Task<Unit> SetWaveformType(string module, ArbWaveForms waveForms)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFTYP, module, waveForms.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<ArbWaveForms> GetWaveformType(string module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFTYP, module);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    Enum.TryParse(result, out ArbWaveForms value);
		    return value;
		}

	    public async Task<Unit> SetBufferLength(string module, int length)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBBUF, module, length);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<int> GetBufferLength(string module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBBUF, module);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<Unit> SetBufferRepeat(string module, int count)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBNUM, module, count);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<int> GetBufferRepeat(string module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBNUM, module);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<Unit> SetAllChannelValue(string module, int value)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCHS, module,value);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetChannelValue(string module, string channel, int value)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCH, module, channel,value);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetChannelRange(string module, string channel, int start, int stop, int range)
	    {

		    var mipsmessage = MipsMessage.Create(MipsCommand.SACHRNG, module, channel,start,stop, range);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}


		//ARB CompressionCommand Module


		public async  Task<Unit> SetArbCompressionCommand(CompressionTable table)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCTBL,table);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
			//throw new NotImplementedException();
		}

	    public async Task<string> GetArbCompressionCommand()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCTBL);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    return result;
		}

	    public async Task<StateCommands> GetArbCompressorMode()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCMODE);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    Enum.TryParse(result, out StateCommands value);
		    return value;
		}

	    public async Task<Unit> SetArbCompressorMode(StateCommands mode)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCMODE, mode.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<int> GetArbCompressorOrder()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCORDER);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    int.TryParse(response, out int result);
		    return result;
		}

	    public async Task<Unit> SetArbCompressorOrder(int order)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCORDER, order);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetArbTriggerDelay()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCTD);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetArbTriggerDelay(double delay)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCTD, delay);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetArbCompressionTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCTC);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetArbCompressionTime(double time)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCTC, time);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetArbNormalTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCTN);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

		public async Task<Unit> SetArbNormalTime(double time)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCTN,time);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<double> GetArbNonCompressionTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCTNC);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var response = responseQueue.Dequeue();
		    double.TryParse(response, out double result);
		    return result;
		}

	    public async Task<Unit> SetArbNonCompressionTime(double time)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCTNC, time);
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetArbTrigger()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TARBTRG);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<SwitchState> GetArbSwitchState()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCSW);
		    messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue(true);
		    var result = responseQueue.Dequeue();
		    Enum.TryParse(result, out SwitchState value);
		    return value;
		}
		
	    public async Task<Unit> SetArbSwitchState(SwitchState state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCSW, state.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}


	    public async Task<Unit> SetArbClock(Status status)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCCLK, status.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}

	    public async Task<Unit> SetArbCompressor(Status status)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCMP, status.ToString());
			messageQueue.Enqueue(mipsmessage);
		    await ProcessQueue();
		    return Unit.Default;
		}
    }
}