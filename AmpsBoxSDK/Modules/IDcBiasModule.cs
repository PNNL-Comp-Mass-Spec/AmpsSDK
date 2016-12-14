using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reactive;
using System.Reactive.Linq;
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
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("SDCB", "SDCB");
                command = command.AddParameter(",", channel);
                command = command.AddParameter(",", volts.ToString());


                var messagePacket = this.communicator.MessageSources;
               var connection = messagePacket.Connect();
                messagePacket.Subscribe(s =>
                {
                    connection.Dispose();
                });
              
                this.communicator.Write(command);
            });
        }

        public IObservable<int> GetDcBiasSetpoint(string channel)
        {
            return Observable.Start(() =>
            {
                Command command = new AmpsCommand("GDCB", "GDCB");
                command = command.AddParameter(",", channel);

                var messagePacket = this.communicator.MessageSources;
                var connection = messagePacket.Connect();
                int dcBiasSetpoint = 0;
                messagePacket.Subscribe(s =>
                {
                    
                    int.TryParse(s.ToString(), out dcBiasSetpoint);
                    connection.Dispose();
                });

                this.communicator.Write(command);
               
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