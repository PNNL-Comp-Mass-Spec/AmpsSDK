using System.ComponentModel.Composition;
using FalkorSDK.Channel;

namespace AmpsBoxSdk.Modules
{
    [InheritedExport]
    public interface IEsiModule
    {
        void SetChannelVoltage(ChannelAddress address, double voltage);
        double GetChannelVoltageSetpoint(ChannelAddress address);
        double GetChannelOutputVoltage(ChannelAddress address);
        double GetChannelCurrentMilliAmps(ChannelAddress address);
        double GetChannelMaxVoltage(ChannelAddress address);
    }
}