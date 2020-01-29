using AmpsBoxSdk.Devices;
using FTD2XX_NET;
using Xunit;
using Xunit.Abstractions;

namespace Amps.SDK.Tests.Devices
{
    public class FtdiCommunicationTests
    {
        private IAmpsBox box;
        private ITestOutputHelper output;

        public FtdiCommunicationTests(ITestOutputHelper output)
        {
            this.output = output;
            var ftdi = new FTDI();
            var deviceCount = 0u;
            ftdi.GetNumberOfDevices(ref deviceCount);
            var deviceArray = new FTDI.FT_DEVICE_INFO_NODE[deviceCount];
            ftdi.GetDeviceList(deviceArray);
            box = AmpsBoxFactory.CreateAmpsBox(deviceArray[0].SerialNumber);
        }

        [Fact]
        public void GetAmpsNameProperty()
        {
            var name = box.Name;
            output.WriteLine(name);
        }
    }
}