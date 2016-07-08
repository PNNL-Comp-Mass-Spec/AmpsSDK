using AmpsBoxSdk.Modules;

namespace AmpsBoxSdk.Devices
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AmpsBoxSdk.Commands;

    using FalkorSDK.Channel;
    using FalkorSDK.Data.Signals;
    using FalkorSDK.Devices;

    public interface IAmpsBox
    {
        /// <summary>
        /// Gets or sets a value indicating whether is emulated or not.
        /// </summary>
        bool Emulated { get; set; }

        string LatestResponse { get; }
        string LatestWrite { get; }

        IClockGenerationModule ClockGenerationModule { get; }

        IPulseSequenceGeneratorModule PulseSequenceGeneratorModule { get; }

        IDcBiasModule DcBiasModule { get; }

        IStandardModule StandardModule { get; }

        IDioModule DioModule { get; }

        IEsiModule EsiModule { get; }

        IFaimsModule FaimsModule { get; }

        IFilamentModule FilamentModule { get; }

        IMacroModule MacroModule { get; }

        IRfDriverModule RfDriverModule { get; }

        ITwaveModule TWaveModule { get; }

        IWiFiModule WiFiModule { get; }
        
    }
}