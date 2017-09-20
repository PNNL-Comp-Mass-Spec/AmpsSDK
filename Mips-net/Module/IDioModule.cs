using System.Reactive;
using System.Threading.Tasks;
using Mips_net.Device;

namespace Mips_net.Module
{
    public interface IDioModule
    {
        Task<Unit> SetDigitalOutput(string channel, int state);
        Task<bool> GetDigitalState(string channel);
        Task<Unit> ReporInputChannelState(string channel,DigitalEdge edge);
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