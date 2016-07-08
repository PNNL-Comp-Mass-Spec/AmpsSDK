using System;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;
using FalkorSDK.Channel;
using FalkorSDK.Data.Signals;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IRfDriverModule
    {
        void SetFrequency(ChannelAddress address, int frequency);
        int GetFrequency(ChannelAddress address);
        void SetRfPeaktoPeakLevel(ChannelAddress address, double voltagePeakToPeak);
        double GetRfPositiveVpp(ChannelAddress address);
        double GetRfNegativeVpp(ChannelAddress address);
        int RfChannelCount();
        int GetDriveLevel(ChannelAddress channel);
        void SetRadioFrequencyOutputVoltage(ChannelAddress address, int voltage);
    }

    public class RfDriverModule : IRfDriverModule
    {
        private readonly IAmpsBoxCommunicator communicator;
        private readonly AmpsCommandProvider provider;

        [ImportingConstructor]
        public RfDriverModule(AmpsCommandProvider provider, IAmpsBoxCommunicator communicator)
        {
            this.provider = provider;
            this.communicator = communicator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="voltage"></param>
        public void SetRadioFrequencyOutputVoltage(ChannelAddress address, int voltage)
        {
            var command = provider.GetCommand(AmpsCommandType.SetDriveLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="frequency"></param>
        public void SetFrequency(ChannelAddress address, int frequency)
        {
            var command = provider.GetCommand(AmpsCommandType.SetFrequency);

        }

        public int GetFrequency(ChannelAddress address)
        {
           var command = provider.GetCommand(AmpsCommandType.GetFrequency);
            this.communicator.Write(string.Format("{1}{0}{2}", provider.CommandSeparator, command.Value, address.Address));

            var data = this.communicator.Response.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
            int frequency = 0;
            foreach (var s in data)
            {
                int.TryParse(s, out frequency);
            }

            return frequency;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Voltage GetRfOutputVoltage(ChannelAddress address)
        {
            var command = provider.GetCommand(AmpsCommandType.GetRfVoltage);


            double resultValue;
            double.TryParse(this.communicator.Response, out resultValue);
            return resultValue;
        }

        public void SetRfPeaktoPeakLevel(ChannelAddress address, double voltagePeakToPeak)
        {
            throw new System.NotImplementedException();
        }

        public double GetRfPositiveVpp(ChannelAddress address)
        {
            throw new System.NotImplementedException();
        }

        public double GetRfNegativeVpp(ChannelAddress address)
        {
            throw new System.NotImplementedException();
        }

        public int RfChannelCount()
        {
            string response = string.Empty;
            var command = provider.GetCommand(AmpsCommandType.GetChannels);
            this.communicator.Write(string.Format(command.Value, Module.RF));


            int result;
            int.TryParse(this.communicator.Response, out result);
            return result;
        }

        public int GetDriveLevel(ChannelAddress channel)
        {
            var command = provider.GetCommand(AmpsCommandType.GetDriveLevel);
            this.communicator.Write(string.Format(command.Value, channel.Address));

            int driveLevel = 0;
            int.TryParse(this.communicator.Response, out driveLevel);
            return driveLevel;
        }
    }
}