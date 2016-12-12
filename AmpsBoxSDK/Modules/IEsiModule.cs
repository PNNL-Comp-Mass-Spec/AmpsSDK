using System;
using System.ComponentModel.Composition;
using System.Reactive;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IEsiModule
    {
        IObservable<Unit> SetPositiveHighVoltage(int volts);
        IObservable<Unit> SetNegativeHighVoltage(int volts);
        Tuple<double, double> GetPositiveEsi();
        Tuple<double, double> GetNegativeEsi();
    }
}