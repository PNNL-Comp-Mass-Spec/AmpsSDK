using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AmpsBoxSdk.Commands;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    public class StandardModule : IStandardModule
    {
        private IAmpsBoxCommunicator communicator;
        private AmpsCommandProvider provider;

        [ImportingConstructor]
        public StandardModule(IAmpsBoxCommunicator communicator, AmpsCommandProvider provider)
        {
            this.communicator = communicator;
            this.provider = provider;
        }
        /// <summary>
        /// Gets the version of the AMPS box.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetVersion()
        {
            var command = provider.GetCommand(AmpsCommandType.GetVersion);
            this.communicator.Write(command.Value);
            var match = Regex.Match(this.communicator.Response, @"\d+(\.\d{1,2}(\w))?", RegexOptions.IgnoreCase);
            return match.Value;
        }

        /// <summary>
        /// Converts string? response into an int and then parses that to an enum. 
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public ErrorCodes GetError()
        {
            var command = provider.GetCommand(AmpsCommandType.GetError);
            this.communicator.Write(command.Value);
            int responseCode;
            int.TryParse(this.communicator.Response, out responseCode);
            var code = (ErrorCodes)Enum.ToObject(typeof(ErrorCodes), responseCode);
            return code;
        }

        public string GetName()
        {
            var command = provider.GetCommand(AmpsCommandType.GetName);
            this.communicator.Write(command.Value);
            return this.communicator.Response;
        }

        public void SetName(string name)
        {
            var command = provider.GetCommand(AmpsCommandType.SetName);
            this.communicator.Write(string.Format(command.Value, name));
        }

        /// <summary>
        /// Reset the AMPS box.
        /// </summary>
        /// <returns></returns>
        public void Reset()
        {
            var command = provider.GetCommand(AmpsCommandType.Reset);
            this.communicator.Write(
               command.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            var command = provider.GetCommand(AmpsCommandType.Save);
            this.communicator.Write(
                           command.Value);
        }

        public int GetChannelCount(Module module)
        {
            throw new System.NotImplementedException();
        }

        public void Delay(int milliseconds)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> GetCommands()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<bool> GetAnalogInputStatus()
        {
            throw new System.NotImplementedException();
        }

        public void SetAnalogInputStatus(bool status)
        {
            throw new System.NotImplementedException();
        }
    }
}