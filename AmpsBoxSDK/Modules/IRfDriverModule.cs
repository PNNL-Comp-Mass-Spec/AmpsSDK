using System;
using System.ComponentModel.Composition;
using System.Reactive;

namespace AmpsBoxSdk.Modules
{
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
}