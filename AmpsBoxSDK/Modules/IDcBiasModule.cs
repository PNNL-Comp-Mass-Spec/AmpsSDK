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

        IObservable<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber);

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
                var command = new AmpsCommand("SDCB", "SDCB");
                command = command.AddParameter(",", channel);
                command = command.AddParameter(",", volts.ToString());
                await this.communicator.Write(command);
            });
        }

        public IObservable<int> GetDcBiasSetpoint(string channel)
        {
            return Observable.StartAsync(async () =>
            {
                var command = new AmpsCommand("GDCB", "GDCB");
                command = command.AddParameter(",", channel);
                var response = await this.communicator.Write(command);
                int dcBiasSetpoint = 0;
                int.TryParse(response, out dcBiasSetpoint);
                return dcBiasSetpoint;
            });
        }

        public IObservable<int> GetDcBiasReadback(string channel)
        {
            throw new NotImplementedException();
        }

        public IObservable<int> GetDcBiasCurrentReadback(string channel)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> SetBoardDcBiasOffsetVoltage(int brdNumber)
        {
            throw new NotImplementedException();
        }

        public IObservable<int> GetBoardDcBiasOffsetVoltage(int brdNumber)
        {
            throw new NotImplementedException();
        }

        public IObservable<int> GetNumberDcBiasChannels()
        {
            throw new NotImplementedException();
        }
    }
}