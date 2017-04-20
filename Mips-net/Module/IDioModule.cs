using System.Reactive;
using System.Threading.Tasks;
using Mips.Device;

namespace Mips.Module
{
    public interface IDioModule
    {
        Task<Unit> SetDigitalOutput(string channel, bool state);
        Task<bool> GetDigitalState(string channel);
        Task<DigitalEdge> GetDigitalInputState(string channel);
        /// <summary>
        /// Input: Q-X
        /// Output: A-P
        /// </summary>
        /// <param name="input">Q-X</param>
        /// <param name="output">A-P</param>
        /// <returns></returns>
        Task<Unit> MirrorInputToOutput(string input, string output);
    }
}