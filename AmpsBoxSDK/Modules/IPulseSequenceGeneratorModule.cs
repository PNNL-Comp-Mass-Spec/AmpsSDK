using System;
using System.ComponentModel.Composition;
using System.Reactive;
using System.Text;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Data;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
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
}