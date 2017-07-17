using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Falkor.Controls.ViewModels.IO;
using FalkorSDK.IO.Ports;
using mips_control.Data;
using mips_control.ValidationControls;
using Mips_net.Commands;
using Mips_net.Device;
using ReactiveUI;
using RJCP.IO.Ports;
using Parity = RJCP.IO.Ports.Parity;
using StopBits = RJCP.IO.Ports.StopBits;


namespace mips_control.ViewModel
{
	[Export(typeof(TwaveCompressionCommandViewModel))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class TwaveCompressionCommandViewModel : ReactiveValidatedObject
	{
		public StateCommands stateCommand;
		public TWaveParameter twaveParameter;
		public CompressionCommandViewModel compressionCommand;
		public string selectedCommandValue;
		public string twaveParameterValue;
		private LoopData loop;
		private IObservable<bool> canExecuteEnd;
		private IObservable<bool> canExecuteStart;
		private static Queue<string> compressionCommandQueue;
		private IObservable<bool> canGenerate;
		private IMipsBox mipsBox;
		private FalkorSerialPort port;
		private bool squareWaveSelected;
		private bool arbWaveSelected;
		

		public TwaveCompressionCommandViewModel()
		{
		}

		[ImportingConstructor]
		public TwaveCompressionCommandViewModel(FalkorSerialPort serialPort)
		{
			this.Port=  serialPort ;
			Loop =LoopData.End;
			SquareWaveSelected = true;
			compressionCommandQueue = new Queue<string>();
			this.AddStateCommand = ReactiveCommand.CreateFromObservable(AddCommandToList);
			this.RemoveCommand = ReactiveCommand.Create(() => { });
			this.AddTWaveParameterCommand = ReactiveCommand.CreateFromObservable(AddTWaveParameterToList);
			this.StartLoop = ReactiveCommand.CreateFromObservable(AddStartLoopToList,CanExecuteStart);
			this.EndLoop = ReactiveCommand.CreateFromObservable(AddEndLoopToList,CanExecuteEnd);
			
			canGenerate=SelectedCommandViewModelList.CountChanged.Select(count => count > 0).StartWith(false);
			this.GenerateCompressionString =
				ReactiveCommand.CreateFromObservable(GenerateCompressionCommand,CanGenerate);
			
		}


		public IObservable<bool> CanGenerate
		{
			get=>canGenerate;
			set => this.RaiseAndSetIfChanged(ref this.canGenerate, value);

		}

		private IObservable<Unit> GenerateCompressionCommand()
		{
			return Observable.Start(() =>
			{
				foreach (var commandViewModel in SelectedCommandViewModelList)
				{
					if (commandViewModel.HasErrors)
					{
						compressionCommandQueue.Clear();
						return;
					}
					else
					{
						compressionCommandQueue.Enqueue(commandViewModel.TWaveCommand);
						compressionCommandQueue.Enqueue(commandViewModel.TWaveCommandValue.ToString());
					}
				}
				var compressionTable = CompressionTable.GetCompressionTable();
				compressionTable.CommandQueue = compressionCommandQueue;
				mipsBox = MipsFactory.CreateMipsBox(Port);
				if (SquareWaveSelected && Port.IsOpen)
				{
					mipsBox.SetTWaveCompressionCommand(compressionTable);
				}
				else if (ARBWaveSelected && Port.IsOpen)
				{
					mipsBox.SetArbCompressionCommand(compressionTable);
				}
				this.SelectedCommandViewModelList.Clear();

					
				
			}, RxApp.MainThreadScheduler);

			
		}

		private IObservable<Unit> AddCommandToList()
		{
			try
			{
				return Observable.Start(() =>
				{
					this.SelectedCommandViewModelList.Add(new CompressionCommandViewModel(this.SelectedState.ToString()));
					if (Loop == LoopData.Started) Loop = LoopData.Filled;
				}, RxApp.MainThreadScheduler);
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.WriteLine(e.StackTrace);
				throw new InvalidOperationException();
			}
			
		}
		private IObservable<Unit> RemoveCommandFromList()
		{
			try
			{
				return Observable.Start(() =>
				{
					this.SelectedCommandViewModelList.Remove(SelectedCompressionCommands);
					
				}, RxApp.MainThreadScheduler);
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.WriteLine(e.StackTrace);
				throw new InvalidOperationException();
			}

		}
		private IObservable<Unit> AddTWaveParameterToList()
		{
			try
			{
				return Observable.Start(() =>
				{
					this.SelectedCommandViewModelList.Add(new CompressionCommandViewModel(this.SelectedTWaveParameter.ToString()));
					if (Loop == LoopData.Started) Loop = LoopData.Filled;
					}, RxApp.MainThreadScheduler);
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.WriteLine(e.StackTrace);
				throw new InvalidOperationException();
			}

		}

		
		public IObservable<bool> CanExecuteEnd
		{
			get
			{
				canExecuteEnd = this.WhenAnyValue(x => x.Loop, x => x == LoopData.Filled);
				return canExecuteEnd;
			} 
			set { this.RaiseAndSetIfChanged(ref this.canExecuteEnd, value); }
		}
		public IObservable<bool> CanExecuteStart
		{
			get
			{
				canExecuteStart = this.WhenAnyValue(x => x.Loop, x => x==LoopData.End);
				return canExecuteStart;
			} 
			set { this.RaiseAndSetIfChanged(ref this.canExecuteStart, value); }
		}


		public IObservable<Unit> AddStartLoopToList()
		{
			try
			{
				return Observable.Start(() =>
				{
					this.SelectedCommandViewModelList.Add(new CompressionCommandViewModel("["));
					Loop=LoopData.Started;

				}, RxApp.MainThreadScheduler);
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.WriteLine(e.StackTrace);
				throw new InvalidOperationException();
			}
		}
		public IObservable<Unit> AddEndLoopToList()
		{
			try
			{
				return Observable.Start(() =>
				{
					this.SelectedCommandViewModelList.Add(new CompressionCommandViewModel("]"));
					this.Loop= LoopData.End;

				}, RxApp.MainThreadScheduler);
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.WriteLine(e.StackTrace);
				throw new InvalidOperationException();
			}
		}

		public CompressionCommandViewModel SelectedCompressionCommands
		{
			get { return compressionCommand; }
			set
			{this.RaiseAndSetIfChanged(ref this.compressionCommand, value);}
		}

		public StateCommands SelectedState
		{
			get { return stateCommand; }
			set { this.RaiseAndSetIfChanged(ref this.stateCommand, value); }
		}
		public TWaveParameter SelectedTWaveParameter
		{
			get { return twaveParameter; }
			set { this.RaiseAndSetIfChanged(ref this.twaveParameter, value); }
		}
		public string SelectedTWaveParameterValue
		{
			get=> twaveParameterValue; 
			set=>this.RaiseAndSetIfChanged(ref this.twaveParameterValue, value); 
		}
		
		
		public LoopData Loop
		{
			get =>loop; 
			set =>this.RaiseAndSetIfChanged(ref this.loop, value); 
		}

		
		public FalkorSerialPort Port
		{
			get => port;
			set =>this.RaiseAndSetIfChanged(ref this.port, value);
		}

		public bool SquareWaveSelected
		{
			get => squareWaveSelected;
			set=>this.RaiseAndSetIfChanged(ref this.squareWaveSelected, value);
		}
		public bool ARBWaveSelected
		{
			get => arbWaveSelected;
			set=>this.RaiseAndSetIfChanged(ref this.arbWaveSelected, value);
		}

		public ReactiveList<CompressionCommandViewModel> SelectedCommandViewModelList { get; } = new ReactiveList<CompressionCommandViewModel>();
		public ReactiveCommand<Unit, Unit> AddStateCommand { get; set; }
		public ReactiveCommand<Unit, Unit> RemoveCommand { get; set; }
		public ReactiveCommand<Unit, Unit> AddTWaveParameterCommand { get; set; }
		public ReactiveCommand<Unit, Unit> StartLoop { get; set; }
		public ReactiveCommand<Unit, Unit> EndLoop { get; set; }
		public ReactiveCommand<Unit, Unit> GenerateCompressionString { get; set; }

		

		
	}
}