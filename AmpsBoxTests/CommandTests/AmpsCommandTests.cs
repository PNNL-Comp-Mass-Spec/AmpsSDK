

using Xunit;

namespace AmpsBoxTests.CommandTests
{
    using System;
    using System.IO.Ports;
    using System.Linq;

    using AmpsBoxSdk.Data;
    using AmpsBoxSdk.Devices;

    using AmpsBoxTests.AmpsService;

    public class AmpsCommandTests
    {
        [Fact]
        public void TestOne()
        {
            
        }

        [Fact]
        public void ConnectAmps()
        {

            AmpsService.AmpsCommunicationClient client = new AmpsCommunicationClient();
            client.Open();

            var name = "Device1";
            var response = client.OpenCommunication(name, "COM25");
            Console.WriteLine(response);
            var version = client.GetVersion(name);
            Console.WriteLine(version);
            client.CloseAmpsBox(name);
        }

    }
}