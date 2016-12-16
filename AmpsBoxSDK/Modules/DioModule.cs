using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    public class DioModule : IDioModule
    {
        private readonly IAmpsBoxCommunicator communicator;

        public DioModule(IAmpsBoxCommunicator communicator)
        {
            this.communicator = communicator;
        }

        public IObservable<Unit> SetDigitalState(string channel, bool state)
        {
            Command command = new AmpsCommand("SDIO", "SDIO");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", state);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<Unit> PulseDigitalSignal(string channel)
        {
            Command command = new AmpsCommand("SDIO", "SDIO");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", "P");

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<bool> GetDigitalState(string channel)
        {
            Command command = new AmpsCommand("GDIO", "GDIO");
            command.AddParameter(",", channel);
            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                bool digitalState;
                bool.TryParse(s, out digitalState);
                return digitalState;
            });
        }

        public IObservable<Unit> SetDigitalDirection(string channel, DigitalDirection digitalDirection)
        {
            Command command = new AmpsCommand("SDIODR", "SDIODR");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", digitalDirection.ToString());

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<DigitalDirection> GetDigitalDirection(string channel)
        {
            Command command = new AmpsCommand("GDIODR", "GDIODR");
            command = command.AddParameter(",", channel);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                var direction = (DigitalDirection)Enum.Parse(typeof(DigitalDirection), s);
                return direction;
            });
        }

        public IObservable<int> GetNumberDigitalChannels()
        {
            Command command = new AmpsCommand("GCHAN", "GCHAN");
            command.AddParameter(",", "DIO");
            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int numberOfChannels = 0;
                int.TryParse(s, out numberOfChannels);
                return numberOfChannels;
            });
        }
    }
}