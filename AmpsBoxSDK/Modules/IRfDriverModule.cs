using System;
using System.ComponentModel.Composition;
using System.Reactive;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IRfDriverModule
    {
        IObservable<Unit> SetFrequency(string address, int frequency);
        IObservable<int> GetFrequencySetting(string address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="drive">Between 0-255</param>
        /// <returns></returns>
        IObservable<Unit> SetRfDriveSetting(string address, int drive);

        IObservable<int> GetRfDriveSetting(string address);

        IObservable<int> GetRfChannelNumber();
    }

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