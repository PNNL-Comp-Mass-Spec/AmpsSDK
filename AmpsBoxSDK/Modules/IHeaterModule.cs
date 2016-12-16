using System;
using System.Reactive;

namespace AmpsBoxSdk.Modules
{
    public interface IHeaterModule
    {
        IObservable<Unit> TurnOnHeater();
        IObservable<Unit> TurnOffHeater();
        IObservable<Unit> SetTemperatureSetpoint(int temperature);
        IObservable<int> ReadTemperature();
        IObservable<Unit> SetPidGain(int gain);

    }
}