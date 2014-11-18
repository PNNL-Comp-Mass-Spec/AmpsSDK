// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalogToDigitalConverterProperties.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The Analog To Digital Converter properties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data
{
    using System;

    using FalkorSDK.Data.Signals;
    using FalkorSDK.Devices;

    using ReactiveUI;

    /// <summary>
    /// TODO The Analog To Digital Converter properties.
    /// </summary>
    [Serializable]
    public class AnalogToDigitalConverterProperties : ReactiveObject, IAnalogToDigitalConverterProperties
    {
        #region Instrument Calibration Properties
        /// <summary>
        /// Gets or sets the sampling rate
        /// </summary>
        public double SamplingRate { get; set; }
        /// <summary>
        /// Gets or sets GateThreshold. Samples whose NormalizedIntensity is less than GateThreshold 
        /// would be rejected upon acquisition. Set this number to 0 to disable this feature.
        /// </summary>
        public int GateThreshold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enable pulse.
        /// Enable Pulse is a waveform regulates the trigger to period of interests.
        /// </summary>
        public bool EnablePulse { get; set; }

        /// <summary>
        /// Gets or sets the interval samples
        /// </summary>
        public int IntervalSamples { get; set; }

        /// <summary>
        /// Gets or sets the acquire channel offset.
        /// The channel offset of the ADC channel, unit volts.
        /// </summary>
        public double AcquireChannelOffset { get; set; }

        /// <summary>
        /// Gets or sets the acquire channel full scale range, in volts
        /// </summary>
        public double AcquireChannelFSR { get; set; }

        /// <summary>
        /// Gets or sets the average time of flight width.
        /// The unit is time in seconds
        /// </summary>
        public double AverageTimeOfFlightWidth { get; set; }
     

        /// <summary>
        /// Gets or sets the date calibrated.
        /// The last time a Manual calibration is performed.
        /// </summary>
        public DateTime DateCalibrated { get; set; }

        /// <summary>
        /// Gets or sets the start delay samples. Software handled ommition of a certain number of
        /// samples after each trigger.
        /// </summary>
        public int StartDelaySamples { get; set; }
        #endregion

        #region Experiment Calibration Properties

        /// <summary>
        /// Gets or sets the full scale intensity.
        /// The full scale range, in units of intensity.
        /// </summary>
        public double FullScaleIntensity { get; set; }

        /// <summary>
        /// Gets or sets the cal type.
        /// </summary>
        public CalibratorType CalType { get; set; }

        /// <summary>
        /// Gets or sets the time offset.
        /// </summary>
        public double TimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the cal a.
        /// </summary>
        public double CalibrationA { get; set; }

        /// <summary>
        /// Gets or sets the calibration t 0.
        /// For IMS t0 is the time the ion spent outside of a drift cell.
        /// </summary>
        public double CalibrationT0 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dither enable.
        /// </summary>
        public bool DitherEnable { get; set; }
        #endregion

        #region Acquisition Properties.

        /// <summary>
        /// Gets or sets the accumulations. An accumulation is a primitive form of a frame, the summation of multiple
        /// accumulations equals a frame.
        /// </summary>
        public int Accumulations { get; set; }

        /// <summary>
        /// Gets or sets the number of frames.
        /// </summary>
        public int NumberOfFrames { get; set; }

        /// <summary>
        /// Gets or sets the nbr samples. Should hold the same value as Number of segment per waveform. 
        /// </summary>
        public int NumberOfSamples { get; set; }

        /// <summary>
        /// Gets or sets the nbr segments per waveform; samples per waveform/TOFpulse
        /// </summary>
        public int NumberOfRecordsPerWaveform { get; set; }

        /// <summary>
        /// Gets or sets the predefined num scans. The number of mobility scans for each accumulation.
        /// </summary>
        public int PredefinedNumScans { get; set; }

        #endregion

        #region File IO properties

        /// <summary>
        /// Gets or sets the properties file name.
        /// </summary>
        public string PropertiesFileName { get; set; }

        /// <summary>
        /// Gets or sets the name multiplexing profile.
        /// </summary>
        public string NameMultiplexingProfile { get; set; }

        /// <summary>
        /// Gets or sets the write directory.
        /// </summary>
        public string WriteDirectory { get; set; }

        #endregion

        #region Trigger Properties

        /// <summary>
        /// Gets or sets the tof spectra per frame.
        /// </summary>
        public int TimeOfFlightSpectraPerFrame { get; set; }

        /// <summary>
        /// Gets or sets the ims trigger len.
        /// </summary>
        public int TriggerLength { get; set; }

        /// <summary>
        /// Gets or sets the type trigger.
        /// </summary>
        public SignalTypes TypeTrigger { get; set; }

        /// <summary>
        /// Gets or sets the spectra trigger period.
        /// </summary>
        public int SpectraTriggerPeriod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether sync on trig out sync.
        /// </summary>
        public bool SyncOnTrigOutSync { get; set; }

        /// <summary>
        /// Gets or sets the threshold trigger.
        /// </summary>
        public double ThresholdTrigger { get; set; }

        #endregion
    }
}