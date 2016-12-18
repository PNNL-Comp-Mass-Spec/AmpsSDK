using System.Reactive;
using System.Threading.Tasks;

namespace Mips.Module
{
    public interface IStandardModule
    {
        Task<string> GetVersion();
        Task<string> GetError(); // TODO: Create enum type for errors. 
        Task<string> GetName();
        Task<Unit> SetName(string name);
        Task<string> GetConfiguration();
        Task<Unit> SetModuleRevisionLevel(int board, string moduleAddress, string level);
        Task<Unit> Rest();
        Task<Unit> Save();

    }
}