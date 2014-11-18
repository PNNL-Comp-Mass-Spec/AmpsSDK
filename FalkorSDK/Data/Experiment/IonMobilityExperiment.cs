namespace FalkorSDK.Data.Experiment
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using FalkorSDK.Data.Signals;
    using FalkorSDK.Devices;

    public class IonMobilityExperiment : Experiment
    {
        public IDigitizer Digitizer { get; set; }
        public Dictionary<ISignalTableDevice, SignalTable> TimeTableMappings { get; set; }

        private UnifiedIonMobilityFile Writer;

        private ConcurrentQueue<Frame> unprocessedAccumulations;

        private Frame currentFrame;

    }
}
