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


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Item1 = voltage, Item2 = uA</returns>
        public Tuple<double, double> GetPositiveEsi()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Item1 = voltage, Item2 = uA</returns>
        public Tuple<double, double> GetNegativeEsi()
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> SetPositiveHighVoltage(int volts)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> SetNegativeHighVoltage(int volts)
        {
            throw new NotImplementedException();
        }
    }
}