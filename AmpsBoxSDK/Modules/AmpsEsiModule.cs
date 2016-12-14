using System;
using System.ComponentModel.Composition;
using System.Reactive;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    public class AmpsEsiModule : IEsiModule
    {
        private IAmpsBoxCommunicator communicator;

        [ImportingConstructor]
        public AmpsEsiModule(IAmpsBoxCommunicator communicator)
        {
            this.communicator = communicator;
        }

        public IObservable<Unit> SetPositiveHighVoltage(int volts)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> SetNegativeHighVoltage(int volts)
        {
            throw new NotImplementedException();
        }

        public IObservable<Tuple<double, double>> GetPositiveEsi()
        {
            throw new NotImplementedException();
        }

        public IObservable<Tuple<double, double>> GetNegativeEsi()
        {
            throw new NotImplementedException();
        }
    }
}