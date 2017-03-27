using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using AmpsBoxSdk.Devices;

namespace AmpsBoxSdk.Modules
{
    public interface IStandardModule
    {
        Task<string> GetVersion();
        Task<ErrorCodes> GetError();
        Task<string> GetName();
        Task<Unit> SetName(string name);
        Task<Unit> Reset();
        Task<Unit> Save();
        Task<IEnumerable<string>>  GetCommands();
        Task<Unit> SetSerialBaudRate(int baudRate);
    }
}