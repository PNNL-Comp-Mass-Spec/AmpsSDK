// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnalogToDigitalConverterProperties.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The AnalogToDigitalConverterProperties interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Devices
{
    using System;

    using FalkorSDK.Data.Signals;

    using ReactiveUI;

    /// <summary>
    /// Gets or sets GateThreshold. Samples whose NormalizedIntensity is less than GateThreshold 
    /// would be rejected upon acquisition. Set this number to 0 to disable this feature.
    /// </summary>
    public interface IAnalogToDigitalConverterProperties
    {

        #region Instrument Calibration Properties

        /// <summary>
        /// Gets or sets the for samples to be accepted, in volts.
        /// </summary>
        int GateThreshold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enable pulse.
        /// Enable Pulse is a waveform regulates the trigger to period of interests.
        /// </summary>
        bool EnablePulse { get; set; }

        /// <summary>
        /// Gets or sets the interval samples
        /// </summary>
        int IntervalSamples { get; set; }

        /// <summary>
        /// Gets or sets the acquire channel offset.
        /// The channel offset of the ADC channel, unit volts.
        /// </summary>
        double AcquireChannelOffset { get; set; }

        /// <summary>
        /// Gets or sets the acquire channel full scale range, in volts
        /// </summary>
        double AcquireChannelFSR { get; set; }

        /// <summary>
        /// Gets or sets the average time of flight width.
        /// The unit is time in seconds
        /// </summary>
        double AverageTimeOfFlightWidth { get; set; }
     

        /// <summary>
        /// Gets or sets the date calibrated.
        /// The last time a Manual calibration is performed.
        /// </summary>
        DateTime DateCalibrated { get; set; }

        /// <summary>
        /// Gets or sets the start delay samples. Software handled ommition of a certain number of
        /// samples after each trigger.
        /// </summary>
        int StartDelaySamples { get; set; }
        #endregion

        #region Experiment Calibration Properties

        /// <summary>
        /// Gets or sets the full scale intensity.
        /// The full scale range, in units of intensity.
        /// </summary>
        double FullScaleIntensity { get; set; }

        /// <summary>
        /// Gets or sets the cal type.
        /// </summary>
        CalibratorType CalType { get; set; }

        /// <summary>
        /// Gets or sets the time offset.
        /// </summary>
        double TimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the cal a.
        /// </summary>
        double CalibrationA { get; set; }

        /// <summary>
        /// Gets or sets the calibration t 0.
        /// For IMS t0 is the time the ion spent outside of a drift cell.
        /// </summary>
        double CalibrationT0 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dither enable.
        /// </summary>
        bool DitherEnable { get; set; }
        #endregion

        #region Acquisition Properties.

        /// <summary>
        /// Gets or sets the accumulations. An accumulation is a primitive form of a frame, the summation of multiple
        /// accumulations equals a frame.
        /// </summary>
        int Accumulations { get; set; }

        /// <summary>
        /// Gets or sets the number of frames.
        /// </summary>
        int NumberOfFrames { get; set; }

        /// <summary>
        /// Gets or sets the nbr samples. Should hold the same value as Number of segment per waveform. 
        /// </summary>
        int NumberOfSamples { get; set; }

        /// <summary>
        /// Gets or sets the number records per waveform; samples per Fetch / Read Operation of Digitizer
        /// </summary>
        int NumberOfRecordsPerWaveform { get; set; }

        /// <summary>
        /// Gets or sets the predefined num scans. Number of spectra per accumulation.
        /// </summary>
        int PredefinedNumScans { get; set; }
        #endregion

        #region File IO properties

        /// <summary>
        /// Gets or sets the properties file name.
        /// </summary>
        string PropertiesFileName { get; set; }

        /// <summary>
        /// Gets or sets the name multiplexing profile.
        /// </summary>
        string NameMultiplexingProfile { get; set; }

        /// <summary>
        /// Gets or sets the write directory.
        /// </summary>
        string WriteDirectory { get; set; }

        #endregion

        #region Trigger Properties

        /// <summary>
        /// Gets or sets *total* number of records per frame. Ex: 360
        /// </summary>
        int TimeOfFlightSpectraPerFrame { get; set; }

        /// <summary>
        /// Gets or sets the ims trigger length.
        /// </summary>
        int TriggerLength { get; set; }

        /// <summary>
        /// Gets or sets the type trigger.
        /// </summary>
        SignalTypes TypeTrigger { get; set; }

        /// <summary>
        /// Gets or sets the spectra trigger period.
        /// </summary>
        int SpectraTriggerPeriod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether sync on trig out sync.
        /// </summary>
        bool SyncOnTrigOutSync { get; set; }

        /// <summary>
        /// Gets or sets the threshold trigger.
        /// </summary>
        double ThresholdTrigger { get; set; }

        #endregion
    }
}