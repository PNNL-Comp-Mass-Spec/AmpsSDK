using System;
using System.ComponentModel.Composition;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IRfDriverModule
    {
        void Setint(string address, int frequency);
        int Getint(string address);
        void SetRfPeaktoPeakLevel(string address, double voltagePeakToPeak);
        double GetRfPositiveVpp(string address);
        double GetRfNegativeVpp(string address);
        int RfChannelCount();
        int GetDriveLevel(string channel);
        void SetRadiointOutputdouble(string address, int voltage);
    }

    public class RfDriverModule : IRfDriverModule
    {
        private readonly IAmpsBoxCommunicator communicator;

        [ImportingConstructor]
        public RfDriverModule(IAmpsBoxCommunicator communicator)
        {
            this.communicator = communicator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="voltage"></param>
        public void SetRadiointOutputdouble(string address, int voltage)
        {
            var command = provider.GetCommand(AmpsCommandType.SetDriveLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="frequency"></param>
        public void Setint(string address, int frequency)
        {
            var command = provider.GetCommand(AmpsCommandType.Setint);

        }

        public int Getint(string address)
        {
           var command = provider.GetCommand(AmpsCommandType.Getint);
            this.communicator.Write(string.Format("{1}{0}{2}", provider.CommandSeparator, command.Value, address.Address));

            var data = this.communicator.Response.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
            int frequency = 0;
            foreach (var s in data)
            {
                int.TryParse(s, out frequency);
            }

            return frequency;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public double GetRfOutputdouble(string address)
        {
            var command = provider.GetCommand(AmpsCommandType.GetRfdouble);


            double resultValue;
            double.TryParse(this.communicator.Response, out resultValue);
            return resultValue;
        }

        public void SetRfPeaktoPeakLevel(string address, double voltagePeakToPeak)
        {
            throw new System.NotImplementedException();
        }

        public double GetRfPositiveVpp(string address)
        {
            throw new System.NotImplementedException();
        }

        public double GetRfNegativeVpp(string address)
        {
            throw new System.NotImplementedException();
        }

        public int RfChannelCount()
        {
            string response = string.Empty;
            var command = provider.GetCommand(AmpsCommandType.GetChannels);
            this.communicator.Write(string.Format(command.Value, Module.RF));


            int result;
            int.TryParse(this.communicator.Response, out result);
            return result;
        }

        public int GetDriveLevel(string channel)
        {
            var command = provider.GetCommand(AmpsCommandType.GetDriveLevel);
            this.communicator.Write(string.Format(command.Value, channel.Address));

            int driveLevel = 0;
            int.TryParse(this.communicator.Response, out driveLevel);
            return driveLevel;
        }
    }
}