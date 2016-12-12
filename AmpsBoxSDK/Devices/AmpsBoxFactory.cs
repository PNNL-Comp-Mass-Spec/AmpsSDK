using System.IO.Ports;

namespace AmpsBoxSdk.Devices
{
    public class AmpsBoxFactory
    {
        public AmpsBox CreateAmpsBox(IAmpsBoxCommunicator communicator)
        {
            return new AmpsBox(communicator);
        }
    }
}