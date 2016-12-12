using System.ComponentModel.Composition;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IDioModule
    {
        void ToggleDigitalDirection(string channel, string direction);
        string GetDigitalDirection(string channel);
        bool GetDigitalState(string channel);
        void ToggleDigitalOutput(string address, bool state);
        void PulseDigitalOutput(string address);
    }
}