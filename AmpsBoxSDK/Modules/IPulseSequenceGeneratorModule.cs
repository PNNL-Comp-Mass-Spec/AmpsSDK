using System;
using System.ComponentModel.Composition;
using System.Reactive;
using System.Threading.Tasks;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Data;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IPulseSequenceGeneratorModule
    {
        IObservable<Unit> AbortTimeTable();
        IObservable<Unit> LoadTimeTable(AmpsSignalTable table);
        IObservable<Unit> SetClock(ClockType clockType);
        IObservable<Unit> SetTrigger(StartTriggerTypes startTriggerType);
        IObservable<Unit> SetMode();
        IObservable<Unit> StopTable();
        IObservable<Unit> StartTimeTable();
        string LastTable { get; }

    }

    public class PulseSequenceGeneratorModule : IPulseSequenceGeneratorModule
    {
        private IAmpsBoxCommunicator communicator;
        private IStandardModule standardModule;

        [ImportingConstructor]
        public PulseSequenceGeneratorModule(IAmpsBoxCommunicator communicator, IStandardModule standardModule)
        {
            this.communicator = communicator;
            this.standardModule = standardModule;
        }
       /// <summary>
       /// 
       /// </summary>
        public IObservable<Unit> AbortTimeTable()
       {
           var command = provider.GetCommand(AmpsCommandType.TimeTableAbort);
           string formattedCommand = command.Value;
            this.communicator.Write(
                                  formattedCommand);
           if (this.communicator.IsError)
           {
               System.Diagnostics.Trace.WriteLine(this.standardModule.GetError());
           }
        }

        /// <summary>
        /// Starts execution of time table.
        /// </summary>
        /// <returns></returns>
        public IObservable<Unit> StartTimeTable()
        {
            var command = provider.GetCommand(AmpsCommandType.TimeTableStart);
            this.communicator.Write(command.Value);
            if (this.communicator.IsError)
            {
                System.Diagnostics.Trace.WriteLine(this.standardModule.GetError());
            }
        }

        public string LastTable { get; private set; }

        /// <summary>
        /// Sets the table mode for the amps / mips box.
        /// </summary>
        public IObservable<Unit> SetMode()
        {
            var command = provider.GetCommand(AmpsCommandType.Mode);
            if (this.communicator.IsError)
            {
                System.Diagnostics.Trace.WriteLine(this.standardModule.GetError());
            }
            this.communicator.Write(command.Value);
        }
        /// <summary>
        /// Stop the time table of the device.
        /// </summary>
        /// <returns></returns>
        public IObservable<Unit> StopTable()
        {
            var command = provider.GetCommand(AmpsCommandType.TimeTableStop);
            if (this.communicator.IsError)
            {
                System.Diagnostics.Trace.WriteLine(this.standardModule.GetError());
            }
            this.communicator.Write(command.Value);
        }

        /// <summary>
        /// Loads the Table onto the device
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public IObservable<Unit> LoadTimeTable(AmpsSignalTable table)
        {
            string command = table.FormatTable();

            this.LastTable = command;

            this.communicator.Write(command);

            if (this.communicator.IsError)
            {
                System.Diagnostics.Trace.WriteLine(this.standardModule.GetError());
            }
        }

        /// <summary>
        /// Tells the AMPS box which clock to use: external or internal
        /// </summary>
        /// <param name="clockType">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public IObservable<Unit> SetClock(ClockType clockType)
        {
            var command = new AmpsCommand("STBLCLK", "STBLCLK");
            command = command.AddParameter(",", clockType.ToString());
            this.communicator.Write(command);

            if (this.communicator.IsError)
            {
                System.Diagnostics.Trace.WriteLine(this.standardModule.GetError());
            }
        }

        /// <summary>
        /// Set the device trigger.
        /// </summary>
        /// <param name="startTriggerType"></param>
        /// <returns></returns>
        public IObservable<Unit> SetTrigger(StartTriggerTypes startTriggerType)
        {
            var command = provider.GetCommand(AmpsCommandType.CommandSetTrigger);
            this.communicator.Write(string.Format(command.Value, startTriggerType));

            if (this.communicator.IsError)
            {
                System.Diagnostics.Trace.WriteLine(this.standardModule.GetError());
            }
        }


    }
}