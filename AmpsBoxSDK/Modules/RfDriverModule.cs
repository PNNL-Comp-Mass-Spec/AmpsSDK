using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using AmpsBoxSdk.Commands;
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
            // TODO: figure out if all values of frequency are already in kHz
           // if(frequency < 500 || frequency > 5000)
            Command command = new AmpsCommand("SRFFRQ", "SRFFRQ");
            command = command.AddParameter(",", address);
            command = command.AddParameter(",", frequency);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<int> GetFrequencySetting(string address)
        {
            Command command = new AmpsCommand("GRFFRQ", "GRFFRQ");
            command = command.AddParameter(",", address);

            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int frequency = 0;
                int.TryParse(s, out frequency);
                return frequency;
            });
        }

        public IObservable<Unit> SetRfDriveSetting(string address, int drive)
        {
            if (drive < 0 || drive > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(drive), "Range must be between 0 and 255");
            }
            Command command = new AmpsCommand("SRFDRV", "SRFDRV");
            command = command.AddParameter(",", address);
            command = command.AddParameter(",", address);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<int> GetRfDriveSetting(string address)
        {
            Command command = new AmpsCommand("GRFDRV", "GRFDRV");
            command = command.AddParameter(",", address);

            int dcBiasSetpoint = 0;
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int.TryParse(s, out dcBiasSetpoint);
                return dcBiasSetpoint;
            });
        }

        public IObservable<int> GetRfChannelNumber()
        {
            Command command = new AmpsCommand("GCHAN", "GCHAN");
            command = command.AddParameter(",", "RF");

            int dcBiasSetpoint = 0;
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int.TryParse(s, out dcBiasSetpoint);
                return dcBiasSetpoint;
            });
        }
    }
}