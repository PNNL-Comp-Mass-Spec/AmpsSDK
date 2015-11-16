// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmpsCommandProvider.cs" company="">
//   
// </copyright>
// <summary>
//   Provides a resource with commands to communicate to the AMPS Box with
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AmpsBoxSdk.Commands
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Provides a resource with commands to communicate to the AMPS Box with
    /// </summary>
    public abstract class AmpsCommandProvider
    {
        /// <summary>
        /// The encoding.
        /// </summary>
        private ASCIIEncoding encoding;
        #region Constants

        /// <summary>
        /// 16MHz internal clock of the AMPS box by default
        /// </summary>
        public const int DefaultInternalClock = 16000000;

        /// <summary>
        /// TODO The command parameter separator.
        /// </summary>
        private const string CommandParameterSeparator = ",";

        /// <summary>
        /// Value required by the AMPS box to convert the clock to ticks
        /// </summary>
        private const int ConstClockDivider = 1024;

        /// <summary>
        /// The number of characters that the AMPS box sends back
        /// </summary>
        private const int ConstNumCharEndPadding = 2;

        /// <summary>
        /// Default End of Line
        /// </summary>
        private const string END_OF_LINE = "\n";

        /// <summary>
        /// TODO The table finish response.
        /// </summary>
        private const string TableFinishResponse = "TBLCMPLT";

        #endregion

        #region Fields

        /// <summary>
        /// Map of commands 
        /// </summary>
        protected Dictionary<AmpsCommandType, AmpsCommand> m_commands;

        protected Dictionary<MipsCommandType, AmpsCommand> mipsCommands; 

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmpsCommandProvider"/> class. 
        /// Constructor
        /// </summary>
        protected AmpsCommandProvider()
        {
            this.m_commands                 = new Dictionary<AmpsCommandType, AmpsCommand>();
            this.mipsCommands = new Dictionary<MipsCommandType, AmpsCommand>();
            this.encoding                   = new ASCIIEncoding();
            this.ErrorResponse              = 0x15;
            this.InternalClock              = DefaultInternalClock;
            this.EndOfLine                  = END_OF_LINE;
            this.OkResponse                 = 0x06;
            this.CommandSeparator           = CommandParameterSeparator;
            this.NumberOfPaddingCharacters  = ConstNumCharEndPadding;
            this.TableResponse              = TableFinishResponse;

            this.GenerateCommands();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the command separator.
        /// </summary>
        public string CommandSeparator { get; protected set; }

        /// <summary>
        /// Gets or sets the end of line.
        /// </summary>
        public string EndOfLine { get; protected set; }

        /// <summary>
        /// Gets or sets the error response.
        /// </summary>
        public int ErrorResponse { get; protected set; }

        /// <summary>
        /// Gets or sets the internal clock.
        /// </summary>
        public int InternalClock { get; protected set; }

        /// <summary>
        /// Gets the number of characters that the amps box will send back in addition to data
        /// </summary>
        public int NumberOfPaddingCharacters { get; set; }

        /// <summary>
        /// Gets or sets the ok response.
        /// </summary>
        public int OkResponse { get; protected set; }

        /// <summary>
        /// Gets or sets the table response.
        /// </summary>
        public string TableResponse { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the command if it exists.
        /// </summary>
        /// <param name="commandType">
        /// The command Type.
        /// </param>
        /// <returns>
        /// String version of the command
        /// </returns>
        public AmpsCommand GetCommand(AmpsCommandType commandType)
        {
            bool hasCommand = this.m_commands.ContainsKey(commandType);
            if (!hasCommand)
            {
                throw new AmpsCommandNotSupported(
                    string.Format("The command {0} is not supported for the version of firmware loaded.", commandType));
            }

            return this.m_commands[commandType];
        }

        /// <summary>
        /// Returns the command if it exists.
        /// </summary>
        /// <param name="commandType">
        /// The command Type.
        /// </param>
        /// <returns>
        /// String version of the command
        /// </returns>
        public AmpsCommand GetCommand(MipsCommandType commandType)
        {
            bool hasCommand = this.mipsCommands.ContainsKey(commandType);
            if (!hasCommand)
            {
                throw new AmpsCommandNotSupported(
                    string.Format("The command {0} is not supported for the version of firmware loaded.", commandType));
            }

            return this.mipsCommands[commandType];
        }

        /// <summary>
        /// Returns the string of the latest version this provider supports.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public abstract string GetSupportedVersions();

        #endregion

        #region Methods

        /// <summary>
        /// Creates a list of commands to be created.
        /// </summary>
        protected abstract void GenerateCommands();

        #endregion
    }
}