using Mips.Device;
using System.Reactive;
using System.Threading.Tasks;

namespace Mips.Module
{
	public interface IFAIMSModule
	{
		Task<Unit> SetPositiveOutput(int slope,int offset);
		Task<Unit> SetNegativeOutput(int slope, int offset);
		Task<Unit> SetEnableWaveformGeneration(Status status);//
		Task<Status> GetEnableWaveformGeneration();
		Task<Unit> SetDriveLevelPercent(double level);//
		Task<double> GetDriveLevelPercent();
		Task<double> GetWaveformGenerationPower();
		Task<double> GetPositivePeakOutputVoltageKW();
		Task<double> GetNegativePeakOutputVoltageKW();
		Task<Unit> SetEnableOutputVoltageLocking(Status status);//
		Task<Status> GetOutputVoltageLockStatus();
		Task<Unit> SetOutputVoltageSetPointKV(double voltage);//
		Task<Unit> SetEnableSystemAutotune();
		Task<Unit> AbortSystemAutotune();
		Task<string> GetSystemAutotuneStatus();
		Task<Unit> SetCVOutputDcVoltageSetpoint(double voltage);//
		Task<double> GetCVOutputDcVoltageSetpoint();
		Task<double> GetCvOutputVoltageReadback();
		Task<Unit> SetBiasOutputDcVoltageSetpoint(double voltage);
		Task<double> GetBiasOutputDcVoltageSetpoint();
		Task<double> GetBiasOutputDcVoltagReadback();
		Task<Unit> SetOffsetOutputVoltageSetpoint(double voltage);
		Task<double> GetOffsetOutputVoltageSetpoint();
		Task<double> GetOffsetOutputVoltageReadback();
		Task<Unit> SetCVScanStartVoltage(double voltage);
		Task<double> GetCVScanStartVoltage();
		Task<Unit> SetCVScanEndVoltage(double voltage);
		Task<double> GetCVScanEndVoltage();
		Task<Unit> SetScanDuration(double seconds);
		Task<double> GetScanDuration();
		Task<Unit> SetScanLoopCount(int count);
		Task<int> GetScanLoopCount();
		Task<Unit> StartLinearScan(Status status);
		Task<Status> GetLinearScanStatus();
		Task<Unit> SetStepScanDuration(int milliseconds);
		Task<int> GetStepScanDuration();
		Task<Unit> SetStepScanStepCount(int count);
		Task<int> GetStepScanStepCount();
		Task<Unit> StartStepScan(Status status);
		Task<Status> GetStepScanModeStatus();
	}
}