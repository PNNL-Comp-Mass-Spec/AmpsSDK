﻿using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Mips.Device;

namespace Mips.Module
{
    public interface IDcBiasModule
    {
        Task<Unit> SetDcVoltage(string channel, double volts);
        Task<double> GetDcSetpoint(string channel);
        Task<double> GetDcReadback(string channel);
        Task<Unit> SetDcOffset(string channel, double offsetVolts);
        Task<double> GetDcOffset(string channel);
        Task<double> GetMinimumVoltage(string channel);
        Task<double> GetMaximumVoltage(string channel);
        Task<Unit> SetDcPowerState(State state);
        Task<State> GetDcPowerState();
        Task<Unit> SetAllDcChannels(IDictionary<string, double> channels);
        Task<IDictionary<string, double>> GetAllDcSetpoints();
        Task<IDictionary<string, double>> GetAllDcReadbacks();
        Task<Unit> SetUniversalOffset(double voltageOffset);
        Task<Unit> SetNumberOfChannelsOnboard(int board, int numberChannels);
        Task<Unit> UseSingleOffsetTwoModules(bool state);
        Task<Unit> EnableOffsetReadback(bool enableReadback);
        Task<Unit> EnableBoardOffsetOption(int board, bool enable);

    }
}