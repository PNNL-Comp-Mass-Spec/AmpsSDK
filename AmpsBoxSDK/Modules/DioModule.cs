using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;
using FalkorSDK.Channel;

namespace AmpsBoxSdk.Modules
{
    public class DioModule : IDioModule
    {
        private IAmpsBoxCommunicator communicator;
        private AmpsCommandProvider provider;

        public DioModule(IAmpsBoxCommunicator communicator, AmpsCommandProvider provider)
        {
            this.communicator = communicator;
            this.provider = provider;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="channel"></param>
       /// <param name="direction"></param>
        public void ToggleDigitalDirection(ChannelAddress channel, string direction)
        {
            var command = provider.GetCommand(AmpsCommandType.SetDigitalIoDirection);
           this.communicator.Write(string.Format(command.Value, channel.Address, direction));
        }

        public string GetDigitalDirection(ChannelAddress channel)
        {
            var command = provider.GetCommand(AmpsCommandType.GetDigitalIoDirection);
            this.communicator.Write(string.Format(command.Value, channel.Address));
            return this.communicator.Response;
        }

        public bool GetDigitalState(ChannelAddress channel)
        {
            var command = provider.GetCommand(AmpsCommandType.GetDigitalIo);
            this.communicator.Write(string.Format(command.Value, channel.Address));
            bool state;
            bool.TryParse(this.communicator.Response, out state);
            return state;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="address"></param>
       /// <param name="state"></param>
        public void ToggleDigitalOutput(ChannelAddress address, bool state)
        {
            var command = provider.GetCommand(AmpsCommandType.SetDigitalIo);
            this.communicator.Write(string.Format(command.Value, address.Address, Convert.ToInt32(state).ToString()));
        }

     /// <summary>
     /// 
     /// </summary>
     /// <param name="address"></param>
        public void PulseDigitalOutput(ChannelAddress address)
        {
            var pulseParam = "P";

            var command = provider.GetCommand(AmpsCommandType.SetDigitalIo);
            this.communicator.Write(string.Format(command.Value, address.Address, pulseParam));

        }
    }
}