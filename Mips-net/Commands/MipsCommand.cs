﻿namespace Mips.Commands
{
	public enum MipsCommand
	{
        NONE,
		GVER,
		GERR,
		GNAME,
		SNAME,
		UUID,
		ABOUT,
		SMREV,
		RESEST,
		SAVE,
		GCHAN,
		MUTE,
		ECHO,
		TRIGOUT,
		DELAY,
		GCMDS,
		GAENA,
		SAENA,
		THREADS,
		STHRDENA,
		SDEVADD,
		RDEV,
		RDEV2,
		TBLTSKENA,
		ADC,
		LEDOVRD,
		LED,
		DSPOFF,

		SSERIALNAV,
		LOADIMAGE,
		CHKIMAGE,
		BIMAGE,
		SSPND,
		GSPND,
		TRACE,
		STATUS,
		DIR,
		DEL,
		SAVEMOD,
		LPADMOD,
		SAVEALL,
		LOADALL,
		GET,
		PUT,
		GETEEPROM,
		PUTEEPROM,


		GWIDTH,
		SWIDTH,
		GFREQ,
		SFREQ,
		BURST,

		SDTRIGINP,
		SDTRIGDLY,
		GDTRIGDLY,
		SDTRIGPRD,
		GDTRIGPRD,
		SDTRIGRPT,
		GDTRIGRPT,
		SDTRIGMOD,
		SDTRIGENA,
		GDTRIGENA,

		SDCB,
		GDCB,
		GDCBV,
		SDCBOF,
		GDCBOF,
		GDCMIN,
		GDCMAX,
		SDCPWR,
		GDCPWR,
		SDCBALL,
		GDCBALL,
		GDCBALLV,
		SDCBDELTA,
		SDCBCHNS,
		SDCBONEOFF,
		DCBOFFRBENA,
		SDCBOFFENA,

		SDCBTEST,
		SDCBADCADD,
		SDCARST,

		SDCBPRO,
		GDCBPRO,
		ADCBPRO,
		CDCBPRO,
		TDCBPRO,
		TDCBSTP,

		SRFFRQ,
		SRFVLT,
		SRFDRV,
		GRFFRQ,
		GRFPPVP,
		GRFPPVN,
		GRFDRV,
		GRFVLT,
		GRFPWR,
		GRFALL,

		TUNERFCH,
		RETUNERFCH,

		SDIO,
		GDIO,
		RPT,
		MIRROR,
		SHV,
		GHV,
		GHVV,
		GHVI,
		GHVMAX,

		SHVPSUP,
		SHVENA,
		SHVDIS,
		GHVSTATUS,
		SHVNSUP,
		GHVITST,
		SHVITST,

		STBLDAT,
		STBLCLK,
		STBLTRG,
		TBLABRT,
		SMOD,
		TBLSTRT,
		TBLSTOP,
		GTBLFRQ,
		STBLNUM,
		GTBLNUM,
		STBLADV,
		GTBLADV,
		STBLVLT,
		GTVLVLT,
		STBLCNT,
		STBLDLY,
		SOFTLDAC,
		STBLREPLY,
		GTBLREPLY,

		MRECORD,
		MSTOP,
		MPLAY,
		MLIST,
		MDELETE,

		GTWF,
		STWF,
		GTWPV,
		STWPV,
		GTWG1V,
		STWG1V,
		GTWG2V,
		STWG2V,
		GTWSEQ,
		STWSEQ,
		GTWDIR,
		STWDIR,
		STWCTBL,
		GTWCTBL,
		GTWCMODE,
		STWCMODE,
		GTWCORDER,
		STWCORDER,
		GTWCTD,
		STWCTD,
		GTWCTC,
		STWCTC,
		GTWCTN,
		STWCTN,
		GTWCTNC,
		STWCTNC,
		TWCTRG,
		GTWCSW,
		STWCSW,
		STWCCLK,
		STWCMP,

		STWSSTRT,
		GTWSSTRT,
		STWSSTP,
		GTWSSTP,
		STWSTM,
		GTWSTM,
		STWSGO,
		STWSHLT,
		GTWSTA,
		STWSSTRTV,
		GTWSSTRTV,
		STWSSTPV,
		GTWSSTPV,

		SRFHPCAL,
		SRFHNCAL,

		GFLENA,
		SFLENA,
		GFLI,
		GFLAI,
		SFLI,
		GFLSV,
		GFLASV,
		SFLSV,
		GFLV,
		GFLPWR,
		GFLRT,
		SFLRT,
		GFLP1,
		SFLP1,
		GFLP2,
		SFLP2,
		GFLCY,
		SFLCY,
		GFLENAR,
		SFLENAR,
		RFLPARMS,
		SFLSRES,
		GFLSRES,
		GFLECUR,

		GHOST,
		GSSID,
		GPSWD,
		SHOST,
		SSSID,
		SPSWD,
		SWIFIENA,

		GEIP,
		SEIP,
		GEPORT,
		SEPORT,
		GEGATE,
		SEGATE,

		SARBMODE,
		GARBMODE,
		SWFREQ,
		GWFREQ,
		SWFVRNG,
		GWFVRNG,
		SWFVOFF,
		GWFVOFF,
		SWFVAUX,
		GWFVAUX,
		SWFDIS,
		SWFENA,
		SWFDIR,
		GWFDIR,
		SWFARB,
		GWFARB,
		SWFTYP,
		GWFTYP,
		SARBBUF,
		GARBBUF,
		SARBNUM,
		GARBNUM,
		SARBCHS,
		SARBCH,
		SACHRNG,

		SARBCTBL,
		GARBCTBL,
		GARBCMODE,
		SARBCMODE,
		GARBCORDER,
		SARBCORDER,
		GARBCTD,
		SARBCTD,
		GARBCTC,
		SARBCTC,
		GARBCTN,
		SARBCTN,
		GARBCTNC,
		SARBCTNC,
		TARBTRG,
		GARBCSW,
		SARBCSW,
		
		SARBCCLK,
		SARBCMP,

		ARBSYNC,
		
		SARBOFFA,
		GARBOFFA,
		SARBOFFB,
		GARBOFFB,

		SSER1ENA,
		GSER1ENA,

		SFMENA,
		GFMENA,
		SFMDRV,
		GFMDRV,
		GFMPWR,
		GFMPV,
		GFMNV,
		SFMLOCK,
		GFMLOCK,
		SFMSP,
		GFMSP,
		SFMTUNE,
		SFMTABRT,
		GFMTSTAT,

		GFMCV,
		GFMCVA,
		SFMBIAS,
		GFMBIAS,
		GFMBIASA,
		SFMOFF,
		GFMOFF,
		GFMOFFA,

		SFMCVSTART,
		GFMCVSTART,
		SFMCVEND,
		GFMCVEND,
		SFMDUR,
		GFMDUR,
		SFMLOOPS,
		GFMLOOPS,
		SFMSTRTLIN,
		GFMSTRTLIN,
		SFMSTPTM,
		GFMSTPTM,
		SFMSTEPS,
		GFMSTEPS,
		SFMSTRTSTP,
		GFMSTRTSTP

	}
}