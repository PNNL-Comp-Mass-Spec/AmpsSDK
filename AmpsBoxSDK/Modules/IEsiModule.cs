using System;
using System.ComponentModel.Composition;
using System.Reactive;

namespace AmpsBoxSdk.Modules
{
    public interface IEsiModule
    {
        IObservable<Unit> SetPositiveHighVoltage(int volts);
        IObservable<Unit> SetNegativeHighVoltage(int volts);
        IObservable<Tuple<double, double>> GetPositiveEsi();
        IObservable<Tuple<double, double>> GetNegativeEsi();
    }
}