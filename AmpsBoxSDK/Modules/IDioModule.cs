using System;
using System.ComponentModel.Composition;
using System.Reactive;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    public interface IDioModule
    {
        IObservable<Unit> SetDigitalState(string channel, bool state);
        IObservable<Unit> PulseDigitalSignal(string channel);

        IObservable<bool> GetDigitalState(string channel);
        IObservable<Unit> SetDigitalDirection(string channel, DigitalDirection digitalDirection);
        IObservable<DigitalDirection> GetDigitalDirection(string channel);
        IObservable<int> GetNumberDigitalChannels();
    }
}