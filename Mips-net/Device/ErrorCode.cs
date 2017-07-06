using System.Runtime.Serialization;

namespace Mips_net.Device
{
	[DataContract]
	public enum ErrorCode
	{
		[EnumMember]
		Nominal = 0,
		
		[EnumMember]
		BadCommand = 1,

		
		[EnumMember]
		BadArg = 2,

		
		[EnumMember]
		LocalAlready = 3,

		
		[EnumMember]
		TableAlready = 4,

		
		[EnumMember]
		NoTableLoaded = 5,

		
		[EnumMember]
		NoTableMode = 6,

		
		[EnumMember]
		TableNotReady = 7,

		
		[EnumMember]
		TokenTimeout = 8,

		
		[EnumMember]
		ExpectedColon = 9,

		
		[EnumMember]
		TableTooBig = 10,

		
		[EnumMember]
		ChannelLowOrBoard = 11,

		
		[EnumMember]
		ChannelHighOrBoard = 12,

		
		[EnumMember]
		ChannelHigh = 13,

		
		[EnumMember]
		BoardLowOrBoard = 14,

		
		[EnumMember]
		BoardHighOrBoard = 15,

		
		[EnumMember]
		BoardHigh = 16,

		
		[EnumMember]
		BoardNotSupport = 17,

		
		[EnumMember]
		BadBaud = 18,

		
		[EnumMember]
		ExpectedComma = 19,

		
		[EnumMember]
		Inception = 20,

		[EnumMember]
		MissingOpenBracket = 21,

		
		[EnumMember]
		InvalidChannel = 22,

		
		[EnumMember]
		DioHardwareNotPresent = 23,

		[EnumMember]
		TemperatureRange = 24,
		
		[EnumMember]
		EsiHvOutOfRange = 25,
		
		[EnumMember]
		TemperatureControlLoopGain = 26,

		[EnumMember]
		NotLocMode = 27,

		[EnumMember]
		WrongTriggerMode = 28,

		[EnumMember]
		CannotFindEntry = 29,

		[EnumMember]
		ArgumentOutOfRange = 101,

		[EnumMember]
		NoSdCard = 102,

		[EnumMember]
		FailCreateFile = 103,

		[EnumMember]
		FileNameTooLong = 104,

		[EnumMember]
		FailOpenFile = 105,

		[EnumMember]
		FailDeleteFile = 106,

		[EnumMember]
		NotSupportedInRevision = 107
	}
}