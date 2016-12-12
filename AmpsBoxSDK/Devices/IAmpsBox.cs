using AmpsBoxSdk.Modules;

namespace AmpsBoxSdk.Devices
{
    public interface IAmpsBox
    {
        IPulseSequenceGeneratorModule PulseSequenceGeneratorModule { get; }

        IDcBiasModule DcBiasModule { get; }

        IStandardModule StandardModule { get; }

        IDioModule DioModule { get; }

        IEsiModule EsiModule { get; }

        IRfDriverModule RfDriverModule { get; }

        string GetConfig();

    }
}