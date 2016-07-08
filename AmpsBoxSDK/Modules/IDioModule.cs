using System.ComponentModel.Composition;
using FalkorSDK.Channel;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IDioModule
    {
        void ToggleDigitalDirection(ChannelAddress channel, string direction);
        string GetDigitalDirection(ChannelAddress channel);
        bool GetDigitalState(ChannelAddress channel);
        void ToggleDigitalOutput(ChannelAddress address, bool state);
        void PulseDigitalOutput(ChannelAddress address);
    }
}