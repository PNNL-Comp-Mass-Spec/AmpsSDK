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
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("SDCB", "SDCB");
                command = command.AddParameter(",", channel);
                command = command.AddParameter(",", volts.ToString());
                await this.communicator.Write(command);
            });
        }

        public IObservable<int> GetDcBiasSetpoint(string channel)
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("GDCB", "GDCB");
                command = command.AddParameter(",", channel);
                var response = await this.communicator.Write(command);
                int dcBiasSetpoint = 0;
                int.TryParse(response, out dcBiasSetpoint);
                return dcBiasSetpoint;
            });
        }

        public IObservable<int> GetDcBiasReadback(string channel)
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("GDCBV", "GDCBV");
                command = command.AddParameter(",", channel);
                var response = await this.communicator.Write(command);
                int dcBiasReadback = 0;
                int.TryParse(response, out dcBiasReadback);
                return dcBiasReadback;
            });
        }

        public IObservable<int> GetDcBiasCurrentReadback(string channel)
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("GDCBI", "GDCBI");
                command = command.AddParameter(",", channel);
                var response = await this.communicator.Write(command);
                int dcBiasCurrentReadback = 0;
                int.TryParse(response, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });
        }

        public IObservable<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber, int offsetVolts)
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("SDCBOF", "SDCBOF");
                command = command.AddParameter(",", brdNumber).AddParameter(",", offsetVolts);
                var response = await this.communicator.Write(command);
            });
        }

        public IObservable<int> GetBoardDcBiasOffsetVoltage(int brdNumber)
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("GDCBOF", "GDCBOF");
                command = command.AddParameter(",", brdNumber);
                var response = await this.communicator.Write(command);
                int dcBiasCurrentReadback = 0;
                int.TryParse(response, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });
        }

        public IObservable<int> GetNumberDcBiasChannels()
        {
            return Observable.StartAsync(async () =>
            {
                Command command = new AmpsCommand("GCHAN", "GCHAN");
                command = command.AddParameter(",", "DCB");
                var response = await this.communicator.Write(command);
                int dcBiasCurrentReadback = 0;
                int.TryParse(response, out dcBiasCurrentReadback);
                return dcBiasCurrentReadback;
            });
        }
    }
}