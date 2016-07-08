using System;
using System.Collections;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;
using FalkorSDK.Data.MassSpectrometry;
using FalkorSDK.Data.Signals;
using FalkorSDK.Devices;

namespace AmpsBoxSdk.Modules
{
    public class MipsTWaveModule : ITwaveModule
    {
        private IAmpsBoxCommunicator communicator;
        private AmpsCommandProvider provider;

        [ImportingConstructor]
        public MipsTWaveModule(IAmpsBoxCommunicator communicator, AmpsCommandProvider provider)
        {
            this.communicator = communicator;
            this.provider = provider;
        }
        public int GetTWaveModuleCount()
        {
            var command = provider.GetCommand(AmpsCommandType.GetChannels);
            this.communicator.Write(string.Format(command.Value, Module.TWAVE));
            int channels;
            int.TryParse(this.communicator.Response, out channels);
            return channels;
        }

        public Frequency GetTravellingWaveFrequency(int boardNumber)
        {
            var command = provider.GetCommand(AmpsCommandType.GetTravellingWaveFrequency);
            this.communicator.Write(string.Format(command.Value, boardNumber));
            double freq = 0;
            double.TryParse(this.communicator.Response, out freq);
            return new Frequency(freq);
        }

        public Voltage GetTravellingWavePulseVoltage(int boardNumber)
        {
            var command = provider.GetCommand(AmpsCommandType.GetTWaveVoltage);
            this.communicator.Write(string.Format(command.Value, boardNumber));
            double volts = 0;
            double.TryParse(this.communicator.Response, out volts);
            return new Voltage(volts);
        }

        /// <summary>
        /// Set TWave frequency (freq in Hz).
        /// </summary>
        /// <param name="frequency"></param>
        public void SetTravellingWaveFrequency(int boardNumber, Frequency frequency)
        {
            var command = provider.GetCommand(AmpsCommandType.SetTravellingWaveFrequency);
            this.communicator.Write(string.Format(command.Value, boardNumber, frequency));

        }

        public void SetTravellingWavePulseVoltage(int boardNumber, Voltage voltage)
        {
            var command = provider.GetCommand(AmpsCommandType.SetTWaveVoltage);
            this.communicator.Write(string.Format(command.Value, boardNumber, voltage));

        }

        public Voltage GetGuardOneOutputVoltage(int boardNumber)
        {
            var command = provider.GetCommand(AmpsCommandType.GetGuardOneVoltage);
            this.communicator.Write(string.Format(command.Value, boardNumber));
            return new Voltage(double.Parse(this.communicator.Response));
        }

        public void SetGuardOneOutputVoltage(int boardNumber, Voltage voltage)
        {
            var command = provider.GetCommand(AmpsCommandType.SetGuardOneVoltage);
            this.communicator.Write(string.Format(command.Value, boardNumber, voltage));
        }

        public Voltage GetGuardTwoOutputVoltage(int boardNumber)
        {
            var command = provider.GetCommand(AmpsCommandType.GetGuardTwoVoltage);
            this.communicator.Write(string.Format(command.Value, boardNumber));
            return new Voltage(double.Parse(this.communicator.Response));
        }

        public void SetGuardTwoOutputVoltage(int boardNumber, Voltage voltage)
        {
            var command = provider.GetCommand(AmpsCommandType.SetGuardTwoVoltage);
            this.communicator.Write(string.Format(command.Value, boardNumber, voltage));
           
        }

        public BitArray GetTWaveOutputSequence(int boardNumber)
        {
            var command = provider.GetCommand(AmpsCommandType.GetOutputSequence);
            this.communicator.Write(string.Format(command.Value, boardNumber));

           
            var response = this.communicator.Response;
            var splitResponse = Regex.Split(response, string.Empty);
            var nonWhiteSpace = splitResponse.Where(x => x != string.Empty).ToArray();
            BitArray array = new BitArray(nonWhiteSpace.Length);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Convert.ToBoolean(int.Parse(nonWhiteSpace[i]));
            }

            return array;
        }

        public void SetTWaveOutputSequence(int boardNumber, BitArray array)
        {
            var command = provider.GetCommand(AmpsCommandType.SetOutputSequence);
            this.communicator.Write(string.Format(command.Value, boardNumber, array));
        }

        public void SetTWaveOutputDirection(int boardNumber, TWaveOutputDirection outputDirection)
        {
            var command = provider.GetCommand(AmpsCommandType.SetTWaveOutputDirection);
            this.communicator.Write(string.Format(command.Value, boardNumber, outputDirection));
           
        }

        public TWaveOutputDirection GetTWaveOutputDirection(int boardNumber)
        {
            var command = provider.GetCommand(AmpsCommandType.GetTWaveOutputDirection);
            this.communicator.Write(string.Format(command.Value, boardNumber));

           
            return (TWaveOutputDirection)Enum.Parse(typeof(TWaveOutputDirection), this.communicator.Response);
        }

        public void SetTWaveMultiPassControl(string asciiTable)
        {
            throw new NotImplementedException();
        }

        public string GetTWaveMultipassTable()
        {
            throw new NotImplementedException();
        }

        public CompressorMode GetCompressorMode()
        {
            throw new NotImplementedException();
        }

        public void SetCompressorMode()
        {
            throw new NotImplementedException();
        }

        public int GetCompressorOrder()
        {
            throw new NotImplementedException();
        }

        public void SetCompressorOrder(int order)
        {
            throw new NotImplementedException();
        }

        public int GetCompressorTriggerDelay()
        {
            throw new NotImplementedException();
        }

        public void SetCompressorTriggerDelay(int delayInMilliseconds)
        {
            throw new NotImplementedException();
        }

        public int GetCompressionTimeMilliseconds()
        {
            throw new NotImplementedException();
        }

        public void SetCompressionTime(int milliseconds)
        {
            throw new NotImplementedException();
        }

        public int GetNormalTimeInMilliseconds()
        {
            throw new NotImplementedException();
        }

        public void SetNormalTime(int milliseconds)
        {
            throw new NotImplementedException();
        }

        public int GetNonCompressTimeMilliseconds()
        {
            throw new NotImplementedException();
        }

        public void SetNonCompressTime(int milliseconds)
        {
            throw new NotImplementedException();
        }

        public void ForceMultipassTrigger()
        {
            throw new NotImplementedException();
        }

        public SwitchState GetSwitchState()
        {
            throw new NotImplementedException();
        }

        public void SetSwitchState(SwitchState switchState)
        {
            throw new NotImplementedException();
        }

        public void SetTWaveToCommonClockMode(bool useCommonClock)
        {
            throw new NotImplementedException();
        }

        public void SetTWaveCompressorMOde(bool useCompressorMode)
        {
            throw new NotImplementedException();
        }
    }
}