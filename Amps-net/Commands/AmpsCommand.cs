namespace AmpsBoxSdk.Commands
{
    public enum AmpsCommand
    {
        NONE,
        GVER,
        GCMDS,
        GERR,
        SAVE,
        SNAME,
        GNAME,
        SBAUD,
        RESET,

        SRFFRQ,
        GRFFRQ,
        SRFDRV,
        GRFDRV,
        GCHAN,

        SDCB,
        GDCB,
        GDCBV,
        GDCBI,
        SDCBOF,
        GDCBOF,

        SPHV,
        SNHV,
        GPHVV,
        GNHVV,

        SHTR,
        SHTRTMP,
        GHTRTMP,
        GHTRTC,
        SHTRGAIN,
		GHTRGAIN,


        STBLDAT,
        STBLCLK,
        STBLTRG,
        TBLSTRT,
        TBLSTOP,
        TBLABRT,
        SMOD,
        TBLRPT,
        STBLVLT,
        GTBLVLT,



        SDIO,
        GDIO,
        SDIODR,
        GDIODR
    }
}