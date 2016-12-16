using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reactive;

namespace AmpsBoxSdk.Modules
{
    public interface IDcBiasModule
    {
        IObservable<Unit> SetDcBiasVoltage(string channel, int volts);
        IObservable<int> GetDcBiasSetpoint(string channel);
        IObservable<int> GetDcBiasReadback(string channel);

        IObservable<int> GetDcBiasCurrentReadback(string channel);

        IObservable<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber, int offsetVolts);

        IObservable<int> GetBoardDcBiasOffsetVoltage(int brdNumber);

        IObservable<int> GetNumberDcBiasChannels();
    }
}