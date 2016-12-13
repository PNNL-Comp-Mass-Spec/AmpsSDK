using System;
using System.ComponentModel.Composition;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
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
        IObservable<Unit> SetMode(Modes mode);
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
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("TBLABRT", "TBLABRT");
                this.communicator.Write(command);
            });
        }

        /// <summary>
        /// Starts execution of time table.
        /// </summary>
        /// <returns></returns>
        public IObservable<Unit> StartTimeTable()
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("TBLSTRT", "TBLSTRT");
                this.communicator.Write(command);
            });
        }

        public string LastTable { get; private set; }

        /// <summary>
        /// Sets the table mode for the amps / mips box.
        /// </summary>
        public IObservable<Unit> SetMode(Modes mode)
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("SMOD", "SMOD");
                command.AddParameter(",", mode.ToString());
                this.communicator.Write(command);
            });
        }
        /// <summary>
        /// Stop the time table of the device.
        /// </summary>
        /// <returns></returns>
        public IObservable<Unit> StopTable()
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("TBLSTOP", "TBLSTOP");
                this.communicator.Write(command);
            });
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
            return Observable.Start(() =>
            {
                string formattedTable = table.RetrieveTableAsEncodedString();
                this.LastTable = formattedTable;
                Command command = new AmpsCommand("STBLDAT", formattedTable);
                this.communicator.Write(command);
            });
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
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("STBLCLK", "STBLCLK");
                command = command.AddParameter(",", clockType.ToString());
                this.communicator.Write(command);
            });
        }

        /// <summary>
        /// Set the device trigger.
        /// </summary>
        /// <param name="startTriggerType"></param>
        /// <returns></returns>
        public IObservable<Unit> SetTrigger(StartTriggerTypes startTriggerType)
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("STBLCLK", "STBLCLK");
                command = command.AddParameter(",", startTriggerType.ToString());
                this.communicator.Write(command);
            });
        }


    }
}