using System.Collections.Generic;
using System.ComponentModel.Composition;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IStandardModule
    {
        string GetVersion();
        ErrorCodes GetError();
        string GetName();
        void SetName(string name);
        void Reset();
        void Save();
        int GetChannelCount(Module module);
        void Delay(int milliseconds);
        IEnumerable<string> GetCommands();
        IEnumerable<bool> GetAnalogInputStatus();
        void SetAnalogInputStatus(bool status);

    }
}