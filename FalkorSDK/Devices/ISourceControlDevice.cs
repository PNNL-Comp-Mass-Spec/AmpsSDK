namespace FalkorSDK.Devices
{
	using System.Threading.Tasks;

	public interface ISourceControlDevice
	{
		Task SetHeaterSetpoint(int temperature);

		void SetSourceVoltage(int voltage, int? channel=null);
	}
}