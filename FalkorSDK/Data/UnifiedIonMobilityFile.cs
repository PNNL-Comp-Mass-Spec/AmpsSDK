// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnifiedIonMobilityFile.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The uimf.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using FalkorSDK.Devices;

    using Microsoft.Practices.Prism.Logging;

    using UIMFLibrary;

    /// <summary>
    /// TODO The Unified Ion Mobility File.
    /// </summary>
    [Export]
    public class UnifiedIonMobilityFile
    {
        #region Fields

        /// <summary>
        /// TODO The _frame parameters.
        /// </summary>
        private FrameParameters _frameParameters;

        /// <summary>
        /// TODO The _global parameters.
        /// </summary>
        private GlobalParameters _globalParameters;

        /// <summary>
        /// TODO The _uimf file name.
        /// </summary>
        private string fileName;

        /// <summary>
        /// TODO The frame count.
        /// </summary>
        private int frameCount;

        /// <summary>
        /// The logger.
        /// </summary>
        private ILoggerFacade logger;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UnifiedIonMobilityFile"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        [ImportingConstructor]
        public UnifiedIonMobilityFile(ILoggerFacade logger)
        {
            this.logger = logger;
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public IAnalogToDigitalConverterProperties Properties { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// TODO The create new uimf.
        /// </summary>
        /// <param name="nameOfFile">
        /// TODO The file name.
        /// </param>
        /// <param name="properties">
        /// TODO The properties.
        /// </param>
        /// <param name="numberOfFrames">
        /// TODO The number of frames.
        /// </param>
        public void CreateNewUnifiedIonMobilityFile(string nameOfFile, IAnalogToDigitalConverterProperties properties, int numberOfFrames)
        {
            // TODO: we should change this to be handled creation of the class; that way the inserting frame can't happen without this running.
            this.fileName = nameOfFile;
            if (properties == null)
            {
                properties = new AnalogToDigitalConverterProperties();
            }

            this.Properties = properties;
            using (var uimfWriter = new DataWriter(nameOfFile))
            {
                uimfWriter.CreateTables(null);

                // TODO: Obtain Tof Intensity Type and Instrument Name from another object, most likely an experiment object. TofIntensityType should most likely live inside the 
                // TODO: Properties object.
                this._globalParameters = new GlobalParameters
                                             {
                                                 DatasetType = string.Empty, 
                                                 Bins = this.Properties.NumberOfRecordsPerWaveform, 
                                                 // 	BinWidth = (Properties.IntervalSamples * 1000000.0) * 1000.0,
                                                 BinWidth = 1, 
                                                 FrameDataBlobVersion = 0.1F, 
                                                 NumFrames = numberOfFrames, 
                                                 Prescan_Accumulations = this.Properties.Accumulations, 
                                                 Prescan_TOFPulses =
                                                     this.Properties.TimeOfFlightSpectraPerFrame, 
                                                 ScanDataBlobVersion = 0.1F, 
                                                 TimeOffset = (int)this.Properties.TimeOffset, 
                                                 TOFIntensityType = "ADC", 
                                                 InstrumentName = "SLIM03"
                                             };

                CheckGlobalParameters(this._globalParameters);
                uimfWriter.InsertGlobal(this._globalParameters);
                this.frameCount = 0;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="accumulatedData">
        /// </param>
        public void WriteUnifiedIonMobilityFileFrame(IDictionary<uint, IDictionary<int, int>> accumulatedData)
        {
            using (var uimfWriter = new DataWriter(this.fileName))
            {
                try
                {
                    if (this._frameParameters == null)
                    {
                        this._frameParameters = new FrameParameters();
                    }

                    if (this.Properties == null)
                    {
                        throw new NullReferenceException();
                    }

                    this._frameParameters.Accumulations = this.Properties.Accumulations;
                    this._frameParameters.AverageTOFLength = this.Properties.AverageTimeOfFlightWidth * 100.0;

                    this._frameParameters.CalibrationIntercept = this.Properties.CalibrationT0 / 10000.0;
                    this._frameParameters.CalibrationSlope = this.Properties.CalibrationA * 10000.0;

                    this._frameParameters.a2 = 0;
                    this._frameParameters.b2 = 0;
                    this._frameParameters.c2 = 0;
                    this._frameParameters.d2 = 0;
                    this._frameParameters.e2 = 0;
                    this._frameParameters.f2 = 0;
                    this._frameParameters.FrameNum = this.frameCount;
                    {
                        this._frameParameters.FrameType = DataReader.FrameType.MS1; // Normal - MS
                    }

                    this._frameParameters.IMFProfile = string.Empty;
                    this._frameParameters.Scans = this.Properties.TimeOfFlightSpectraPerFrame;
                    this._frameParameters.CalibrationDone = 1;
                    CheckFrameParameters(this._frameParameters);
                    uimfWriter.InsertFrame(this._frameParameters);

                    foreach (var scanKvp in accumulatedData)
                    {
                        int scanNumber = (int)scanKvp.Key;
                        var scanData = scanKvp.Value;

                        List<int> bins = scanData.Keys.ToList();
                        bins.Sort();
                        List<int> intensities = new List<int>();
                        foreach (var binIndex in bins)
                        {
                            intensities.Add(scanData[binIndex]);
                        }

                        uimfWriter.InsertScan(
                            this._frameParameters, 
                            scanNumber, 
                            bins, 
                            intensities, 
                            this.Properties.IntervalSamples, 
                            (int)this.Properties.TimeOffset);
                    }

                    this.frameCount++;
                }
                catch (Exception ex)
                {
                    this.logger.Log(ex.Message, Category.Exception, Priority.High);
                    this.logger.Log(ex.StackTrace, Category.Exception, Priority.High);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// TODO The check frame parameters.
        /// </summary>
        /// <param name="fp">
        /// TODO The fp.
        /// </param>
        private static void CheckFrameParameters(FrameParameters fp)
        {
            if (fp.FragmentationProfile == null)
            {
                fp.FragmentationProfile = new double[0];
            }

            if (fp.IMFProfile == null)
            {
                fp.IMFProfile = string.Empty;
            }
        }

        /// <summary>
        /// Ensure no null properties in the global parameters.
        /// </summary>
        /// <param name="gp">
        /// </param>
        private static void CheckGlobalParameters(GlobalParameters gp)
        {
            if (gp.DatasetType == null)
            {
                gp.DatasetType = string.Empty;
            }

            if (gp.DateStarted == null)
            {
                gp.DateStarted = DateTime.Now.ToLocalTime().ToShortDateString() + " "
                                 + DateTime.Now.ToLocalTime().ToLongTimeString();
            }

            if (gp.Prescan_Profile == null)
            {
                gp.Prescan_Profile = string.Empty;
            }

            if (gp.TOFIntensityType == null)
            {
                gp.TOFIntensityType = string.Empty;
            }
        }

        #endregion
    }
}