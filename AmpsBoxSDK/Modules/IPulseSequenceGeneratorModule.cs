using System;
using System.ComponentModel.Composition;
using System.Reactive;
using System.Text;
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
        Task<Unit> SetTrigger(StartTriggerTypes startTriggerType);
        Task<Unit> SetMode(Modes mode);
        Task<Unit> StopTable();
        Task<Unit> StartTimeTable();
        string LastTable { get; }

    }
}