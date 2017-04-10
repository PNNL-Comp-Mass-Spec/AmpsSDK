using System.Reactive;
using System.Threading.Tasks;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Data;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    public interface IPulseSequenceGeneratorModule
    {
        Task<Unit> AbortTimeTable();
        Task<Unit> LoadTimeTable(AmpsSignalTable table);
        Task<Unit> SetClock(ClockType clockType);
        Task<Unit> SetTrigger(StartTrigger startTrigger);
        Task<Unit> SetMode(Modes mode);
        Task<Unit> StopTable();
        Task<Unit> StartTimeTable();
        string LastTable { get; }

        Task<string> ReportExecutionStatus();

    }
}