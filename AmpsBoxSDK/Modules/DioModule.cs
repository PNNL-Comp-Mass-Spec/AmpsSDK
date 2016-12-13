using System;
using System.Reactive;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    public class DioModule : IDioModule
    {
        private IAmpsBoxCommunicator communicator;

        public DioModule(IAmpsBoxCommunicator communicator)
        {
            this.communicator = communicator;
        }

        public IObservable<Unit> SetDigitalState(string channel, bool state)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> PulseDigitalSignal(string channel)
        {
            throw new NotImplementedException();
        }

        IObservable<bool> IDioModule.GetDigitalState(string channel)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> SetDigitalDirection(string channel, DigitalDirection digitalDirection)
        {
            throw new NotImplementedException();
        }

        IObservable<DigitalDirection> IDioModule.GetDigitalDirection(string channel)
        {
            throw new NotImplementedException();
        }

        public IObservable<int> GetNumberDigitalChannels()
        {
            throw new NotImplementedException();
        }
    }
}