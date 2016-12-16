using System;
using System.ComponentModel.Composition;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Data;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
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
            Command command = new AmpsCommand("TBLABRT", "TBLABRT");
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }

        /// <summary>
        /// Starts execution of time table.
        /// </summary>
        /// <returns></returns>
        public IObservable<Unit> StartTimeTable()
        {
            Command command = new AmpsCommand("TBLSTRT", "TBLSTRT");
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }

        public string LastTable { get; private set; }

        /// <summary>
        /// Sets the table mode for the amps / mips box.
        /// </summary>
        public IObservable<Unit> SetMode(Modes mode)
        {
            Command command = new AmpsCommand("SMOD", "SMOD");
            command.AddParameter(",", mode.ToString());
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }
        /// <summary>
        /// Stop the time table of the device.
        /// </summary>
        /// <returns></returns>
        public IObservable<Unit> StopTable()
        {
            Command command = new AmpsCommand("TBLSTOP", "TBLSTOP");
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
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
            string formattedTable = table.RetrieveTableAsEncodedString();
            this.LastTable = formattedTable;
            Command command = new AmpsCommand("STBLDAT", formattedTable);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
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
            Command command = new AmpsCommand("STBLCLK", "STBLCLK");
            command = command.AddParameter(",", clockType.ToString());
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }

        /// <summary>
        /// Set the device trigger.
        /// </summary>
        /// <param name="startTriggerType"></param>
        /// <returns></returns>
        public IObservable<Unit> SetTrigger(StartTriggerTypes startTriggerType)
        {
            Command command = new AmpsCommand("STBLCLK", "STBLCLK");
            command = command.AddParameter(",", startTriggerType.ToString());
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }


    }
}