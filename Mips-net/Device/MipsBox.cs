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

	    private ConcurrentQueue<MipsMessage> messageQueue = new ConcurrentQueue<MipsMessage>();
	    private ConcurrentQueue<string> responseQueue = new ConcurrentQueue<string>();
	    private object sync = new object();
		private SemaphoreSlim semaphore = new SemaphoreSlim(1);

		public MipsBox(MipsCommunicator communicator)
		{
			this.communicator = communicator?? throw new ArgumentNullException(nameof(communicator));
			this.communicator.Open();
			this.communicator.MessageSources.Subscribe(s =>
			{
				responseQueue.Enqueue(s);
			});

			ClockFrequency = 16000000;
		}
	    [DataMember]
	    public int ClockFrequency { get; set; }
	    [DataMember]
	    public string Name => GetName().Result;
	    public async Task<MipsBoxDeviceData> GetConfig()
	    {
		    var dcBiasChannels = await this.GetNumberDcBiasChannels();
		    var rfChannels = await this.GetNumberRfChannels();
		    var digitalChannels = await this.GetNumberDigitalChannels();
		    var twaveChannels = await this.GetNumberTwaveChannels();
		    var arbChannels = await this.GetNumberARBChannels();

			this.deviceData = new Lazy<MipsBoxDeviceData>(() => new MipsBoxDeviceData((uint)dcBiasChannels,
								(uint)rfChannels,  (uint) digitalChannels, (uint)twaveChannels,(uint)arbChannels));
			if (this.communicator.IsOpen)
			{
				return this.deviceData.Value;
			}

			return MipsBoxDeviceData.Empty;
	    }

	    private async Task RunQueue()
	    {
		    await semaphore.WaitAsync();
		    while (!messageQueue.IsEmpty)
		    {
			    if (messageQueue.TryDequeue(out var message))
			    {
				    message.WriteTo(communicator);
			    }
			    Thread.Sleep(50);
			    string response = string.Empty;
			    while (!responseQueue.TryPeek(out response))
			    {
				    Thread.Sleep(50);
			    }

		    }
		    semaphore.Release();
	    }
	    public async Task<int> GetNumberARBChannels()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.Arb.ToString());
		    messageQueue.Enqueue(mipsmessage);
		    await RunQueue();
		    await semaphore.WaitAsync();
		    responseQueue.TryDequeue(out string result);
			int.TryParse(result, out int channels);
		    semaphore.Release();
		    return channels;
		}
		 public async Task<string> GetName()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GNAME);
			messageQueue.Enqueue(mipsmessage);
		    await RunQueue();
		    await semaphore.WaitAsync();
		    responseQueue.TryDequeue(out string result);
		    semaphore.Release();
			return result;
	    }
		public async Task<int> GetNumberTwaveChannels()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.TWAVE.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int channels);
			    return channels;
		    }).FirstAsync();
	    }

		public async Task<int> GetNumberDigitalChannels()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.TWAVE.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int channels);
			    return channels;
		    }).FirstAsync();
		}

	    public  async Task<int> GetNumberRfChannels()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.RF.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int channels);
			    return channels;
		    }).FirstAsync();
		}

	    public async Task<int> GetNumberDcBiasChannels()
		{
				var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN, Modules.DCB.ToString());
			mipsmessage.WriteTo(communicator);
				var messagePacket = communicator.MessageSources;

				return await messagePacket.Select(bytes =>
				{
					int.TryParse(bytes, out int channels);
					return channels;
				}).FirstAsync();

		}
	

	   
	    public async Task<Unit> SetName(string name)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SNAME, name);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>Unit.Default).FirstAsync();
	    }


		public async Task<string> GetVersion()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GVER);
		    mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => bytes).FirstAsync();
		}

		public async Task<ErrorCode> GetError()
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.GERR);
			mipsmessage.WriteImpl(communicator);
			var messagePacket = communicator.MessageSources;
			return await messagePacket.Select(bytes =>
			{
				Enum.TryParse(bytes, true, out ErrorCode result);
				return result;
			}).FirstAsync();
		}
	    public async Task<string> About()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.ABOUT);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => bytes).FirstAsync();
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
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }
	    public async Task<int> GetChannel(Modules module)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GCHAN,module.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int channels);
			    return channels;
		    }).FirstAsync();
		}
		public async Task<Unit> Mute(State mute)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.MUTE,mute.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}
	    public async Task<Unit> Echo(Status echo)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.ECHO,echo.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }
	    public async Task<Unit> TriggerOut(TriggerValue trigger)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.TRIGOUT, trigger.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }
	    public async Task<Unit> TriggerOut(int trigger)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.TRIGOUT, trigger);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }
		public async Task<Unit> Delay(double delay)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.DELAY, delay);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }
	    public async Task<IEnumerable<string>> GetCommand()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GCMDS);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;
		   // return await messagePacket.Select(bytes => bytes).FirstAsync();
			var aggregator = await messagePacket.Select(s => s)
			 .Scan(new List<string>(),
			 (list, s) =>
			 {
				 list.Add(s);
				 return list;
			 }).Where(list=>list.Count==225).FirstAsync() ; 

			return aggregator;
		}

	    public async Task<Status> GetAnalogInputStatus()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GAENA);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    Enum.TryParse(bytes, true, out Status result);
			    return result;
		    }).FirstAsync();
		}
	    public async Task<Unit> SetAnalogInputStatus(Status status)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SAENA,status.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>Unit.Default).FirstAsync();
	    }
	    public async Task<IEnumerable<string>> Threads()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.THREADS);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

			var aggregator = await messagePacket.Select(s => s).Scan(new List<string>(),
				    (list, s) =>
				    {
					  list.Add(s);
					  return list;
				    }).Where(list=>list.Count==7).FirstAsync(); 
		    return aggregator;
		}
	    public async Task<Unit> SetThreadControl(string threadName, string threadValue)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.STHRDENA, threadName.ToString(),threadValue.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }
	    public async Task<Unit> SetADCAddress(int board, double address)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.SDEVADD, board, address);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
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
				    mipsmessage.WriteTo(communicator);
				    var messagePacket = communicator.MessageSources;

				    return await messagePacket.Select(bytes => bytes).FirstAsync();
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
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();

		}
	    public async Task<int> ReadADCChannel(int channel)
		{
			switch (channel)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					var ampsmessage = MipsMessage.Create(MipsCommand.ADC, channel);
					ampsmessage.WriteTo(communicator);
					var messagePacket = communicator.MessageSources;

					return await messagePacket.Select(bytes =>
						{ int.TryParse(bytes, out int count);
						  return count;
						}).FirstAsync();
			default:
					return 0;
			}
		}
	    public async Task<Unit> LEDOverride(bool LEDValue)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.LEDOVRD, LEDValue.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }
	    public async Task<Unit> LEDColor(int color)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.LED, color);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }
	    public async Task<Unit> DisplayOff(Status status)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.DSPOFF, status.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }

		//Clock Generation

	    public async Task<int> GetClockPulseWidth()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GWIDTH);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>{int.TryParse(bytes, out int result );
			    return result;
		    }).FirstAsync();
	    }
	    public async Task<Unit> SetClockPulseWidth(int microseconds)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.SWIDTH, microseconds);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }

	    public async Task<int> GetClockFrequency()
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.GFREQ);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => {
				int.TryParse(bytes, out int result);
				return result;
			}).FirstAsync();
		}

	    public async Task<Unit> SetClockFrequency(int frequencyInHz)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.SFREQ,frequencyInHz);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }

	    public async Task<Unit> ConfigureClockBurst(int numberCycles)
		{
		    var mipsmessage = MipsMessage.Create(MipsCommand.BURST,numberCycles);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }

		//DC Bias Module

	    public async Task<Unit> SetDcVoltage(string channel, double volts)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCB,channel,volts);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetDcSetpoint(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCB, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<double> GetDcReadback(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCBV, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetDcOffset(string channel, double offsetVolts)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCBOF, channel, offsetVolts);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetDcOffset(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCBOF, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<double> GetMinimumVoltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCMIN, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<double> GetMaximumVoltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCMAX, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetDcPowerState(State state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCPWR, state.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<State> GetDcPowerState()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCPWR);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    Enum.TryParse(bytes, out State result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetAllDcChannels(IEnumerable<double> channels)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCBALL, channels);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

		public async Task<IEnumerable<double>> GetAllDcSetpoints()
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCBALL);
			mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes =>bytes)
				.Scan(new List<double>(),
				(list, bytes) =>
				{
					var value = bytes.Split(',');
					if (value.Length == 8)
					{
						foreach (var v in value)
						{
							double.TryParse(v, out double result);
							list.Add(result);
						}
						
					}
					
					return list;
				}).FirstAsync();
			
			
		}

		public async Task<IEnumerable<double>> GetAllDcReadbacks()
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCBALLV);
			mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

			var aggregator = await messagePacket.Select(bytes => bytes)
				.Scan(new List<double>(),
				(list, bytes) =>
				{
					var value = bytes.Split(',');
					if (value.Length == 8)
					{
						foreach (var v in value)
						{
							double.TryParse(v, out double result);
							list.Add(result);
						}

					}

					return list;
				}).FirstAsync();
			return aggregator;
		}

		public async Task<Unit> SetUniversalOffset(double voltageOffset)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCBDELTA, voltageOffset);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetNumberOfChannelsOnboard(int board, int numberChannels)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SDCBCHNS, board, numberChannels);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> UseSingleOffsetTwoModules(Status status)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SDCBONEOFF, status.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> EnableOffsetReadback(Status enableReadback)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.DCBOFFRBENA, enableReadback.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> EnableBoardOffsetOption(int board, Status enable)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCBOFFENA, board, enable.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

		

		//DC BIAS PROFILE

	    public async Task<Unit> SetDCbiasProfile(int profile, IEnumerable<int> channels)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDCBPRO, profile, channels);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<IEnumerable<double>> GetDCbiasProfile(int profile)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDCBPRO, profile);
			mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => bytes)
			    .Scan(new List<double>(),
				    (list, bytes) =>
				    {
					    var value = bytes.Split(',');
					    if (value.Length == 8)
					    {
						    foreach (var v in value)
						    {
							    double.TryParse(v, out double result);
							    list.Add(result);
						    }

					    }

					    return list;
				    }).FirstAsync();
		}

	    public  async Task<Unit> OutputWithDCbiasProfile(int profile)
		{
			var mipsmessage = MipsMessage.Create(MipsCommand.ADCBPRO, profile);
			mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			//throw new NotImplementedException();
			
		}

	    public async Task<Unit> CopiesToDCbiasProfile(int profile)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.CDCBPRO, profile);
			mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}

	    public async Task<Unit> ToggleProfile(int profile1, int profile2, int time)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TDCBPRO, profile1, profile2, time);
			mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}

	    public async Task<Unit> StopToggleProfile()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TDCBSTOP);
			mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}

	    //Rf Driver

		public async Task<Unit> SetFrequency(string channel, int frequencyInHz)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SRFFRQ,channel,frequencyInHz);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetLevel(string channel, int peakToPeakVoltage)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SRFVLT,channel,peakToPeakVoltage);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetDriveLevel(string channel, int drive)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SRFDRV,channel,drive);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetFrequency(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFFRQ, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<double> GetPositiveComponent(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFPPVP, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<double> GetNegativeComponent(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFPPVN, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			   double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<double> GetOutputDriveLevelPercent(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFDRV, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<double> GetPeakToPeakVoltageSetpoint(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFVLT, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<int> GetChannelPower(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFPWR, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
		}

		public async Task<IEnumerable<double>> GetParameters()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GRFALL);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => bytes)
				.Scan(new List<double>(),
					(list, bytes) =>
					{
						var value = bytes.Split(',');
						if (value.Length == 8)
						{
							foreach (var v in value)
							{
								double.TryParse(v, out double result);
								list.Add(result);
							}

						}

						return list;
					}).FirstAsync();
		}

		//DioModule

		public async Task<Unit> SetDigitalOutput(string channel, int state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SDIO, channel, state);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetDigitalState(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GDIO, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
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
					mipsmessage.WriteTo(communicator);
					var messagePacket = communicator.MessageSources;

					return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
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
							mipsmessage.WriteTo(communicator);
							var messagePacket = communicator.MessageSources;

							return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
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
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    
	    public async Task<int> GetEsiSetpointVoltage(int channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GHV, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
				return result;
		    }).FirstAsync();
		}

	    public async Task<int> GetEsiReadbackVoltage(int channel)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GHVV, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
				return result;
		    }).FirstAsync();
		}

	    public async Task<int> GetEsiReadbackCurrent(int channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GHVI, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
				return result;
		    }).FirstAsync();
		}

	    public async Task<int> GetMaximumEsiVoltage(int channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GHVMAX, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
				return result;
		    }).FirstAsync();
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
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> PlayMacro(string name)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.MPLAY,name);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<string> ListMacro()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.MLIST);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => bytes).FirstAsync();
		}

	    public async Task<Unit> DeleteMacro(string name)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.MDELETE,name);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

		//TWAVE Module

	    public async Task<double> GetTWaveFrequency(string channel)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GTWF, channel);
			messageQueue.Enqueue(mipsmessage);
		    await RunQueue();
		    await semaphore.WaitAsync();
		    responseQueue.TryDequeue(out string result);
		    double.TryParse(result, out double freq);
			semaphore.Release();
		    return freq;
			
				
		}

	    public async Task<Unit> SetTWaveFrequency(string channel, double frequency)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWF, channel,frequency);
			messageQueue.Enqueue(mipsmessage);
		    await RunQueue();
		    await semaphore.WaitAsync();
		    responseQueue.TryDequeue(out string result);
		    semaphore.Release();
			return Unit.Default;

		}

	    public async Task<double> GetTWavePulseVoltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWPV, channel);
			messageQueue.Enqueue(mipsmessage);
		    await RunQueue();
		    await semaphore.WaitAsync();
		    responseQueue.TryDequeue(out string result);
		    double.TryParse(result, out double pulseVoltage);
		    semaphore.Release();
		    return pulseVoltage;
		}

	    public async Task<Unit> SetTWavePulseVoltage(string channel,double voltage)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWPV, channel,voltage);
			messageQueue.Enqueue(mipsmessage);
		    await RunQueue();
		    await semaphore.WaitAsync();
		    responseQueue.TryDequeue(out string result);
		    semaphore.Release();
		    return Unit.Default;
		}

	    public async Task<Unit> SetTWaveGuard1Voltage(string channel,double voltage)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWG1V, channel,voltage);
			messageQueue.Enqueue(mipsmessage);
		    await RunQueue();
		    await semaphore.WaitAsync();
		    responseQueue.TryDequeue(out string result);
		    semaphore.Release();
		    return Unit.Default;
		}

	    public async Task<double> GetTWaveGuard1Voltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWG1V, channel);
			messageQueue.Enqueue(mipsmessage);
		    await RunQueue();
		    await semaphore.WaitAsync();
		    responseQueue.TryDequeue(out string result);
		    double.TryParse(result, out double voltage);
		    semaphore.Release();
		    return voltage;
		}

	    public async Task<Unit> SetTWaveGuard2Voltage(string channel,double voltage)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWG2V, channel,voltage);
			messageQueue.Enqueue(mipsmessage);
		    await RunQueue();
		    await semaphore.WaitAsync();
		    responseQueue.TryDequeue(out string result);
		    semaphore.Release();
		    return Unit.Default;
		}

	    public async Task<double> GetTWaveGuard2Voltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWG2V, channel);
			messageQueue.Enqueue(mipsmessage);
		    await RunQueue();
		    await semaphore.WaitAsync();
		    responseQueue.TryDequeue(out string result);
		    double.TryParse(result, out double voltage);
		    semaphore.Release();
		    return voltage;
		}

	   public async Task<BitArray> GetTWaveSequence(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWSEQ, channel);
			mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;
			var bitPattern = await messagePacket.Select(s =>{
				var boolArray = new List<bool>();
				foreach (var v in s.ToCharArray())
				{
					boolArray.Add(v == '0' ? false : true);

				}
				
				return boolArray;
			}).FirstAsync();
			var sequence = new BitArray(bitPattern.ToArray());
			return sequence;
		    //throw new NotImplementedException();
	    }

	    public async Task<Unit> SetTWaveSequence(string channel, BitArray sequence)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.STWSEQ, channel,sequence);
			messageQueue.Enqueue(mipsmessage);
		    await RunQueue();
		    await semaphore.WaitAsync();
		    responseQueue.TryDequeue(out string result);
		    semaphore.Release();
		    return Unit.Default;
		}

	    public async Task<TWaveDirection> GetTWaveDirection(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWDIR, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    Enum.TryParse(bytes,true, out TWaveDirection dir);
			    return dir;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetTWaveDirection(string channel, TWaveDirection direction)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWDIR, channel,direction.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetTWaveCompressionCommand(CompressionTable compressionTable)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCTBL, compressionTable);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<string> GetTWaveCompressionCommand()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCTBL);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>bytes).FirstAsync();
		}

	    

	    public async Task<StateCommands> GetCompressorMode()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCMODE);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    Enum.TryParse(bytes,true, out StateCommands mod);
			    return mod;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetCompressorMode(StateCommands mode)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCMODE, mode.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetCompressorOrder()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCORDER);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int freq);
			    return freq;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetCompressorOrder(int order)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCORDER, order);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetCompressorTriggerDelay()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCTD);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double freq);
			    return freq;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetCompressorTriggerDelay(double delayMilliseconds)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCTD, delayMilliseconds);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

		public async Task<double> GetCompressionTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCTC);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double freq);
			    return freq;
		    }).FirstAsync();
		}

	   

	    public async Task<Unit> SetCompressionTime(int timeMilliseconds)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCTC, timeMilliseconds);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    

	    public async Task<double> GetNormalTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCTN);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double freq);
			    return freq;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetNormalTime(int timeMilliseconds)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCTN, timeMilliseconds);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    
	    public async Task<double> GetNonCompressTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCTNC);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double freq);
			    return freq;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetNonCompressTime(int timemilliSeconds)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCTNC,timemilliSeconds);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> ForceMultipassTrigger()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TWCTRG);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    

	    public async Task<SwitchState> GetSwitchState()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWCSW);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    Enum.TryParse(bytes,true, out SwitchState state);
			    return state;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetSwitchState(SwitchState state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWCSW);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}
	    public async Task<Unit> SetTWaveToCommonClockMode(bool setToMode)
	    {
		    throw new NotImplementedException();
	    }

	    public async Task<Unit> SetTWaveToCompressorMode(bool setToMode)
	    {
		    throw new NotImplementedException();
	    }



		//IFrequencySweepModule

		public async Task<Unit> SetStartFrequency(string channel, int frequency)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWSSTRT,channel,frequency);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

		public async Task<int> GetStartFrequency(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWSSTRT,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {

				int.TryParse(bytes, NumberStyles.Number,
				    CultureInfo.CurrentCulture.NumberFormat,
				    out int result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetStopFrequency(string channel, int stopFrequency)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWSSTP,channel,stopFrequency);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

		public async Task<int> GetStopFrequency(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWSSTP,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {

				int.TryParse(bytes, NumberStyles.Number,
				    CultureInfo.CurrentCulture.NumberFormat,
				    out int result);
			    return result;
		    }).FirstAsync();
		}

		public async Task<Unit> SetSweepTime(string channel, double timeInSeconds)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWSTM,channel,timeInSeconds);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

		public async Task<double> GetSweepTime(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWSTM,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double state);
			    return state;
		    }).FirstAsync();
		}

		public async Task<Unit> StartSweep(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWSGO,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

		public async Task<Unit> StopSweep(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STWSHLT,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

		public async Task<string> GetStatus(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTWSTA,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>bytes).FirstAsync();
		}

		//WiFi Module

	    public async Task<string> GetHostName()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GHOST);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => bytes).FirstAsync();
		}

	    public async Task<string> GetSSID()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GSSID);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => bytes).FirstAsync();
		}

	    public async Task<string> GetWiFiPassword()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GPSWD);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => bytes).FirstAsync();
		}

	    public async Task<Unit> SetHostName(string name)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SHOST,name);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetSSID(string id)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SSSID,id);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetWiFiPassword(string password)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SPSWD,password);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> EnablesInterface(bool enables)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWIFIENA,enables.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}
	    //IPulse GeneratorModule

	    public async Task<Unit> SetSignalTable(MipsSignalTable table)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLDAT,table);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetClock(ClockType clockType)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLCLK, clockType.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}
	    public async Task<Unit> SetClock(double clockValue)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.STBLCLK, clockValue);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
	    }

		public async Task<Unit> SetTrigger(SignalTableTrigger trigger)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLTRG, trigger.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> AbortTimeTable()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TBLABRT);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetMode(Modes mode)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SMOD, mode.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> StartTimeTable()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TBLSTRT);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> StopTable()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TBLSTOP);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetClockFreuency()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTBLFRQ);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetTableNumber(int number)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLNUM, number);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetTableNumber()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTBLNUM);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes =>
			{
				int.TryParse(bytes, out int result);
				return result;
			}).FirstAsync();
		}

	    public async Task<Unit> SetAdvanceTableNumber(State state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLADV,state.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<State> GetAdvanceTableNumber()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GTBLNUM);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			   Enum.TryParse(bytes, out State result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetChannelValue(int count, string channel, int newValue)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.STBLVLT,count,channel, newValue);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetChannelValue(int count, string channel)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.GTVLVLT, count, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;
		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();

		}

	    public async Task<Unit> SetChannelCount(int count, string channel, int newCount)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.STBLCNT, count, channel, newCount);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetTableDelay(int delay)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.STBLDLY, delay);
			mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}

	    public async Task<Unit> SetSoftwareGeneration(Status value)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.SOFTLDAC, value.ToString());
			mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}

	    public async Task<Unit> EnablesRelpy(Status enables)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.STBLREPLY, enables.ToString());
			mipsmessage.WriteTo(communicator);
			var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}

	    public async Task<Status> GetStatusReply()
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.GTBLREPLY);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes =>
			{
				Enum.TryParse(bytes, out Status result);
				return result;
			}).FirstAsync();
		}


		//DELAY TRIGGER


		public async Task<Unit> SetTriggerChannelLevel(string channel, TriggerLevel trigger)
		{
			//var mipsmessage = MipsMessage.Create(MipsCommand.SDTRIGINP, channel, trigger.ToString());
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			 throw new NotImplementedException();
		}

		public async Task<Unit> SetTriggerDelay(double delayTime)
		{
			//var mipsmessage = MipsMessage.Create(MipsCommand.SDTRIGDLY, delayTime);
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
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
			//var mipsmessage = MipsMessage.Create(MipsCommand.SDTRIGPRD, delayPeriod);
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}

		public async Task<double> GetTriggerPeriod()
		{
			//var mipsmessage = MipsMessage.Create(MipsCommand.GDTRIGPRD);
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes =>
			//{
			//	double.TryParse(bytes, out double result);
			//	return result;

			//}).FirstAsync();
			throw new NotImplementedException();
		}

		public async Task<Unit> SetTriggerRepeatCount(int count)
		{
			//var mipsmessage = MipsMessage.Create(MipsCommand.SDTRIGRPT, count);
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}

		public async Task<int> GetTriggerRepeatCount()
		{
			//var mipsmessage = MipsMessage.Create(MipsCommand.GDTRIGRPT);
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes =>
			//{
			//	int.TryParse(bytes, out int result);
			//	return result;

			//}).FirstAsync();
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
			//var mipsmessage = MipsMessage.Create(MipsCommand.SDTRIGENA, enable.ToString());
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
			throw new NotImplementedException();
		}

		public async Task<string> GetDelayTriggerStatus()
		{
			//var mipsmessage = MipsMessage.Create(MipsCommand.GDTRIGENA);
			//mipsmessage.WriteTo(communicator);
			//var messagePacket = communicator.MessageSources;

			//return await messagePacket.Select(bytes => bytes).FirstAsync();
			throw new NotImplementedException();
		}

		//Ethernet Module

		public async Task<string> GetIP()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GEIP);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>bytes).FirstAsync();
			//throw new NotImplementedException();
		}

	    public async Task<Unit> SetIP(string ip)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SEIP,ip);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetPortNumber()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GEPORT);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => { int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
			//throw new NotImplementedException();
		}

	    public async Task<Unit> SetPortNumber(int port)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SEPORT,port);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
				// throw new NotImplementedException();
	    }

	    public async Task<string> GetGatewayIP()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GEGATE);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => bytes).FirstAsync();
			//throw new NotImplementedException();
		}

	    public async Task<Unit> SetGatewayIP(string ip)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GEGATE,ip);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>Unit.Default).FirstAsync();
			//throw new NotImplementedException();
		}
		//FAIMS Module

	    public async Task<Unit> SetPositiveOutput(int slope, int offset)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SRFHPCAL,slope,offset);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetNegativeOutput(int slope, int offset)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SRFHPCAL,slope,offset);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}


		//Filament Module


	    public async Task<State> GetFilamentEnable(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLENA,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    Enum.TryParse(bytes, out State result);
			    return result;
		    }).FirstAsync();
		}

		public async Task<Unit> SetFilamentEnable(string channel, State state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLENA,channel,state.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

		public async Task<double> GetFilamentSetPointCurrent(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLI,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

		public async Task<double> GetFilamentActualCurrent(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLAI, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => {
				double.TryParse(bytes, out double result);
				return result;
			}).FirstAsync();
		}

	    public async Task<Unit> SetFilamentCurrent(string channel, int current)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLI, channel,current);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetFilamentSetPointVoltaget(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLSV, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<double> GetFilamentActualVoltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLASV, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => {
				double.TryParse(bytes, out double result);
				return result;
			}).FirstAsync();
		}

	    public async Task<Unit> SetFilamentSetVoltage(string channel, int volts)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLSV, channel,volts);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync(); 
	    }

	    public async Task<double> GetFilamentLoadVoltage(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLV, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<double> GetFilamentPower(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLPWR,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => {
				double.TryParse(bytes, out double result);
				return result;
			}).FirstAsync();
		}

	    public async Task<double> GetFilamentRampRate(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLRT, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

			return await messagePacket.Select(bytes => {
				double.TryParse(bytes, out double result);
				return result;
			}).FirstAsync();
		}

	    public async Task<Unit> SetFilamentRampRate(string channel, int ramp)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLRT,channel,ramp);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetCyclingCurrent1(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLP1, channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetCyclingCurrent1(string channel, int current)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLP1,channel,current);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetCyclingCurrent2(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLP2,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetCyclingCurrent2(string channel, int current)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLP2,channel,current);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetCycle(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLCY,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetCycle(string channel, int cycle)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLCY,channel,cycle);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<State> GetCycleStatus(string channel)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLENAR,channel);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => {
			    Enum.TryParse(bytes, out State result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetCycleStatus(string channel, State state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLENAR,channel,state.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetCurrentToHost(double resistor)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.RFLPARMS, resistor);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetResistor(string channel, int time)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SFLSRES,channel,time);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetResistor()
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.GFLSRES);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync(); throw new NotImplementedException();
	    }

	    public async Task<double> GetEmissionCurrent()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GFLECUR);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

		//ARB Module

	    public async Task<string> SetArbMode(int module, ArbMode mode)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBMODE, module, mode.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => bytes).FirstAsync();
			//throw new NotImplementedException();
		}

	    public async Task<ArbMode> GetArbMode(int module)
	    {

			var mipsmessage = MipsMessage.Create(MipsCommand.GARBMODE, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
			{ Enum.TryParse(bytes, true, out ArbMode result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetArbFrequency(int module, int frequencyInHz)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFREQ,module,frequencyInHz);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetArbFrequency(int module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFREQ, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			   int.TryParse(bytes,  out int result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetArbVoltsPeakToPeak(int module, int peakToPeakVolts)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFVRNG, module, peakToPeakVolts);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetArbVoltsPeakToPeak(int module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFVRNG, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetArbOffsetVoltage(int module, int value)
	    {
		    var mipsmessage = MipsMessage.Create(MipsCommand.SWFVOFF, module, value);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetArbOffsetVoltage(int module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFVOFF, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
		}

		public async Task<Unit> SetAuxOutputVoltage(int module, int value)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFVAUX, module, value);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetAuxOutputVoltage(int module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFVAUX, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> StopArb(int module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFDIS, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> StartArb(int module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFENA, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetTwaveDirection(int module, TWaveDirection direction)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFDIR, module, direction.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<TWaveDirection> GetTwaveDirection(int module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFDIR, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			   Enum.TryParse(bytes,true, out TWaveDirection result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetWaveform(int module, IEnumerable<int> points)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFARB, module, points);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<IEnumerable<int>> GetWaveform(int module)
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

		public async Task<Unit> SetWaveformType(int module, ArbWaveForms waveForms)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SWFTYP, module, waveForms.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<ArbWaveForms> GetWaveformType(int module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GWFTYP, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    Enum.TryParse(bytes,true, out ArbWaveForms result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetBufferLength(int module, int length)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBBUF, module, length);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetBufferLength(int module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBBUF, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetBufferRepeat(int module, int count)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBNUM, module, count);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetBufferRepeat(int module)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBNUM, module);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetAllChannelValue(int module, int value)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCHS, module,value);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetChannelValue(int module, int channel, int value)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCH, module, channel,value);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetChannelRange(int module, int channel, int start, int stop, int range)
	    {

		    var mipsmessage = MipsMessage.Create(MipsCommand.SACHRNG, module, channel,start,stop, range);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}


		//ARB CompressionCommand Module


		public async  Task<Unit> SetArbCompressionCommand(CompressionTable table)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCTBL,table);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>Unit.Default).FirstAsync();
			//throw new NotImplementedException();
		}

	    public async Task<string> GetArbCompressionCommand()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCTBL);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>bytes).FirstAsync();
		}

	    public async Task<StateCommands> GetArbCompressorMode()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCMODE);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    Enum.TryParse(bytes,true, out StateCommands result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetArbCompressorMode(StateCommands mode)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCMODE, mode.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<int> GetArbCompressorOrder()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCORDER);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    int.TryParse(bytes, out int result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetArbCompressorOrder(int order)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCORDER, order);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetArbTriggerDelay()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCTD);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetArbTriggerDelay(double delay)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCTD, delay);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetArbCompressionTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCTC);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetArbCompressionTime(double time)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCTC, time);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetArbNormalTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCTN);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

		public async Task<Unit> SetArbNormalTime(double time)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCTN,time);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<double> GetArbNonCompressionTime()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCTNC);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    double.TryParse(bytes, out double result);
			    return result;
		    }).FirstAsync();
		}

	    public async Task<Unit> SetArbNonCompressionTime(double time)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCTNC, time);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetArbTrigger()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.TARBTRG);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<SwitchState> GetArbSwitchState()
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.GARBCSW);
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes =>
		    {
			    Enum.TryParse(bytes,true, out SwitchState result);
			    return result;
		    }).FirstAsync();
		}
		
	    public async Task<Unit> SetArbSwitchState(SwitchState state)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCSW, state.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}


	    public async Task<Unit> SetArbClock(Status status)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCCLK, status.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}

	    public async Task<Unit> SetArbCompressor(Status status)
	    {
			var mipsmessage = MipsMessage.Create(MipsCommand.SARBCMP, status.ToString());
		    mipsmessage.WriteTo(communicator);
		    var messagePacket = communicator.MessageSources;

		    return await messagePacket.Select(bytes => Unit.Default).FirstAsync();
		}
    }
}