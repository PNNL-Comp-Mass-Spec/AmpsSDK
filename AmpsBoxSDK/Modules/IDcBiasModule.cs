using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;
using FalkorSDK.Channel;
using FalkorSDK.Data.Signals;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IDcBiasModule
    {
        void SetDcBias(ChannelAddress channel, double volts);
        void SetDcBias(ChannelAddress channel, int volts);
        double GetDcSetPoint(ChannelAddress channel);
        double GetDcActualOutput(ChannelAddress channel);
        void SetDcOffset(ChannelAddress channel, double volts);
        double GetDcOffset(ChannelAddress channel);
        double GetMinimumVoltage(ChannelAddress channel);
        double GetMaximumVoltage(ChannelAddress channel);
        void SetDcPowerState(State state);
        State GetDcPowerState();
        IEnumerable<double> GetAllDcBiasSetpoints();
        IEnumerable<double> GetAllDcBiasActualOutputs();
        void SetNumberOfChannelsOnBoard(int board, int channels);
        void UseOneDcOffsetForTwoModules(bool useOneForTwo);
        void EnableOffsetReadback(bool enable);
        int GetHvChannelCount();
    }

    public class DcBiasModule : IDcBiasModule
    {
        private IAmpsBoxCommunicator communicator;
        private AmpsCommandProvider provider;

        [ImportingConstructor]
        public DcBiasModule(IAmpsBoxCommunicator communicator, AmpsCommandProvider provider)
        {
            this.communicator = communicator;
            this.provider = provider;
        }
        public void SetDcBias(ChannelAddress channel, double volts)
        {
            var command = provider.GetCommand(AmpsCommandType.SetDcBias);

            this.communicator.Write(string.Format(command.Value, channel.Address, volts));
        }

        public void SetDcBias(ChannelAddress channel, int volts)
        {
            var command = provider.GetCommand(AmpsCommandType.SetDcBias);

            this.communicator.Write(string.Format(command.Value, channel.Address, volts));
        }

        public double GetDcSetPoint(ChannelAddress channel)
        {
            var command = provider.GetCommand(AmpsCommandType.GetDcBias);

            this.communicator.Write(string.Format(command.Value, channel.Address));


            double output = 0;
            var s = this.communicator.Response;
            double.TryParse(s, out output);

            return new Voltage(output);
        }

        public double GetDcActualOutput(ChannelAddress channel)
        {
            throw new System.NotImplementedException();
        }

        public void SetDcOffset(ChannelAddress channel, double volts)
        {
            throw new System.NotImplementedException();
        }

        public double GetDcOffset(ChannelAddress channel)
        {
            throw new System.NotImplementedException();
        }

        public double GetMinimumVoltage(ChannelAddress channel)
        {
            throw new System.NotImplementedException();
        }

        public double GetMaximumVoltage(ChannelAddress channel)
        {
            throw new System.NotImplementedException();
        }

        public void SetDcPowerState(State state)
        {
            throw new System.NotImplementedException();
        }

        public State GetDcPowerState()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<double> GetAllDcBiasSetpoints()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<double> GetAllDcBiasActualOutputs()
        {
            throw new System.NotImplementedException();
        }

        public void SetNumberOfChannelsOnBoard(int board, int channels)
        {
            throw new System.NotImplementedException();
        }

        public void UseOneDcOffsetForTwoModules(bool useOneForTwo)
        {
            throw new System.NotImplementedException();
        }

        public void EnableOffsetReadback(bool enable)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Retrieves from the device how many HV DC Power Supplies are available.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GetHvChannelCount()
        {
            var command = provider.GetCommand(AmpsCommandType.GetChannels);
            string response = string.Empty;
            this.communicator.Write(string.Format(command.Value, Module.DCB));

            int channels;
            int.TryParse(this.communicator.Response, out channels);
            return channels;
        }
    }
}