using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IDcBiasModule
    {
        IObservable<Unit> SetDcBiasVoltage(string channel, int volts);
        IObservable<int> GetDcBiasSetpoint(string channel);
        IObservable<int> GetDcBiasReadback(string channel);

        IObservable<int> GetDcBiasCurrentReadback(string channel);

        IObservable<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber, int offsetVolts);

        IObservable<int> GetBoardDcBiasOffsetVoltage(int brdNumber);

        IObservable<int> GetNumberDcBiasChannels();
    }

    public class DcBiasModule : IDcBiasModule
    {
        private readonly IAmpsBoxCommunicator communicator;

        [ImportingConstructor]
        public DcBiasModule(IAmpsBoxCommunicator communicator)
        {
            this.communicator = communicator;
        }

        public IObservable<Unit> SetDcBiasVoltage(string channel, int volts)
        {
            Command command = new AmpsCommand("SDCB", "SDCB");
            command = command.AddParameter(",", channel);
            command = command.AddParameter(",", volts.ToString());


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
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("GDCBV", "GDCBV");
                command = command.AddParameter(",", channel);
                this.communicator.Write(command);
                int dcBiasReadback = 0;
               // int.TryParse(response, out dcBiasReadback);
                return dcBiasReadback;
            });
        }

        public IObservable<int> GetDcBiasCurrentReadback(string channel)
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("GDCBI", "GDCBI");
                command = command.AddParameter(",", channel);
                this.communicator.Write(command);
                int dcBiasCurrentReadback = 0;
            //    int.TryParse(response, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });
        }

        public IObservable<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber, int offsetVolts)
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("SDCBOF", "SDCBOF");
                command = command.AddParameter(",", brdNumber).AddParameter(",", offsetVolts);
                this.communicator.Write(command);
            });
        }

        public IObservable<int> GetBoardDcBiasOffsetVoltage(int brdNumber)
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("GDCBOF", "GDCBOF");
                command = command.AddParameter(",", brdNumber);
                this.communicator.Write(command);
                int dcBiasCurrentReadback = 0;
             //   int.TryParse(response, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });
        }

        public IObservable<int> GetNumberDcBiasChannels()
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("GCHAN", "GCHAN");
                command = command.AddParameter(",", "DCB");
               this.communicator.Write(command);
                int dcBiasCurrentReadback = 0;
                //int.TryParse(response, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });
        }
    }
}