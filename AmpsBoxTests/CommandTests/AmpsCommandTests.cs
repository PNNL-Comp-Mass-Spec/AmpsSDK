namespace AmpsBoxTests.CommandTests
{
    using System;
    using System.IO.Ports;
    using System.Linq;

    using AmpsBoxSdk.Data;
    using AmpsBoxSdk.Devices;

    using AmpsBoxTests.AmpsService;

    using FalkorSDK.Data.Signals;
    using FalkorSDK.IO.Ports;

    using NUnit.Framework;

    [TestFixture]
    public class AmpsCommandTests
    {
        [Test]
        public void TestOne()
        {
            var table = new AmpsSignalTable();
            table.AddTimePoint(new PsgPoint("A", 0, new LoopData()));
            table.AddTimePoint(new PsgPoint("B", 20, new LoopData()));
            table.AddTimePoint(new PsgPoint("C", 30, new LoopData() { DoLoop = true, LoopCount = 10, LoopToName = table[0].Name}));
            table[0].ReferenceTimePoint(table.Points.LastOrDefault(), 10);

            var formattedTable = AmpsBoxSignalTableCommandFormatter.FormatTable(table, new AmpsClockConverter(1e6));
            Console.WriteLine(formattedTable);
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