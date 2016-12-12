using System;
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

       /// <summary>
       /// 
       /// </summary>
       /// <param name="channel"></param>
       /// <param name="direction"></param>
        public void ToggleDigitalDirection(string channel, string direction)
        {
            var command = provider.GetCommand(AmpsCommandType.SetDigitalIoDirection);
           this.communicator.Write(string.Format(command.Value, channel.Address, direction));
        }

        public string GetDigitalDirection(string channel)
        {
            var command = provider.GetCommand(AmpsCommandType.GetDigitalIoDirection);
            this.communicator.Write(string.Format(command.Value, channel.Address));
            return this.communicator.Response;
        }

        public bool GetDigitalState(string channel)
        {
            var command = provider.GetCommand(AmpsCommandType.GetDigitalIo);
            this.communicator.Write(string.Format(command.Value, channel.Address));

            return Convert.ToBoolean(int.Parse(this.communicator.Response));
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="address"></param>
       /// <param name="state"></param>
        public void ToggleDigitalOutput(string address, bool state)
        {
            var command = provider.GetCommand(AmpsCommandType.SetDigitalIo);
            this.communicator.Write(string.Format(command.Value, address.Address, Convert.ToInt32(state)));
        }

     /// <summary>
     /// 
     /// </summary>
     /// <param name="address"></param>
        public void PulseDigitalOutput(string address)
        {
            var pulseParam = "P";

            var command = provider.GetCommand(AmpsCommandType.SetDigitalIo);
            this.communicator.Write(string.Format(command.Value, address.Address, pulseParam));

        }
    }
}