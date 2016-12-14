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
    public class DcBiasModule : IDcBiasModule
    {
        private readonly IAmpsBoxCommunicator communicator;

        public DcBiasModule(IAmpsBoxCommunicator communicator)
        {
            this.communicator = communicator;
        }

        public IObservable<Unit> SetDcBiasVoltage(string channel, int volts)
        {
            Command command = new AmpsCommand("SDCB", "SDCB");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", volts);

            var messagePacket = this.communicator.MessageSources;
            this.communicator.Write(command);
            return messagePacket.Select(bytes => Unit.Default);
        }

        public IObservable<int> GetDcBiasSetpoint(string channel)
        {
            Command command = new AmpsCommand("GDCB", "GDCB");
            command = command.AddParameter(",", channel);

            var messagePacket = this.communicator.MessageSources;
            int dcBiasSetpoint = 0;
            this.communicator.Write(command);
            return messagePacket.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int.TryParse(s, out dcBiasSetpoint);
                return dcBiasSetpoint;
            });
        }

        public IObservable<int> GetDcBiasReadback(string channel)
        {
            Command command = new AmpsCommand("GDCBV", "GDCBV");
            command = command.AddParameter(",", channel);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int dcBiasReadback = 0;
                int.TryParse(s, out dcBiasReadback);
                return dcBiasReadback;
            });
            
        }

        public IObservable<int> GetDcBiasCurrentReadback(string channel)
        {
            Command command = new AmpsCommand("GDCBI", "GDCBI");
            command = command.AddParameter(",", channel);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int dcBiasCurrentReadback = 0;
                int.TryParse(s, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });
            
        }

        public IObservable<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber, int offsetVolts)
        {
            Command command = new AmpsCommand("SDCBOF", "SDCBOF");
            command = command.AddParameter(",", brdNumber).AddParameter(",", offsetVolts);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes => Unit.Default);
        }

        public IObservable<int> GetBoardDcBiasOffsetVoltage(int brdNumber)
        {
            Command command = new AmpsCommand("GDCBOF", "GDCBOF");
            command = command.AddParameter(",", brdNumber);
            this.communicator.Write(command);
            return this.communicator.MessageSources.Select(bytes =>
            {
                var s = Encoding.ASCII.GetString(bytes.ToArray());
                int dcBiasCurrentReadback = 0;
                int.TryParse(s, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });

            
        }

        public IObservable<int> GetNumberDcBiasChannels()
        {
            Command command = new AmpsCommand("GCHAN", "GCHAN");
            command = command.AddParameter(",", "DCB");
            this.communicator.Write(command);

            return this.communicator.MessageSources.Select(bytes =>
            {
                var response = Encoding.ASCII.GetString(bytes.ToArray());
                int dcBiasCurrentReadback = 0;
                int.TryParse(response, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });
            
        }
    }
}