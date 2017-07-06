using System.Reactive;
using System.Threading.Tasks;

namespace Mips_net.Module
{
    public interface IMacroModule
    {
	    Task<Unit> RecordMacro(string name);
	    Task<Unit> StopMacro();
	    Task<Unit> PlayMacro(string name);
	    Task<string> ListMacro();
	    Task<Unit> DeleteMacro(string name);


    }
}