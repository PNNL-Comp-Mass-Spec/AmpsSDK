using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Data;
using AmpsBoxSdk.Devices;
using FalkorSDK.Data.Signals;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IPulseSequenceGeneratorModule
    {
        void AbortTimeTable();
        void LoadTimeTable(AmpsSignalTable table);
        void SetClock(ClockType clockType);
        void SetTrigger(StartTriggerTypes startTriggerType);
        void SetMode();
        void StopTable();
        string StartTimeTable();
        string LastTable { get; }

    }

    public class PulseSequenceGeneratorModule : IPulseSequenceGeneratorModule
    {
        private IAmpsBoxCommunicator communicator;
        private AmpsCommandProvider provider;

        [ImportingConstructor]
        public PulseSequenceGeneratorModule(AmpsCommandProvider provider, IAmpsBoxCommunicator communicator)
        {
            this.provider = provider;
            this.communicator = communicator;
        }
       /// <summary>
       /// 
       /// </summary>
        public void AbortTimeTable()
       {
           var command = provider.GetCommand(AmpsCommandType.TimeTableAbort);
           string formattedCommand = command.Value;
            this.communicator.Write(
                                  formattedCommand);
        }

        /// <summary>
        /// Starts execution of time table.
        /// </summary>
        /// <returns></returns>
        public string StartTimeTable()
        {
            var command = provider.GetCommand(AmpsCommandType.TimeTableStart);
            this.communicator.Write(command.Value);

            return "\tStarting time table.";
        }

        public string LastTable { get; private set; }

        /// <summary>
        /// Tells the AMPS Box how to repeat (if at all) 
        /// </summary>
        /// <param name="iterations">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public void SetMode()
        {
            var command = provider.GetCommand(AmpsCommandType.Mode);
            this.communicator.Write(command.Value);
        }
        /// <summary>
        /// Stop the time table of the device.
        /// </summary>
        /// <returns></returns>
        public void StopTable()
        {
            var command = provider.GetCommand(AmpsCommandType.TimeTableStop);
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
        public void LoadTimeTable(AmpsSignalTable table)
        {
            string command = AmpsBoxSignalTableCommandFormatter.FormatTable(table, null);
            this.LastTable = command;

            this.communicator.Write(command);
        }

        /// <summary>
        /// Tells the AMPS box which clock to use: external or internal
        /// </summary>
        /// <param name="clockType">
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public void SetClock(ClockType clockType)
        {
            var command = provider.GetCommand(AmpsCommandType.TimeTableClockSync);
            switch (clockType)
            {
                
                case ClockType.External:
                    this.communicator.Write(string.Format(command.Value, "EXT"));
                    break;

                case ClockType.Internal:
                    this.communicator.Write(string.Format(command.Value, "INT"));
                    break;
            }
        }

        /// <summary>
        /// Set the device trigger.
        /// </summary>
        /// <param name="startTriggerType"></param>
        /// <returns></returns>
        public void SetTrigger(StartTriggerTypes startTriggerType)
        {
            var command = provider.GetCommand(AmpsCommandType.CommandSetTrigger);
            this.communicator.Write(string.Format(command.Value, startTriggerType));
        }
    }
}