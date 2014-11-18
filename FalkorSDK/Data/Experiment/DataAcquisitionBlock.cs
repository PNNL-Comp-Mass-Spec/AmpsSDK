// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataAcquisitionBlock.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The data acquisition block.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.Data.Experiment
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    using FalkorSDK.Devices;

    using ReactiveUI;

    /// <summary>
    /// TODO The data acquisition block.
    /// </summary>
    [Export]
    public class DataAcquisitionBlock : ExperimentBlock<Frame>
    {
        #region Fields

        /// <summary>
        /// TODO The digitizer.
        /// </summary>
        private IDigitizer digitizer;

        /// <summary>
        /// TODO The UnifiedIonMobilityFile.
        /// </summary>
        private UnifiedIonMobilityFile unifiedIonMobilityFile;

        /// <summary>
        /// The frame action block.
        /// </summary>
        private ActionBlock<Frame> frameActionBlock;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAcquisitionBlock"/> class.
        /// </summary>
        [ImportingConstructor]
        public DataAcquisitionBlock()
        {
            this.frameActionBlock = new ActionBlock<Frame>(frame => this.ProcessFrame(frame));
        }

        /// <summary>
        /// The process frame.
        /// </summary>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task ProcessFrame(Frame frame)
        {
            await Task.Run(() => this.unifiedIonMobilityFile.WriteUnifiedIonMobilityFileFrame(frame.Spectra));
        }

        #endregion

        /// <summary>
        /// The create unified ion mobility file.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="adcProperties">
        /// The adc properties.
        /// </param>
        /// <param name="numberOfFrames">
        /// The number of frames.
        /// </param>
        public void CreateUnifiedIonMobilityFile(string fileName, AnalogToDigitalConverterProperties adcProperties, int numberOfFrames)
        {
            this.unifiedIonMobilityFile.CreateNewUnifiedIonMobilityFile(fileName, adcProperties, numberOfFrames);               
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the digitizer.
        /// </summary>
        public IDigitizer Digitizer
        {
            get
            {
                return this.digitizer;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.digitizer, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// TODO The abort.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override async Task<bool> AbortAsync()
        {
            await this.Digitizer.ResetAsync();
            return true;
        }


        /// <summary>
        /// TODO The load.
        /// </summary>
        /// <param name="file">
        /// TODO The file.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override Task<bool> Load(string file)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO The load.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override Task<bool> Load()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO The save.
        /// </summary>
        /// <param name="file">
        /// TODO The file.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override Task<bool> SaveAsync(string file)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO The save.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO The run async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async override Task RunAsync()
        {
            await this.Digitizer.ReadAsync(this.frameActionBlock);
        }

        /// <summary>
        /// TODO The update.
        /// </summary>
        /// <param name="fileName">
        /// TODO The file name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task Update(string fileName = "")
        {
            await this.Digitizer.UpdateAsync(fileName);
        }

        #endregion
    }
}