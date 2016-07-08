using System;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;
using FalkorSDK.Channel;

namespace AmpsBoxSdk.Modules
{
    public class AmpsEsiModule : IEsiModule
    {
        private IAmpsBoxCommunicator communicator;

        private AmpsCommandProvider provider;

        [ImportingConstructor]
        public AmpsEsiModule(IAmpsBoxCommunicator communicator, AmpsCommandProvider provider)
        {
            this.communicator = communicator;
            this.provider = provider;
        }
        /// <summary>
        /// Set the high voltage.
        /// </summary>
        /// <param name="voltage">      The voltage to set.</param>
        public void SetChannelVoltage(double voltage)
        {
            throw new NotImplementedException();
        }

        public void SetChannelVoltage(ChannelAddress address, double voltage)
        {
            throw new NotImplementedException();
        }

        public double GetChannelVoltageSetpoint(ChannelAddress address)
        {
            throw new NotImplementedException();
        }

        public double GetChannelOutputVoltage(ChannelAddress address)
        {
            throw new NotImplementedException();
        }

        public double GetChannelCurrentMilliAmps(ChannelAddress address)
        {
            throw new NotImplementedException();
        }

        public double GetChannelMaxVoltage(ChannelAddress address)
        {
            throw new NotImplementedException();
        }
    }
}