namespace AmpsBoxSdk.Commands
{
    using AmpsBoxSdk.Devices;

    public class ClockSync
    {
        private AmpsBoxCOMReader comReader;

        /// <summary>
        /// Sets the AMPS to use External Clock
        /// </summary>
        private const string CommandClockSyncExternal = "STBLCLK,EXT";

        /// <summary>
        /// Sets the AMPS to use Internal Clock
        /// </summary>
        private const string CommandClockSyncInternal = "STBLCLK,INT";

        public ClockSync(AmpsBoxCOMReader comReader)
        {
            this.comReader = comReader;
        }

        public void SetClockExternal()
        {
            this.comReader.Write(CommandClockSyncExternal);
        }
    }
}