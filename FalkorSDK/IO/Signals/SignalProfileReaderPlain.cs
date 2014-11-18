// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalProfileReaderPlain.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The signal profile reader plain.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FalkorSDK.IO.Signals
{
    using System;
    using System.IO;

    using FalkorSDK.Channel;
    using FalkorSDK.Data.Events;
    using FalkorSDK.Data.Signals;
    using FalkorSDK.Devices;

    /// <summary>
    /// TODO The signal profile reader plain.
    /// </summary>
    public class SignalProfileReaderPlain : ISignalProfileReader<SignalOutputProfile, IFalkorDevice>
    {
        #region Constants

        /// <summary>
        /// TODO The cons t_ colum n_ board.
        /// </summary>
        private const int CONST_COLUMN_BOARD = 1;

        /// <summary>
        /// TODO The cons t_ colum n_ channel.
        /// </summary>
        private const int CONST_COLUMN_CHANNEL = 3;

        /// <summary>
        /// TODO The cons t_ colum n_ name.
        /// </summary>
        private const int CONST_COLUMN_NAME = 2;

        /// <summary>
        /// TODO The cons t_ heade r_ lin e_ count.
        /// </summary>
        private const int CONST_HEADER_LINE_COUNT = 2;

        /// <summary>
        /// TODO The cons t_ require d_ columns.
        /// </summary>
        private const int CONST_REQUIRED_COLUMNS = 5;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalProfileReaderPlain"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="delimiter">
        /// The delimiter.
        /// </param>
        public SignalProfileReaderPlain(string delimiter="\t")
        {
            this.Delimiter = delimiter;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// String to split the data with.
        /// </summary>
        public string Delimiter { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="device">
        /// The device.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="SignalOutputProfile"/>.
        /// </returns>
        /// <exception cref="SignalProfileTableInvalidException">
        /// </exception>
        public SignalOutputProfile Read(IFalkorDevice device, string path)
        {
            var lines = File.ReadAllLines(path);
            var profile = new SignalOutputProfile(device as IInputOutputDevice);
            string[] splitChars = { this.Delimiter };

            if (lines.Length < 3)
            {
                throw new SignalProfileTableInvalidException("The signal profile does not have enough entries.");
            }

            var nameHeader = lines[0].Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            if (nameHeader.Length < 2)
            {
                throw new SignalProfileTableInvalidException(
                    "No name was specified in the header.  The file must contain the appropriate header.");
            }

            profile.Name = nameHeader[1];

            // Then skip over the header line
            for (var i = CONST_HEADER_LINE_COUNT; i < lines.Length; i++)
            {
                string[] datum = lines[i].Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

                // We expect 5 columns...go with the magic for now...until we formalize the Table.
                if (datum.Length == CONST_REQUIRED_COLUMNS)
                {
                    if (datum[4].ToLower() == "analogoutput")
                    {
                        // Reconstruct an analog signal from the name...we'll have to link these two up later. tho
                        // TODO: SourceBlock the real reference of a signal to the device's reference...
						/*
                        var signal = new AOChannel(
                            datum[CONST_COLUMN_NAME], 
                            Convert.ToInt32(datum[CONST_COLUMN_CHANNEL]));
                        double value = Convert.ToDouble(datum[3]);*/
                     //   var stepEvent = new AnalogStepEvent(signal, 0, value);
                    }
                }
            }

            return profile;
        }

        #endregion
    }
}