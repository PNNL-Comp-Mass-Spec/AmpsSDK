namespace FalkorSDK.IO.Signals
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using FalkorSDK.Channel;

    class AnalogOutputDcChannelReader : IChannelReader<AOChannel>
    {
        public IEnumerable<AOChannel> Read(string path)
        {
            // TODO: catch fileNotFound exceptions?
            var document = XDocument.Load(path);

            // TODO: Catch possible null reference exception from this?
            var root = document.Root;

            var query =
                root.Element("Signals")
                    .Elements()
                    .Select(
                        signal =>
                        new { Name = (string)signal.Attribute("name"), Address = (int) signal.Attribute("channel"), IsRf = (bool)signal.Attribute("isrf")});

            return query.Select(signal => new AOChannel(signal.Address, signal.Name, "", signal.IsRf)).ToList();
        }
    }
}