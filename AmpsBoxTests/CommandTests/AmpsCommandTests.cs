

namespace AmpsBoxTests.CommandTests
{
    using System;
    using System.IO.Ports;
    using System.Linq;

    using AmpsBoxSdk.Data;
    using AmpsBoxSdk.Devices;

    using AmpsBoxTests.AmpsService;


    using NUnit.Framework;

    [TestFixture]
    public class AmpsCommandTests
    {
        [Test]
        public void TestOne()
        {
            
        }

        [Test]
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