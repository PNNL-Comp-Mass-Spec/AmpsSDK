using System;
using System.ComponentModel.Composition;
using System.Reactive;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    public class RfDriverModule : IRfDriverModule
    {
        private readonly IAmpsBoxCommunicator communicator;

        [ImportingConstructor]
        public RfDriverModule(IAmpsBoxCommunicator communicator)
        {
            this.communicator = communicator;
        }

        public IObservable<Unit> SetFrequency(string address, int frequency)
        {
            throw new NotImplementedException();
        }

        public IObservable<int> GetFrequencySetting(string address)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> SetRfDriveSetting(string address, int drive)
        {
            throw new NotImplementedException();
        }

        public IObservable<int> GetRfDriveSetting(string address)
        {
            throw new NotImplementedException();
        }

        public IObservable<int> GetRfChannelNumber()
        {
            throw new NotImplementedException();
        }
    }
}